using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public float ScreenX = 0;
    public float ScreenY = 0;
    private void Awake()
    {
        ScreenSetting();
    }
    private void Update()
    {
        ScreenSetting();
    }
    public void ScreenSetting()
    {
        if (Screen.width == ScreenX && Screen.height == ScreenY) return;

        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;

        float ScaleHeight = (float)Screen.width / Screen.height / (ScreenX / ScreenY);
        float ScaleWidth = 1f / ScaleHeight;

        if(ScaleHeight < 1f)
        {
            rect.width = 1f;
            rect.height = ScaleHeight;
            rect.x = 0f;
            rect.y = (1f - ScaleHeight) / 2f;
        }
        else
        {
            rect.width = ScaleWidth;
            rect.height = 1f;
            rect.x = (1f - ScaleWidth) / 2f;
            rect.y = 0f;
        }
        camera.rect = rect;
    }
    private void OnPreCull() // 래터박스 검은색으로 바꿔줌
    {
        GL.Clear(true, true, Color.black);
    }
}
