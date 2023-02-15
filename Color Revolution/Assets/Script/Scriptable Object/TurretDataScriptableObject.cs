using System;
using System.Collections.Generic;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using UnityEngine;


[CreateAssetMenu(fileName = "Turret Data", menuName = "ScriptableObjects/Turret/Turret Data", order = 1)]
public class TurretDataScriptableObject : ScriptableObject
{
    public List<TurretData> TurretDataList;
    public Dictionary<TurretType, TurretData> TurretDataCache { get; private set; }

    public void InitializeTurretData()
    {
        TurretDataCache = new();
        foreach (var data in TurretDataList)
        {
            TurretDataCache.Add(data.TurretType, data);
        }
    }
}

[Serializable]
public class TurretData
{
    public TurretType TurretType;
    public Sprite Sprite;
    public Turret Turret;
    public TurretBasicDataScriptableObject BasicDataScriptableObject;
    public int Cost;
}