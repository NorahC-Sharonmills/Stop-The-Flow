using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    private Vector3 CameraPositionView = new Vector3(0f, 0f, -8.5f);
    private Vector3 CameraRotationView = new Vector3(-65f, 0f, 0f);

    private Vector3 CameraPositionDraw = new Vector3(0f, 0f, 0f);
    private Vector3 CameraRotationDraw = new Vector3(0f, 0f, 0f);

    private bool IsView = false;
    private bool IsDraw = false;

    public CameraAutoSize[] CameraOffsets;

    protected override void Awake()
    {
        base.Awake();
        transform.position = CameraPositionView;
        transform.eulerAngles = CameraRotationView;
        OffsetCameraSize = 0.8f;
        IsView = false;
        IsDraw = false;
    }

    private System.Action Complete;

    public void MoveToDraw(System.Action complete = null)
    {
        IsView = false;
        IsDraw = true;
        Complete = complete;
    }

    public void MoveToView(System.Action complete = null)
    {
        IsView = true;
        IsDraw = false;
        Complete = complete;
    }

    float timeElapsed;
    float lerpDuration = 1f;

    public float OffsetCameraSize
    {
        get { return CameraOffsets[0].Offset; }
        set
        {
            for(int i = 0; i < CameraOffsets.Length; i++)
            {
                CameraOffsets[i].Offset = value;
            }    
        }
    }    

    void Update()
    {
        if(IsDraw)
        {
            if (timeElapsed < lerpDuration)
            {
                transform.eulerAngles = Vector3.Lerp(CameraRotationView, CameraRotationDraw, timeElapsed / lerpDuration);
                transform.position = Vector3.Lerp(CameraPositionView, CameraPositionDraw, timeElapsed / lerpDuration);
                OffsetCameraSize = Mathf.Lerp(0.8f, 1f, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                IsDraw = false;
                timeElapsed = 0;
                Complete?.Invoke();
            }
        }

        if(IsView)
        {
            if (timeElapsed < lerpDuration)
            {
                transform.eulerAngles = Vector3.Lerp(CameraRotationDraw, CameraRotationView, timeElapsed / lerpDuration);
                transform.position = Vector3.Lerp(CameraPositionDraw, CameraPositionView, timeElapsed / lerpDuration);
                OffsetCameraSize = Mathf.Lerp(1f, 0.8f, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                IsView = false;
                timeElapsed = 0;
                Complete?.Invoke();
            }
        }    
    }
}
