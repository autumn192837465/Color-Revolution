using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR;
using CR.Game;
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
    [SerializeField] private GameObject turretDeckContent;
    
    
    [Header("Card")]
    [SerializeField] private Button cardTabButton;
    [SerializeField] private Transform cardContent;
    [SerializeField] private Transform cardScrollerContent;
    [SerializeField] private GameObject cardDeckContent;
    [SerializeField] private CardDeckThumbnail cardDeckThumbnailPrefab;
    [SerializeField] private GameObject cardSwapRoot;
    [SerializeField] private Button cardSwapRootCancelButton;
    [SerializeField] private CardUI swapCardUI;
    [SerializeField] private GridLayoutGroup cardScrollerLayoutGroup;
    
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
                cardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.InDeck;
            }
            else
            {
                cardDeckThumbnail.transform.SetParent(cardScrollerContent);
                cardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.CardPool;
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

        RefreshCardScroller();
    }

    private void OnSelectCard(CardDeckThumbnail cardDeckThumbnail)
    {
        if (isOpeningSwapRoot)
        {
            SwapCard(cardDeckThumbnail);
            return;
        }

        if (selectingCardDeckThumbnail == cardDeckThumbnail)
        {
            DeselectingCard();    
            return;
        }
        
        
        DeselectingCard();

        selectingCardDeckThumbnail = cardDeckThumbnail;
        selectingCardDeckThumbnail.ShowButtonRoot();


        
        switch (selectingCardDeckThumbnail.CardStatus)
        {
            case CardDeckThumbnail.Status.InDeck:
                foreach (var slot in cardSlots)
                {
                    if (slot.PlacingCardDeckThumbnail != selectingCardDeckThumbnail) continue;
                    slot.transform.SetAsLastSibling();
                    break;
                }
                break;
            case CardDeckThumbnail.Status.CardPool:
                selectingCardDeckThumbnail.transform.SetAsLastSibling();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
       
        
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
        selectingCardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.InDeck;
        DeselectingCard();
        PlayerCardDeck[slotIndex] = cardDeckThumbnail.UCard;
        RefreshCardScroller();
    }

    
    
    private void RemoveCard(CardDeckThumbnail cardDeckThumbnail)
    {
        // Remove
        cardDeckThumbnail.transform.SetParent(cardScrollerContent);
        selectingCardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.CardPool;
        for (int i = 0; i < PlayerCardDeck.Length; i++) 
        {
            if (PlayerCardDeck[i] == cardDeckThumbnail.UCard)
            {
                PlayerCardDeck[i] = null;
                break;
            }
        }
        RefreshCardScroller();
    }

    private void SwapCard(CardDeckThumbnail cardDeckThumbnail)
    {
        for (int i = 0; i < cardSlots.Length; i++) 
        {
            if (PlayerCardDeck[i] == cardDeckThumbnail.UCard)
            {
                PlayerCardDeck[i] = selectingCardDeckThumbnail.UCard;
                
                cardDeckThumbnail.transform.SetParent(cardScrollerContent);
                cardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.CardPool;
                selectingCardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.InDeck;
                cardSlots[i].PlaceCard(selectingCardDeckThumbnail);
                CloseCardSwapRoot();
                RefreshCardScroller();
                return;
            }
        }
        Debug.LogError("Not find slot");
    }

    private void RefreshCardScroller()
    {
        cardScrollerLayoutGroup.enabled = true;
        Canvas.ForceUpdateCanvases();
        cardScrollerLayoutGroup.enabled = false;
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
        cardDeckContent.SetActive(false);
        turretDeckContent.SetActive(true);
    }
    
    private void OpenCardPage()
    {
        cardContent.SetAsLastSibling();
        cardDeckContent.SetActive(true);
        turretDeckContent.SetActive(false);
    }
    
}