using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR;
using Kinopi.Enums;
using Kinopi.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuDeckUI : MonoBehaviour
{
    [SerializeField] private CardSlot[] cardSlots;
    
    
    [Header("Turret")]
    [SerializeField] private Button turretTabButton;
    [SerializeField] private Transform turretPage;
    [SerializeField] private Transform turretScrollerContent;
    
    
    
    [Header("Card")]
    [SerializeField] private Button cardTabButton;
    [SerializeField] private Transform cardContent;
    [SerializeField] private Transform cardScrollerContent;
    [SerializeField] private CardDeckThumbnail cardDeckThumbnailPrefab;
    private CardDeckThumbnail selectingCardDeckThumbnail;
    private UCardData[] PlayerCardDeck => PlayerDataManager.Instance.PlayerData.CardDeck;
    private List<UCardData> PlayerpossessingCards => PlayerDataManager.Instance.PlayerData.UCardDataList;
    private bool IsInDeck(UCardData uCard) => PlayerCardDeck.ToList().Contains(uCard);
    
    private void Awake()
    {
        turretTabButton.onClick.AddListener(OpenTurretPage);
        cardTabButton.onClick.AddListener(OpenCardPage);
        OpenCardPage();
    }
    
    void Start()
    {
        InitializeUI();
    }

    public void InitializeUI()
    {
        InitializeCardDeck();
    }

    #region Card
    private void InitializeCardDeck()
    {
        foreach (var uCard in PlayerpossessingCards)
        {
            int slotIndex = Array.FindIndex(PlayerCardDeck, x => x.CardType == uCard.CardType);
            
            
            var cardDeckThumbnail = Instantiate(cardDeckThumbnailPrefab);
            if (slotIndex >= 0)
            {
                cardSlots[slotIndex].PlaceCard(cardDeckThumbnail);
                cardDeckThumbnail.SetCardStatus(CardDeckThumbnail.CardStatus.InDeck);
            }
            else
            {
                cardDeckThumbnail.transform.SetParent(cardScrollerContent);
                cardDeckThumbnail.SetCardStatus(CardDeckThumbnail.CardStatus.CardPool);
            }
            
            cardDeckThumbnail.InitializeUI(uCard);
            cardDeckThumbnail.OnSelectCard = () => OnSelectCard(cardDeckThumbnail);

            cardDeckThumbnail.OnClickUseRemoveButton = () =>
            {
                if(IsInDeck(uCard))
                {
                    RemoveCard(cardDeckThumbnail);
                }
                else
                {
                    UseCard(cardDeckThumbnail);
                }
                
            };

        }
    }

    private void OnSelectCard(CardDeckThumbnail cardDeckThumbnail)
    {
        DeselectingCard();
        if (selectingCardDeckThumbnail == cardDeckThumbnail) return;
        
        selectingCardDeckThumbnail = cardDeckThumbnail;
        selectingCardDeckThumbnail.ShowButtonRoot();
    }

    private void DeselectingCard()
    {
        if (selectingCardDeckThumbnail != null) selectingCardDeckThumbnail.HideButtonRoot();
        selectingCardDeckThumbnail = null;
    }
    
    

   

    public void UseCard(CardDeckThumbnail cardDeckThumbnail)
    {
        // Check has empty slot
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].PlacingCardDeckThumbnail == null)
            {
                PlaceCardToSlot(cardSlots[i], i, cardDeckThumbnail);
                return;
            }
        }
    }

    private void PlaceCardToSlot(CardSlot slot, int slotIndex, CardDeckThumbnail cardDeckThumbnail)
    {
        slot.PlaceCard(cardDeckThumbnail);
        selectingCardDeckThumbnail.SetCardStatus(CardDeckThumbnail.CardStatus.InDeck);
        DeselectingCard();

        PlayerCardDeck[slotIndex] = cardDeckThumbnail.UCard;
    }

    private void RemoveCard(CardDeckThumbnail cardDeckThumbnail)
    {
        // Remove
        cardDeckThumbnail.transform.SetParent(cardScrollerContent);
        selectingCardDeckThumbnail.SetCardStatus(CardDeckThumbnail.CardStatus.CardPool);
        DeselectingCard();
        for (int i = 0; i < PlayerCardDeck.Length; i++) 
        {
            if (PlayerCardDeck[i] == cardDeckThumbnail.UCard)
            {
                PlayerCardDeck[i] = null;
            }
        }
        
    }
    #endregion
    

    
    
    
    private void OnValidate()
    {
        if (cardSlots == null || cardSlots.Length == 0)
        {
            cardSlots = GetComponentsInChildren<CardSlot>();
        }
        
        
    }

    private void OpenTurretPage()
    {
        turretPage.SetAsLastSibling();
    }
    
    private void OpenCardPage()
    {
        cardContent.SetAsLastSibling();
    }
    
}