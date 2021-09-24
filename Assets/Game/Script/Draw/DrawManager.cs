using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject linePrefabs;
    private GameObject currentLine;
    private LineRenderer lineRenderer;
    private List<Vector3> fingerPositions;
    private Vector3 defaultPostion = Vector3.zero;

    private void Start()
    {

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempFingerPos.y = 3;
            if(Vector3.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .2f)
            {
                UpdateLine(tempFingerPos);
            }
        }
        //if(Input.GetMouseButtonUp(0))
        //{
        //    CompleteLine();
        //}    
    }

    private void CreateLine()
    {
        currentLine = Instantiate(linePrefabs, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();

        fingerPositions = new List<Vector3>();
        var startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startPos.y = 3;
        defaultPostion.y = 3;
        fingerPositions.Add(startPos);
        fingerPositions.Add(startPos);
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
    }

    private void UpdateLine(Vector3 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        //PathScript.Instance.DrawLine(lineRenderer, defaultPostion);
        CompleteLine();
    }

    //public float ScaleX = 1.0f;
    //public float ScaleY = 1.0f;
    //public float ScaleZ = 1.0f;
    //public bool RecalculateNormals = false;
    //private Vector3[] _baseVertices;
    Mesh mesh = null;
    private bool IsInit = false;

    public int[] triangles;
    public Vector3[] vertices;

    private void CompleteLine()
    {
        if(!IsInit)
        {
            IsInit = true;
            mesh = new Mesh();
            mesh.name = "GeneralMesh";
            GameObject MeshObject = new GameObject();
            MeshObject.name = "Mesh";
            var mr = MeshObject.AddComponent<MeshRenderer>();
            var MeshFilter = MeshObject.AddComponent<MeshFilter>();
            MeshFilter.mesh = mesh;
        }    

        lineRenderer.BakeMesh(mesh, Camera.main, false);

        triangles = mesh.triangles;
        vertices = mesh.vertices;

        //var mesh = MeshObject.GetComponent<MeshFilter>().mesh;
        //if (_baseVertices == null)
        //    _baseVertices = mesh.vertices;
        //var vertices = new Vector3[_baseVertices.Length];
        //for (var i = 0; i < vertices.Length; i++)
        //{
        //    var vertex = _baseVertices[i];
        //    vertex.x = vertex.x * ScaleX;
        //    vertex.y = vertex.y * ScaleY;
        //    vertex.z = vertex.z * ScaleZ;
        //    vertices[i] = vertex;
        //}
        //mesh.vertices = vertices;
        //if (RecalculateNormals)
        //    mesh.RecalculateNormals();
        //mesh.RecalculateBounds();
    }
}
