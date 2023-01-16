
using System;
using UnityEngine;
using Kinopi.Utils;
[Serializable]
public class RGB
{
    public RGB(int r, int g, int b)
    {
        RedValue = r;
        GreenValue = g;
        BlueValue = b;
    }

    public int Sum => RedValue + GreenValue + BlueValue;
    public Color Color => CRUtils.ToColor(this);
    
    public int RedValue;
    public int GreenValue;
    public int BlueValue;
    
}