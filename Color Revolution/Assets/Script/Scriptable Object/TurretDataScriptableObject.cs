using System;
using System.Collections.Generic;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using UnityEngine;


[CreateAssetMenu(fileName = "Turret Data", menuName = "ScriptableObjects/Turret/Turret Data", order = 1)]
public class TurretDataScriptableObject : ScriptableObject
{
    public List<OffensiveTurretData> OffensiveTurretDataList;
    public Dictionary<OffensiveTurretType, OffensiveTurretData> OffensiveTurretDataCache { get; private set; }
    
    
    public List<SupportTurretData> SupportTurretDataList;
    public Dictionary<SupportTurretType, SupportTurretData> SupportTurretDataCache { get; private set; }
    
    public void InitializeTurretData()
    {
        OffensiveTurretDataCache = new();
        foreach (var data in OffensiveTurretDataList)
        {
            OffensiveTurretDataCache.Add(data.OffensiveTurretType, data);
        }
        
        SupportTurretDataCache = new();
        foreach (var data in SupportTurretDataList)
        {
            SupportTurretDataCache.Add(data.SupportTurretType, data);
        }
    }
}

[Serializable]
public abstract class TurretData
{
    public abstract TurretType TurretType { get; }
    public Sprite Sprite;
    public int Cost;
    public Turret Turret;
}

[Serializable]
public class OffensiveTurretData : TurretData
{
    public override TurretType TurretType => TurretType.Offensive;
    public OffensiveTurretType OffensiveTurretType;
    public MOffensiveTurret MOffensiveTurret;


}

[Serializable]
public class SupportTurretData : TurretData
{
    public override TurretType TurretType => TurretType.Support;
    public SupportTurretType SupportTurretType;
    public MSupportTurret MSupportTurret;
}