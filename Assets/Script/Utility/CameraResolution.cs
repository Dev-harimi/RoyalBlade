using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    void Awake()
    {
        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;

        //16:9로 하고싶으면  ((float)9 / 16) 이 부분만 수정
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }

        cam.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}
