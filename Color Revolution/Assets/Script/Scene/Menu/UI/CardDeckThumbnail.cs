using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using TMPro;

// Todo : Inherit?
public class CardDeckThumbnail : MonoBehaviour
{
    public enum Status
    {
        InDeck,
        CardPool,
    }
    
    [SerializeField] private Button cardButton;
    [SerializeField] private CardUI cardUI;
    [SerializeField] private GameObject buttonRoot;
    [SerializeField] private Button useRemoveButton;
    [SerializeField] private TextMeshProUGUI useRemoveText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button upgradeButton;
    
    
    public Status CardStatus
    {
        get
        {
            return cardStatus;
        }
        set
        {
            cardStatus = value;
            useRemoveText.text = cardStatus switch
            {
                Status.InDeck => "Remove",
                Status.CardPool => "Use",
                _ => throw new NotImplementedException(),
            };
        }
    }

    private Status cardStatus;
    
    public UCard UCard { get; private set; }
    
    
    public Action OnSelectCard;
    public Action OnClickUseRemoveButton;
    public Action OnClickUpgradeButton;
    private void Awake()
    {
        cardButton.onClick.AddListener(() => OnSelectCard?.Invoke());
        useRemoveButton.onClick.AddListener(() => OnClickUseRemoveButton?.Invoke());
        upgradeButton.onClick.AddListener(() => OnClickUpgradeButton?.Invoke());
        HideButtonRoot();
    }

    public void InitializeUI(UCard uCard)
    {
        UCard = uCard;
        cardUI.InitializeUI(uCard);
        levelText.text = $"Lv.{uCard.Level}";
    }



    public void ShowButtonRoot()
    {
        buttonRoot.SetActive(true);
    }
    
    public void HideButtonRoot()
    {
        buttonRoot.SetActive(false);
    }
}