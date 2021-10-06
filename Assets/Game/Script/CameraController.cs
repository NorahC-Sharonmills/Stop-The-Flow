using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    private Vector3 CameraRotationView = new Vector3(-35f, 0f, 0f);
    private Vector3 CameraRotationDraw = new Vector3(0f, 0f, 0f);

    private bool IsView = false;
    private bool IsDraw = false;

    protected override void Awake()
    {
        base.Awake();
        transform.eulerAngles = CameraRotationView;
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
    float lerpDuration = 0.7f;

    void Update()
    {
        if(IsDraw)
        {
            if (timeElapsed < lerpDuration)
            {
                transform.eulerAngles = Vector3.Lerp(CameraRotationView, CameraRotationDraw, timeElapsed / lerpDuration);
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
