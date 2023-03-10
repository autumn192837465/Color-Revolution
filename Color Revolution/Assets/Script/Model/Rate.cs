using System;
using Kinopi.Utils;
using UnityEngine;

[Serializable]
public class Rate
{
    public Rate(int rate)
    {
        value = rate;
    }
    
    public int Value
    {
        get => value;
        set => this.value = Mathf.Clamp(value, 0, 100);
    }

    [SerializeField] [Range(0, 100)] private int value;

    public bool HitProbability()
    {
        return Utils.HitProbability(value);
    }

    public float ToPercentage()
    {
        return value / 100.0f;
    }

    public override string ToString()
    {
        return $"{value}%";
    }
}