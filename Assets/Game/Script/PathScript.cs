using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoSingletonGlobal<PathScript>
{
    public GameObject meshObject;
    public List<Vector3> points = new List<Vector3>();
    private MeshFilter meshFilter;

    public void DrawLine(LineRenderer line, Vector3 meshPosition)
    {
        var caret = new GameObject();
        Vector3 left, right;
        for (var i = 0; i < line.positionCount - 1; i++)
        {
            caret.transform.position = line.GetPosition(i);
            caret.transform.LookAt(line.GetPosition(i + 1));
            right = caret.transform.position + caret.transform.right * line.startWidth * 0.5f;
            left = caret.transform.position - caret.transform.right * line.startWidth * 0.5f;

            if (!points.Contains(left))
                points.Add(left);
            if (!points.Contains(right))
                points.Add(right);
        }

        // Last point looks backwards and reverses
        caret.transform.position = line.GetPosition(line.positionCount - 1);
        caret.transform.LookAt(line.GetPosition(line.positionCount - 2));
        right = caret.transform.position - caret.transform.right * line.startWidth * 0.5f;
        left = caret.transform.position + caret.transform.right * line.startWidth * 0.5f;

        if (!points.Contains(left))
            points.Add(left);
        if (!points.Contains(right))
            points.Add(right);
        caret.transform.parent = transform;
        DrawMesh(meshPosition);
    }

    private void DrawMesh(Vector3 meshPosition)
    {
        Vector3[] verticies = new Vector3[points.Count];

        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i] = points[i];
        }

        int[] triangles = new int[((points.Count / 2) - 1) * 6];

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

        meshObject.transform.position = meshPosition;
    }
}