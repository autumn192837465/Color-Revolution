using System;
using Kinopi.Utils;
using UnityEngine;

//per ten thousand
[Serializable]
public class Amplifier
{
    public Amplifier(int amplifier)
    {
        value = amplifier;
    }
    
    public int Value
    {
        get => value;
        set => this.value = value;
    }

    private int value;

    public float GetAmplifiedValue(int oldValue)
    {
        return oldValue * value / 100.0f;
    }
    
    public float GetAmplifiedValue(float oldValue)
    {
        return oldValue * value / 100.0f;
    }

    public override string ToString()
    {
        return $"{value}%";
    }
}