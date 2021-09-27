using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoSingleton<PathScript>
{
    public GameObject meshObject;
    private List<Vector3> bottom_points = new List<Vector3>();
    private List<Vector3> top_points = new List<Vector3>();
    private List<Vector3> right_point = new List<Vector3>();
    private List<Vector3> left_point = new List<Vector3>();
    private Vector3[] forward_point = new Vector3[4];
    private Vector3[] back_point = new Vector3[4];
    private MeshFilter bottom_meshFilter;
    private MeshFilter top_meshFilter;
    private MeshFilter right_meshFilter;
    private MeshFilter left_meshFilter;
    private MeshFilter back_meshFilter;
    private MeshFilter foward_meshFilter;

    private MeshFilter meshFilterTotal;
    private GameObject meshObjectInstance;

    int[] triangles_back = new int[]{
            0,1,2,
            0,2,3
        };
    int[] triangles_foward = new int[]{
            2,1,0,
            3,2,0
        };

    public Material Mat;
    private Vector3 GeneralBottomPostion(Vector3 vector, float hight)
    {
        var rp = vector;
        rp.y -= hight;
        return rp;
    }

    public void DrawLine(LineRenderer line, Vector3 meshPosition, float hight)
    {
        var caret = new GameObject();
        Vector3 top_left, top_right, bot_left, bot_right;
        for (var i = 0; i < line.positionCount - 1; i++)
        {
            caret.transform.position = line.GetPosition(i);
            caret.transform.LookAt(line.GetPosition(i + 1));
            top_right = caret.transform.position + caret.transform.right * line.startWidth * 0.5f;
            bot_right = GeneralBottomPostion(top_right, hight);
            top_left = caret.transform.position - caret.transform.right * line.startWidth * 0.5f;
            bot_left = GeneralBottomPostion(top_left, hight);

            if (!bottom_points.Contains(top_left))
                bottom_points.Add(top_left);
            if (!bottom_points.Contains(top_right))
                bottom_points.Add(top_right);

            if (!top_points.Contains(bot_right))
                top_points.Add(bot_right);
            if (!top_points.Contains(bot_left))
                top_points.Add(bot_left);

            if (!left_point.Contains(bot_left))
                left_point.Add(bot_left);
            if (!left_point.Contains(top_left))
                left_point.Add(top_left);

            if (!right_point.Contains(top_right))
                right_point.Add(top_right);
            if (!right_point.Contains(bot_right))
                right_point.Add(bot_right);
        }

        //// First point looks backwards and reverses
        back_point[0] = top_points[0];
        back_point[1] = top_points[1];
        back_point[2] = bottom_points[0];
        back_point[3] = bottom_points[1];

        // Last point looks backwards and reverses
        caret.transform.position = line.GetPosition(line.positionCount - 1);
        caret.transform.LookAt(line.GetPosition(line.positionCount - 2));
        top_right = caret.transform.position - caret.transform.right * line.startWidth * 0.5f;
        bot_right = GeneralBottomPostion(top_right, hight);
        top_left = caret.transform.position + caret.transform.right * line.startWidth * 0.5f;
        bot_left = GeneralBottomPostion(top_left, hight);

        if (!bottom_points.Contains(top_left))
            bottom_points.Add(top_left);
        if (!bottom_points.Contains(top_right))
            bottom_points.Add(top_right);

        if (!top_points.Contains(bot_right))
            top_points.Add(bot_right);
        if (!top_points.Contains(bot_left))
            top_points.Add(bot_left);

        if (!left_point.Contains(bot_left))
            left_point.Add(bot_left);
        if (!left_point.Contains(top_left))
            left_point.Add(top_left);

        if (!right_point.Contains(top_right))
            right_point.Add(top_right);
        if (!right_point.Contains(bot_right))
            right_point.Add(bot_right);

        forward_point[0] = top_points[top_points.Count - 2];
        forward_point[1] = top_points[top_points.Count - 1];
        forward_point[2] = bottom_points[bottom_points.Count - 2];
        forward_point[3] = bottom_points[bottom_points.Count - 1];


        caret.transform.parent = transform;

        if (bottom_meshFilter == null)
        {
            var obj = Instantiate(meshObject);
            obj.name = "bottom";
            bottom_meshFilter = obj.GetComponent<MeshFilter>();
        }    

        if (top_meshFilter == null)
        {
            var obj = Instantiate(meshObject);
            obj.name = "top";
            top_meshFilter = obj.GetComponent<MeshFilter>();
        }

        if (left_meshFilter == null)
        {
            var obj = Instantiate(meshObject);
            obj.name = "left";
            left_meshFilter = obj.GetComponent<MeshFilter>();
        }

        if (right_meshFilter == null)
        {
            var obj = Instantiate(meshObject);
            obj.name = "right";
            right_meshFilter = obj.GetComponent<MeshFilter>();
        }

        if (back_meshFilter == null)
        {
            var obj = Instantiate(meshObject);
            obj.name = "back";
            back_meshFilter = obj.GetComponent<MeshFilter>();
        }

        if (foward_meshFilter == null)
        {
            var obj = Instantiate(meshObject);
            obj.name = "foward";
            foward_meshFilter = obj.GetComponent<MeshFilter>();
        }

        if (meshFilterTotal == null)
        {
            meshObjectInstance = this.gameObject;
            meshObjectInstance.name = "center";
            meshFilterTotal = meshObjectInstance.GetComponent<MeshFilter>();
        }


        bottom_meshFilter.gameObject.SetActive(true);
        DrawMesh(bottom_points, Vector3.zero, bottom_meshFilter);
        top_meshFilter.gameObject.SetActive(true);
        DrawMesh(top_points, Vector3.zero, top_meshFilter);
        left_meshFilter.gameObject.SetActive(true);
        DrawMesh(left_point, Vector3.zero, left_meshFilter);
        right_meshFilter.gameObject.SetActive(true);
        DrawMesh(right_point, Vector3.zero, right_meshFilter);
        back_meshFilter.gameObject.SetActive(true);
        DrawMeshWithTriangle(back_point, Vector3.zero, back_meshFilter, triangles_back);
        foward_meshFilter.gameObject.SetActive(true);
        DrawMeshWithTriangle(forward_point, Vector3.zero, foward_meshFilter, triangles_foward);

        meshFilterTotal.transform.position = meshPosition;

        CombineInstance[] combine = new CombineInstance[6];
        combine[0].mesh = bottom_meshFilter.sharedMesh;
        combine[0].transform = bottom_meshFilter.transform.localToWorldMatrix;
        bottom_meshFilter.gameObject.SetActive(false);

        combine[1].mesh = top_meshFilter.sharedMesh;
        combine[1].transform = top_meshFilter.transform.localToWorldMatrix;
        top_meshFilter.gameObject.SetActive(false);

        combine[2].mesh = left_meshFilter.sharedMesh;
        combine[2].transform = left_meshFilter.transform.localToWorldMatrix;
        left_meshFilter.gameObject.SetActive(false);

        combine[3].mesh = right_meshFilter.sharedMesh;
        combine[3].transform = right_meshFilter.transform.localToWorldMatrix;
        right_meshFilter.gameObject.SetActive(false);

        combine[4].mesh = back_meshFilter.sharedMesh;
        combine[4].transform = back_meshFilter.transform.localToWorldMatrix;
        back_meshFilter.gameObject.SetActive(false);

        combine[5].mesh = foward_meshFilter.sharedMesh;
        combine[5].transform = foward_meshFilter.transform.localToWorldMatrix;
        foward_meshFilter.gameObject.SetActive(false);

        meshFilterTotal.mesh.Clear();
        meshFilterTotal.mesh.CombineMeshes(combine);
    }

    MeshCollider meshColTotal;
    public void CompleteLine()
    {
        meshColTotal = meshObjectInstance.AddComponent<MeshCollider>();
        meshColTotal.sharedMesh = meshFilterTotal.sharedMesh;
    }    

    private void DrawMeshWithTriangle(Vector3[] verticies, Vector3 meshPosition, MeshFilter meshFilter, int[] triangles)
    {
        Mesh mesh = meshFilter.mesh;
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.transform.position = meshPosition;
    }    

    private void DrawMesh(List<Vector3> meshPoints, Vector3 meshPosition, MeshFilter meshFilter)
    {
        Vector3[] verticies = new Vector3[meshPoints.Count];

        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i] = meshPoints[i];
        }

        int size = 6;

        int[] triangles = new int[((meshPoints.Count / 2) - 1) * size];

        //Works on linear patterns tn = bn+c
        int position = size;
        for (int i = 0; i < (triangles.Length / size); i++)
        {
            triangles[i * position] = 2 * i;
            triangles[i * position + 1] = 2 * i + 3;
            triangles[i * position + 2] = 2 * i + 1;

            triangles[i * position + 3] = 2 * i;
            triangles[i * position + 4] = (2 * i + 3) - 1;
            triangles[i * position + 5] = (2 * i + 1) + 2;
        }


        Mesh mesh = meshFilter.mesh;
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.transform.position = meshPosition;
    }
}