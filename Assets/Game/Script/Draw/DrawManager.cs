using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoSingleton<DrawManager>
{
    public GameObject linePrefabs;

    private GameObject currentLine = null;
    private LineRenderer lineRenderer = null;
    private List<Vector3> fingerPositions;

    private GameObject currentWaterLine;
    private LineRenderer lineWaterRender;
    private List<Vector2> fingerWaterPositions;
    private EdgeCollider2D edgeWaterCollider;

    [SerializeField] private Camera m_Camera;
    [SerializeField] private Camera m_WaterCamera;

    public float distance = 0.1f;
    public float hight = 0.25f;
    private float m_TimerToVictory = 0;
    public float m_TimeToVictory = 5f;

    private Vector3 position = Vector3.zero;

    private bool IsComplete = false;
    private bool IsLine = false;
    [SerializeField, Range(0f, 1f)] public float radius = 0.2f;

    private void Start()
    {
        IsComplete = false;
    }

    private void CheckingVictory()
    {
        if (StaticVariable.GameState != GameState.PLAY)
            return;

        if (!IsComplete)
            return;

        if (Game.LevelManager.Instance.IsLose)
            return;

        m_TimerToVictory += Time.deltaTime;
        if (m_TimerToVictory > m_TimeToVictory)
        {
            Game.LevelManager.Instance.OnVictory();
        }
    }

    private void Update()
    {
        CheckingVictory();

        if (StaticVariable.GameState != GameState.DRAW)
            return;

        if (IsComplete)
            return;

        if (Game.Shop.Instance.IsShop)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }

        if(Input.GetMouseButton(0))
        {

            if (!IsLine)
            {
                CreateLine();
            }
            else
            {
                int layerMask = 1 << 9;
                layerMask = ~layerMask;

                if (Physics.SphereCast(m_Camera.ScreenToWorldPoint(Input.mousePosition), radius, m_Camera.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.name == "Plane")
                    {
                        Vector3 tempFingerPos = hit.point;
                        if (fingerPositions == null)
                            CreateLine();

                        if (fingerPositions.Count < 0)
                            CreateLine();
                        float _distance = Vector3.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]);
                        if (_distance > distance && _distance < 4f * distance)
                        {
                            UpdateLine(tempFingerPos);
                        }
                    }
                }
            }    
        }

        if (!IsLine)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            if (fingerPositions == null)
                return;

            if (fingerPositions.Count < 3)
                return;

            switch (Game.LevelManager.Instance.m_AttackType)
            {
                case Enum.AttackType.Enemy:
                    break;
                case Enum.AttackType.Water:
                    Water2D.Water2D_Spawner.instance.Spawn();
                    break;
            }
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
                }, 0.5f);
            });
        }    
    }

    private void CreateLine()
    {
        if (currentLine == null)
            currentLine = Instantiate(linePrefabs, Vector3.zero, Quaternion.identity);
        if (lineRenderer == null)
            lineRenderer = currentLine.GetComponent<LineRenderer>();

        fingerPositions = new List<Vector3>();

        if (Physics.SphereCast(m_Camera.ScreenToWorldPoint(Input.mousePosition), radius, m_Camera.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if(hit.collider.name == "Plane")
            {
                Vector3 fingerPos = hit.point;
                fingerPositions.Add(fingerPos);
                fingerPositions.Add(fingerPos);
                lineRenderer.SetPosition(0, fingerPositions[0]);
                lineRenderer.SetPosition(1, fingerPositions[1]);

                position.y = hight + 0.06f;
                IsLine = true;
            }
        }

        Game.LevelManager.Instance.HideTutorial();
    }

    private void UpdateLine(Vector3 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        PathScript.Instance.DrawLine(lineRenderer, position, hight, distance);
    }
}
