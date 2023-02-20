using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using CR;
using Kinopi.Enums;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private MenuDeckUI MenuDeckUI;
    [SerializeField] private CardUpgradeUI CardUpgradeUI;
    [SerializeField] private MenuHeaderUI MenuHeaderUI;
    protected override void Awake()
    {
        base.Awake();
        if (isDuplicate) return;        
    }
    
    void Start()
    {
        MenuHeaderUI.InitializePlayerData();
        AddMenuDeckUIEvent();
        AddCardUpgradeUIEvent();
    }
    
    void Update()
    {
        
    }
    #region AddUIEvent

    private void AddMenuDeckUIEvent()
    {
        MenuDeckUI.OnClickCardUpgrade = (uCard) =>
        {
            CardUpgradeUI.Open();
            CardUpgradeUI.InitializeUI(uCard);
        };
    }

    private void AddCardUpgradeUIEvent()
    {
        CardUpgradeUI.OnClickButton = (type) =>
        {
            switch (type)
            {
                case CardUpgradeUI.ButtonType.Close:
                    CardUpgradeUI.Close();
                    break;
                case CardUpgradeUI.ButtonType.Upgrade:
                    //CardUpgradeUI.UCard;
                    // Todo : get m card cost
                    PlayerDataManager.Instance.SubUPoint(new PointTuple(PointType.RainbowCandy, 10));
                    MenuHeaderUI.InitializePlayerData();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        };
    }
    #endregion

    #region RemoveUIEvent
    #endregion
}