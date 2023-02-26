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
    public abstract int Cost { get; }
    public Turret Turret;
}

[Serializable]
public class OffensiveTurretData : TurretData
{
    public override TurretType TurretType => TurretType.Offensive;
    
    public MOffensiveTurret MOffensiveTurret;
    public OffensiveTurretType OffensiveTurretType => MOffensiveTurret.OffensiveTurretType;
    public override int Cost => MOffensiveTurret.Cost;

}

[Serializable]
public class SupportTurretData : TurretData
{
    public override TurretType TurretType => TurretType.Support;
    
    public MSupportTurret MSupportTurret;
    public SupportTurretType SupportTurretType => MSupportTurret.SupportTurretType;
    public override int Cost => MSupportTurret.Cost;
}