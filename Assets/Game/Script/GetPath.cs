using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPath : MonoBehaviour
{
    // Start is called before the first frame update
    private Mesh mesh;
    private MeshFilter meshFilter;

    public int[] triangles;
    public Vector3[] vertices;
    void Start()
    {
        meshFilter = this.GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        triangles = mesh.triangles;
        vertices = mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        mesh.triangles = triangles;
        mesh.vertices = vertices;

        mesh.RecalculateNormals();
    }
}
