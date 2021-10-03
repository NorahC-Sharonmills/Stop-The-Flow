using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FillScreen : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    private SpriteRenderer GetRender
    {
        get
        {
            return _renderer;
        }
    }
    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
    }

    public void CallBackChangeTheme(Sprite img)
    {
        GetRender.sprite = img;
    }    

    void Update()
    {
        UpdateFillScreen();
    }

    private void UpdateFillScreen()
    {
        var height = _camera.orthographicSize*2;
        var width = height*Screen.width/Screen.height;

        var aspect = (float)Screen.width / Screen.height;
        var imgAspect = GetRender.sprite.bounds.extents.x/ GetRender.sprite.bounds.extents.y;

        if (aspect >= imgAspect)
        {
            transform.localScale = Vector3.one*width/(2* GetRender.sprite.bounds.extents.x);
        }
        else
        {
            transform.localScale = Vector3.one * height / (2 * GetRender.sprite.bounds.extents.y);
        }
    }
}
