using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraResolution : MonoBehaviour
{
    [SerializeField]
    bool m_clearScreen;
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)2 / 3);
        float scaleWidth = 1f / scaleHeight;
        if(scaleHeight < 1f)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }
        cam.rect = rect;
    }
    void OnPreCull()
    {
        if(m_clearScreen)
        {
            GL.Clear(true, true, Color.black);
        }
        
    }
}
