using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoSingleton<PathScript>
{
    public GameObject meshObject;
    private List<Vector3> points = new List<Vector3>();
    private MeshFilter meshFilter;

    //test
    private MeshFilter MeshFilter;
    private Mesh Mesh;

    [Header("show on inspector")]
    public int[] ListTriangles;
    public Vector3[] ListVerticies;

    public List<Vector3> ListVerBottoms = new List<Vector3>();
    public Material Mat;

    private void Start()
    {
        CreateMesh();
    }

    private void CreateMesh()
    {
        Mesh = new Mesh();
        Mesh.name = "GeneralMesh";

        Mesh.vertices = GeneralVerts();
        Mesh.triangles = GeneralTries();

        Mesh.RecalculateNormals();
        //Mesh.RecalculateBounds();

        GameObject MeshObject = new GameObject();
        MeshObject.name = "Mesh";

        var mr = MeshObject.AddComponent<MeshRenderer>();
        mr.materials[0] = Mat;

        MeshFilter = MeshObject.AddComponent<MeshFilter>();
        MeshFilter.mesh = Mesh;
    }    

    private Vector3[] GeneralVerts()
    {
        return new Vector3[]
        {
            //bottom
            new Vector3(0.5399822f, 3, 4.089711f),
            new Vector3(0.8399822f, 3, 4.089711f),
            new Vector3(0.7426509f, 3, 0.7426509f),
            new Vector3(0.6373134f, 3, 4.23016f),

            //top
            new Vector3(0.5399822f, 1, 4.089711f),
            new Vector3(0.8399822f, 1, 4.089711f),
            new Vector3(0.7426509f, 1, 0.7426509f),
            new Vector3(0.6373134f, 1, 4.23016f),

            ////left
            //new Vector3(-1, 0, 1),
            //new Vector3(-1, 0, -1),
            //new Vector3(-1, 2, 1),
            //new Vector3(-1, 2, -1),

            ////right
            //new Vector3(1, 0, 1),
            //new Vector3(1, 0, -1),
            //new Vector3(1, 2, 1),
            //new Vector3(1, 2, -1),

            ////front
            //new Vector3(1, 0, -1),
            //new Vector3(-1, 0, -1),
            //new Vector3(1, 2, -1),
            //new Vector3(-1, 2, -1),

            ////back
            //new Vector3(-1, 0, 1),
            //new Vector3(1, 0, 1),
            //new Vector3(-1, 2, 1),
            //new Vector3(1, 2, 1),
        };
    }    

    private int[] GeneralTries()
    {
        return new int[]
        {
            //bottom
            0,1,3,
            0,2,3,
            ////top
            //4,5,6,
            //4,6,7,
            ////left
            //9,10,11,
            //8,10,9,
            ////right
            //12,13,15,
            //14,12,15,
            ////front
            //16,17,19,
            //18,16,19,
            ////back
            //20,21,23,
            //22,20,23
        };
    }   
    
    private Vector3 GeneralBottomPostion(Vector3 vector)
    {
        var rp = vector;
        rp.y -= 2;
        return rp;
    }    

    public void DrawLine(LineRenderer line, Vector3 meshPosition)
    {
        var caret = new GameObject();
        Vector3 top_left, top_right, bot_left, bot_right;
        for (var i = 0; i < line.positionCount - 1; i++)
        {
            caret.transform.position = line.GetPosition(i);
            caret.transform.LookAt(line.GetPosition(i + 1));
            top_right = caret.transform.position + caret.transform.right * line.startWidth * 0.5f;
            bot_right = GeneralBottomPostion(top_right);
            top_left = caret.transform.position - caret.transform.right * line.startWidth * 0.5f;
            bot_left = GeneralBottomPostion(top_left);

            if (!points.Contains(top_left))
                points.Add(top_left);
            if (!points.Contains(top_right))
                points.Add(top_right);

            if (!ListVerBottoms.Contains(bot_left))
                ListVerBottoms.Add(bot_left);
            if (!ListVerBottoms.Contains(bot_right))
                ListVerBottoms.Add(bot_right);
        }

        // Last point looks backwards and reverses
        caret.transform.position = line.GetPosition(line.positionCount - 1);
        caret.transform.LookAt(line.GetPosition(line.positionCount - 2));
        top_right = caret.transform.position - caret.transform.right * line.startWidth * 0.5f;
        bot_right = GeneralBottomPostion(top_right);
        top_left = caret.transform.position + caret.transform.right * line.startWidth * 0.5f;
        bot_left = GeneralBottomPostion(top_left);

        if (!points.Contains(top_left))
            points.Add(top_left);
        if (!points.Contains(top_right))
            points.Add(top_right);

        if (!ListVerBottoms.Contains(bot_left))
            ListVerBottoms.Add(bot_left);
        if (!ListVerBottoms.Contains(bot_right))
            ListVerBottoms.Add(bot_right);

        caret.transform.parent = transform;

        DrawMesh(points, meshPosition);
    }

    private void DrawMesh(List<Vector3> meshPoints, Vector3 meshPosition)
    {
        Vector3[] verticies = new Vector3[meshPoints.Count];

        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i] = meshPoints[i];
        }

        int[] triangles = new int[((meshPoints.Count / 2) - 1) * 6];

        //Works on linear patterns tn = bn+c
        int position = 6;
        for (int i = 0; i < (triangles.Length / 6); i++)
        {
            triangles[i * position] = 2 * i;
            triangles[i * position + 3] = 2 * i;

            triangles[i * position + 1] = 2 * i + 3;
            triangles[i * position + 4] = (2 * i + 3) - 1;

            triangles[i * position + 2] = 2 * i + 1;
            triangles[i * position + 5] = (2 * i + 1) + 2;
        }

        if (meshFilter == null)
            meshFilter = Instantiate(meshObject).GetComponent<MeshFilter>();


        Mesh mesh = meshFilter.mesh;
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        ListTriangles = triangles;
        ListVerticies = verticies;

        meshFilter.transform.position = meshPosition;
    }
}