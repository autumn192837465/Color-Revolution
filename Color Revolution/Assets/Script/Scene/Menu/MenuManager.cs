using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private MenuDeckUI MenuDeckUI;
    [SerializeField] private CardUpgradeUI CardUpgradeUI;
    protected override void Awake()
    {
        base.Awake();
        if (isDuplicate) return;        
    }
    
    void Start()
    {
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
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        };
    }
    #endregion

    #region RemoveUIEvent
    #endregion
}