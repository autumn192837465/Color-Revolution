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
    [SerializeField] private GameObject cardSwapRoot;
    [SerializeField] private Button cardSwapRootCancelButton;
    [SerializeField] private CardUI swapCardUI;
    
    private CardDeckThumbnail selectingCardDeckThumbnail;
    private UCard[] PlayerCardDeck => PlayerDataManager.Instance.PlayerData.CardDeck;
    private List<UCard> PlayerpossessingCards => PlayerDataManager.Instance.PlayerData.UCardDataList;
    private bool IsInDeck(UCard uCard) => PlayerCardDeck.ToList().Contains(uCard);
    
    private void Awake()
    {
        turretTabButton.onClick.AddListener(OpenTurretPage);
        cardTabButton.onClick.AddListener(OpenCardPage);
        cardSwapRootCancelButton.onClick.AddListener(CloseCardSwapRoot);
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
        if (isOpeningSwapRoot)
        {
            SwapCard(cardDeckThumbnail);
            return;
        }
        
        
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

    private bool isOpeningSwapRoot;
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
        
        OpenCardSwapRoot(cardDeckThumbnail);
    }

    private void OpenCardSwapRoot(CardDeckThumbnail cardDeckThumbnail)
    {
        selectingCardDeckThumbnail = cardDeckThumbnail;
        cardSwapRoot.SetActive(true);
        swapCardUI.InitializeUI(selectingCardDeckThumbnail.UCard.MCard);
        isOpeningSwapRoot = true;
    }

    private void CloseCardSwapRoot()
    {
        DeselectingCard();
        cardSwapRoot.SetActive(false);
        isOpeningSwapRoot = false;
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
        for (int i = 0; i < PlayerCardDeck.Length; i++) 
        {
            if (PlayerCardDeck[i] == cardDeckThumbnail.UCard)
            {
                PlayerCardDeck[i] = null;
            }
        }
        
    }

    private void SwapCard(CardDeckThumbnail cardDeckThumbnail)
    {
        for (int i = 0; i < cardSlots.Length; i++) 
        {
            if (PlayerCardDeck[i] == cardDeckThumbnail.UCard)
            {
                PlayerCardDeck[i] = selectingCardDeckThumbnail.UCard;
                
                cardDeckThumbnail.transform.SetParent(cardScrollerContent);
                cardDeckThumbnail.SetCardStatus(CardDeckThumbnail.CardStatus.CardPool);
                selectingCardDeckThumbnail.SetCardStatus(CardDeckThumbnail.CardStatus.InDeck);
                cardSlots[i].PlaceCard(selectingCardDeckThumbnail);
                CloseCardSwapRoot();
                return;
            }
        }
        Debug.LogError("Not find slot");
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