using Kinopi.Enums;
using Kinopi.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using UnityEditor;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicate) return;
        InitializeData();
    }

    private void InitializeData()
    {
        TurretData.InitializeTurretData();
        CardData.InitializeCardData();
    }


    #region Turret
    [SerializeField] private TurretDataScriptableObject TurretData;
    public TurretData GetTurretData(TurretType type)
    {
        return TurretData.turretDataCache.GetValue(type);
    }
    #endregion
    
    
    #region Card
    [SerializeField] private CardDataScriptableObject CardData;
    public CardData GetCardData(CardType type)
    {
        return CardData.cardDataCache.GetValue(type);
    }
    #endregion
   

}