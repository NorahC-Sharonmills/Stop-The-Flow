using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoSize : MonoBehaviour
{
    Vector2 m_DefaultSize = new Vector2(1920, 1080);

    private Camera m_Camera;
    private float m_ValueScale = 1;
    public float m_RatioAspect = 1;
    public float m_RatioAspectDefatlt = 1;
    public float m_ScaleRatio = 1;
    private void Awake()
    {
        m_Camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        m_RatioAspect = m_Camera.aspect;
        m_RatioAspectDefatlt = m_DefaultSize.y / m_DefaultSize.x;
        m_ScaleRatio = m_RatioAspectDefatlt / m_RatioAspect;
        if (m_Camera.orthographicSize != Game.LevelManager.Instance.m_SizeCamera * m_ScaleRatio)
        {
            m_Camera.orthographicSize = Game.LevelManager.Instance.m_SizeCamera * m_ScaleRatio;
        }
    }
}
