
using System;
using UnityEngine;

[Serializable]
public class RGB
{
    public RGB(int r, int g, int b)
    {
        RedValue = r;
        GreenValue = g;
        BlueValue = b;
    }
    public int RedValue;
    public int GreenValue;
    public int BlueValue;
}