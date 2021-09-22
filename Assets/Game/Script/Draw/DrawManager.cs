using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject linePrefabs;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    //public EdgeCollider2D edgeCollider;
    public List<Vector3> fingerPositions;

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
            if(Vector3.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
            {
                UpdateLine(tempFingerPos);
            }
        }
    }

    private void CreateLine()
    {
        currentLine = Instantiate(linePrefabs, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        //edgeCollider = currentLine.GetComponent<EdgeCollider2D>();

        fingerPositions.Clear();
        var startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startPos.y = 3;
        fingerPositions.Add(startPos);
        fingerPositions.Add(startPos);
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
        //edgeCollider.points = fingerPositions.ToArray();
    }

    private void UpdateLine(Vector3 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        //edgeCollider.points = fingerPositions.ToArray();
    }
}
