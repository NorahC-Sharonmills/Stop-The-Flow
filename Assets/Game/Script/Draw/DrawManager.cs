using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoSingleton<DrawManager>
{
    public GameObject linePrefabs;

    private GameObject currentLine;
    private LineRenderer lineRenderer;
    private List<Vector3> fingerPositions;

    private GameObject currentWaterLine;
    private LineRenderer lineWaterRender;
    private List<Vector2> fingerWaterPositions;
    private EdgeCollider2D edgeWaterCollider;

    [SerializeField] private Camera m_Camera;
    [SerializeField] private Camera m_WaterCamera;

    public float distance = 0.1f;
    public float hight = 0.25f;

    private Vector3 position = Vector3.zero;

    private bool IsComplete = false;

    private void Start()
    {
        IsComplete = false;
    }

    private void Update()
    {
        if (StaticVariable.GameState != GameState.DRAW)
            return;

        if (IsComplete)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        if(Input.GetMouseButton(0))
        {
            int layerMask = 1 << 9;
            layerMask = ~layerMask;


            if (Physics.Raycast(m_Camera.ScreenToWorldPoint(Input.mousePosition), m_Camera.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.name == "Plane")
                {
                    Vector3 tempFingerPos = hit.point;
                    if (fingerPositions == null)
                        CreateLine();

                    if (fingerPositions.Count < 0)
                        CreateLine();
                    float _distance = Vector3.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]);
                    if (_distance > distance && _distance < 4 * distance)
                    {
                        UpdateLine(tempFingerPos);
                    }  
                }
            }
        }  
        if(Input.GetMouseButtonUp(0))
        {
            if (fingerPositions == null)
                return;

            if (fingerPositions.Count < 3)
                return;
            PathScript.Instance.CompleteLine();
            IsComplete = true;
            CameraController.Instance.MoveToView(() =>
            {
                StaticVariable.GameState = GameState.PLAY;
                CoroutineUtils.PlayCoroutine(() =>
                {
                    switch(Game.LevelManager.Instance.m_AttackType)
                    {
                        case Enum.AttackType.Enemy:
                            break;
                        case Enum.AttackType.Water:
                            Game.LevelManager.Instance.Tank.waterFall.SetActive(false);
                            break;
                    }
                }, 0.1f);

                CoroutineUtils.PlayCoroutine(() =>
                {
                    Game.LevelManager.Instance.OnVictory();
                }, 5f);
            });
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

        edgeWaterCollider = currentWaterLine.AddComponent<EdgeCollider2D>();
        edgeWaterCollider.edgeRadius = lineWaterRender.startWidth * 2;
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

        edgeWaterCollider.SetPoints(fingerWaterPositions);
    }
}
