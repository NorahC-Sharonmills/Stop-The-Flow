using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject linePrefabs;
    private GameObject currentLine;
    private LineRenderer lineRenderer;
    private GameObject currentWaterLine;
    private LineRenderer lineWaterRender;
    private List<Vector3> fingerPositions;
    private List<Vector2> fingerWaterPositions;

    [SerializeField] private Camera m_Camera;
    [SerializeField] private Camera m_WaterCamera;

    public float distance = 0.1f;
    public float hight = 0.5f;

    private Vector3 position = Vector3.zero;

    private bool IsComplete = false;

    private void Start()
    {
        IsComplete = false;
    }

    private void Update()
    {
        if (IsComplete)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            CreateLine();
            CreatedLine2d();
        }
        if(Input.GetMouseButton(0))
        {
            Vector2 tempFingerWaterPos = m_WaterCamera.ScreenToWorldPoint(Input.mousePosition);
            if (Vector3.Distance(tempFingerWaterPos, fingerWaterPositions[fingerWaterPositions.Count - 1]) > distance)
            {
                UpdateLine2d(tempFingerWaterPos);
            }

            int layerMask = 1 << 9;
            layerMask = ~layerMask;


            if (Physics.Raycast(m_Camera.ScreenToWorldPoint(Input.mousePosition), m_Camera.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.name == "Plane")
                {
                    Vector3 tempFingerPos = hit.point;
                    float _distance = Vector3.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]);
                    if (_distance > distance && _distance < 2 * distance)
                    {
                        UpdateLine(tempFingerPos);
                    }  
                }
            }
        }  
        if(Input.GetMouseButtonUp(0))
        {
            PathScript.Instance.CompleteLine();
            IsComplete = true;
        }    
    }

    private void CreateLine()
    {
        currentLine = Instantiate(linePrefabs, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();

        fingerPositions = new List<Vector3>();

        if (Physics.Raycast(m_Camera.ScreenToWorldPoint(Input.mousePosition), m_Camera.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if(hit.collider.name == "Plane")
            {
                Vector3 fingerPos = hit.point;
                fingerPositions.Add(fingerPos);
                fingerPositions.Add(fingerPos);
                lineRenderer.SetPosition(0, fingerPositions[0]);
                lineRenderer.SetPosition(1, fingerPositions[1]);

                position.y = hight + 0.06f;
            }
        }
    }

    private void CreatedLine2d()
    {
        currentWaterLine = Instantiate(linePrefabs, Vector2.zero, Quaternion.identity);
        lineWaterRender = currentWaterLine.GetComponent<LineRenderer>();

        fingerWaterPositions = new List<Vector2>();

        Vector2 startPos = m_WaterCamera.ScreenToWorldPoint(Input.mousePosition);
        fingerWaterPositions.Add(startPos);
        fingerWaterPositions.Add(startPos);
        lineWaterRender.SetPosition(0, fingerWaterPositions[0]);
        lineWaterRender.SetPosition(1, fingerWaterPositions[1]);
    }

    private void UpdateLine(Vector3 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        PathScript.Instance.DrawLine(lineRenderer, position, hight, distance);
    }

    private void UpdateLine2d(Vector2 newWaterFingerPos)
    {
        fingerWaterPositions.Add(newWaterFingerPos);
        lineWaterRender.positionCount += 1;
        lineWaterRender.SetPosition(lineWaterRender.positionCount - 1, newWaterFingerPos);
    }

    ////public float ScaleX = 1.0f;
    ////public float ScaleY = 1.0f;
    ////public float ScaleZ = 1.0f;
    ////public bool RecalculateNormals = false;
    ////private Vector3[] _baseVertices;
    //Mesh mesh = null;
    //private bool IsInit = false;

    //public int[] triangles;
    //public Vector3[] vertices;

    //private int[] trianglesModels;
    //private Vector3[] verticesModels;

    
    //public List<int> cacheTriangles = new List<int>();
    //public List<Vector3> cacheVertices = new List<Vector3>();

    //private void CompleteLine()
    //{
    //    if(!IsInit)
    //    {
    //        IsInit = true;
    //        mesh = new Mesh();
    //        mesh.name = "GeneralMesh";
    //        GameObject MeshObject = new GameObject();
    //        MeshObject.name = "Mesh";
    //        var mr = MeshObject.AddComponent<MeshRenderer>();
    //        var MeshFilter = MeshObject.AddComponent<MeshFilter>();
    //        MeshFilter.mesh = mesh;
    //    }    

    //    lineRenderer.BakeMesh(mesh, Camera.main, false);

    //    triangles = mesh.triangles;
    //    vertices = mesh.vertices;

    //    //int size = 4;
    //    //for(int i = 0; i < (int)(vertices.Length / size); i++)
    //    //{
    //    //    cacheVertices.Add(vertices[i * size]);
    //    //    cacheVertices.Add(vertices[i * size + 1]);
    //    //    cacheVertices.Add(vertices[i * size + 2]);
    //    //    cacheVertices.Add(vertices[i * size + 3]);

    //    //    cacheVertices.Add(GeneralBottomPostion(vertices[i * size]));
    //    //    cacheVertices.Add(GeneralBottomPostion(vertices[i * size + 1]));
    //    //    cacheVertices.Add(GeneralBottomPostion(vertices[i * size + 2]));
    //    //    cacheVertices.Add(GeneralBottomPostion(vertices[i * size + 3]));
    //    //}

    //    //var mesh = MeshObject.GetComponent<MeshFilter>().mesh;
    //    //if (_baseVertices == null)
    //    //    _baseVertices = mesh.vertices;
    //    //var vertices = new Vector3[_baseVertices.Length];
    //    //for (var i = 0; i < vertices.Length; i++)
    //    //{
    //    //    var vertex = _baseVertices[i];
    //    //    vertex.x = vertex.x * ScaleX;
    //    //    vertex.y = vertex.y * ScaleY;
    //    //    vertex.z = vertex.z * ScaleZ;
    //    //    vertices[i] = vertex;
    //    //}
    //    //mesh.vertices = vertices;
    //    //if (RecalculateNormals)
    //    //    mesh.RecalculateNormals();
    //    //mesh.RecalculateBounds();
    //}

    //private Vector3 GeneralBottomPostion(Vector3 vector)
    //{
    //    var rp = vector;
    //    rp.y += 2;
    //    return rp;
    //}
}
