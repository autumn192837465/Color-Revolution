using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR;
using Kinopi.Constants;
using Kinopi.Enums;
using Kinopi.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUpgradeUI : AnimatorBase
{
    [SerializeField] private CardUI cardUI;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private FeedbackButton upgradeButton;
    [SerializeField] private Button closeButton;
    
    public UCard SelectingUCard { get; private set; }


    public Action OnClickUpgrade;    
    

    protected override void Awake()
    {
        base.Awake();
        upgradeButton.OnClick = () => OnClickUpgrade?.Invoke();
        closeButton.onClick.AddListener(Close);
    }
    
    void Start()
    {
        
    }

    public void InitializeUI(UCard uCard)
    {
        SelectingUCard = uCard;
        RefreshUI();
        
    }

    public void RefreshUI()
    {
        var uCard = SelectingUCard;
        cardUI.InitializeUI(uCard);

        if (uCard.Level == Constants.MaxCardLevel)
        {
            descriptionText.text = uCard.GetDescription();
            upgradeButton.SetActive(false);
        }
        else
        {
            int cost = DataManager.Instance.GetCardLevelData(uCard.Level).UpgradeCost;
            descriptionText.text = uCard.GetUpgradeDescription();
            upgradeCostText.text = cost.ToString();
            bool canUpgrade = PlayerDataManager.Instance.GetUPoint(PointType.RainbowCandy).Count >= cost;
            upgradeCostText.color = canUpgrade
                ? Constants.EnableColor
                : Constants.DisableColor;
            
            upgradeButton.Interactable = canUpgrade;
            upgradeButton.SetActive(true);
        }
    }
    
}