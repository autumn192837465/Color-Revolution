using Kinopi.Enums;
using Kinopi.Extensions;
using CB.Model;
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
        CardLevelData.InitializeCardLevelData();
        PointData.InitializePointData();
    }


    #region Turret
    [SerializeField] private TurretDataScriptableObject TurretData;
    public TurretData GetTurretData(TurretType type)
    {
        return TurretData.TurretDataCache.GetValue(type);
    }
    #endregion
    
    
    #region Card
    [SerializeField] private CardDataScriptableObject CardData;
    public MCard GetCardData(CardType type)
    {
        return CardData.CardDataCache.GetValue(type);
    }

    [SerializeField] private CardLevelDataScriptableObject CardLevelData;

    public MCardLevel GetCardLevelData(int level)
    {
        return CardLevelData.CardLevelDataCache.GetValue(level);
    }

    #endregion

    #region Point
    [SerializeField] private PointDataScriptableObject PointData;
    public MPoint GetPointData(PointType type)
    {
        return PointData.PointDataCache.GetValue(type);
    }
    

    #endregion

}