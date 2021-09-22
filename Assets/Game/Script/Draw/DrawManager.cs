using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject drawPrefabs;

    private GameObject theTrail;
    private Plane planeObject;

    private Vector3 startPos;

    private void Start()
    {
        planeObject = new Plane(Camera.main.transform.forward * -1, this.transform.position);
    }

    private void Update()
    {
#if UNITY_EDITOR
        //if(Input.touchCount > 0)
        //{
        //    switch(Input.GetTouch(0).phase)
        //    {
        //        case TouchPhase.Moved:
        //            break;
        //    }
        //}

        if(Input.GetMouseButtonDown(0))
        {
            theTrail = Instantiate(drawPrefabs, this.transform.position, Quaternion.identity) as GameObject;

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float _dis;
            if(planeObject.Raycast(mouseRay, out _dis))
            {
                startPos = mouseRay.GetPoint(_dis);
            }
        }
        else if(Input.GetMouseButton(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float _dis;
            if (planeObject.Raycast(mouseRay, out _dis))
            {
                theTrail.transform.position = mouseRay.GetPoint(_dis);
            }
        }
#endif
    }
}
