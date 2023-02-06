using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using TMPro;

// Todo : Inherit?
public class CardDeckThumbnail : MonoBehaviour
{
    public enum CardStatus
    {
        InDeck,
        CardPool,
    }
    
    [SerializeField] private Button cardButton;
    [SerializeField] private CardUI cardUI;
    [SerializeField] private GameObject buttonRoot;
    [SerializeField] private Button useRemoveButton;
    [SerializeField] private TextMeshProUGUI useRemoveText;
    public UCard UCard { get; private set; }
    
    
    public Action OnSelectCard;
    public Action OnClickUseRemoveButton;
    private void Awake()
    {
        cardButton.onClick.AddListener(() => OnSelectCard?.Invoke());
        useRemoveButton.onClick.AddListener(() => OnClickUseRemoveButton?.Invoke());
    }

    public void InitializeUI(UCard uCard)
    {
        UCard = uCard;
        cardUI.InitializeUI(uCard.MCard);
    }

    public void SetCardStatus(CardStatus status)
    {
        switch (status)
        {
            case CardStatus.InDeck:
                useRemoveText.text = "Remove";
                break;
            case CardStatus.CardPool:
                useRemoveText.text = "Use";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
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