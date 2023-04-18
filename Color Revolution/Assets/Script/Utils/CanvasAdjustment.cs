using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAdjustment : MonoBehaviour
{
    public int preferredWidth = 1920;
    public int preferredHeight = 1080;    
    

    private void Awake()
    {
        float screenRatio = (float)Screen.height / Screen.width;
        CanvasScaler scaler = GetComponent<CanvasScaler>();

        float ratio = (float)preferredHeight / preferredWidth;
        scaler.matchWidthOrHeight = (screenRatio > ratio) ? 0 : 1;
    }
}

