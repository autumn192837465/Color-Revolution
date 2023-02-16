using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR;
using Kinopi.Constants;
using Kinopi.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUpgradeUI : AnimatorBase
{
    public enum ButtonType
    {
        Close,
        Upgrade,
    }

    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }

    [SerializeField] private List<ButtonInfo> buttonList;
    [SerializeField] private CardUI cardUI;

    [SerializeField] private TextMeshProUGUI upgradeCostText;
    public UCard UCard { get; private set; }
    
 
    public Action<ButtonType> OnClickButton;    
    

    protected override void Awake()
    {
        base.Awake();
        foreach(ButtonInfo buttonInfo in buttonList)
        {
            buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
        }
    }
    
    void Start()
    {
        
    }

    public void InitializeUI(UCard uCard)
    {
        UCard = uCard;
        cardUI.InitializeUI(uCard);

        if (uCard.Level == Constants.MaxCardLevel)
        {
            
        }
        else
        {
            int cost = DataManager.Instance.GetCardLevelData(uCard.Level).UpgradeCost;


            upgradeCostText.text = cost.ToString();
            upgradeCostText.color = PlayerDataManager.Instance.GetUPoint(PointType.RainbowCandy).Count >= cost
                ? Constants.EnableColor
                : Constants.DisableColor;
        }

    }
    
}