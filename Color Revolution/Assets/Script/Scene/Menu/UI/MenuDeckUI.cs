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
    [SerializeField] private Transform cardPage;
    [SerializeField] private Transform cardScrollerContent;
    [SerializeField] private GameObject cardDeckContent;
    [SerializeField] private CardDeckThumbnail cardDeckThumbnailPrefab;
    [SerializeField] private GameObject cardSwapRoot;
    [SerializeField] private Button cardSwapRootCancelButton;
    [SerializeField] private CardUI swapCardUI;
    [SerializeField] private GridLayoutGroup cardScrollerLayoutGroup;
    public Action<UCard> OnClickCardUpgrade;
    
    
    public CardDeckThumbnail SelectingCardDeckThumbnail { get; private set; }
    private UCard[] PlayerCardDeck => PlayerDataManager.Instance.PlayerData.CardDeck;
    private List<UCard> PlayerPossessingCards => PlayerDataManager.Instance.PlayerData.UCardDataList;
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


    #region Turret

    

    #endregion

    #region Card
    private void InitializeCardDeck()
    {
        foreach (var uCard in PlayerPossessingCards)
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

            cardDeckThumbnail.OnClickUpgradeButton = () => OnClickCardUpgrade?.Invoke(uCard);
            //cardDeckThumbnail.OnClickUpgradeButton = 
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

        if (SelectingCardDeckThumbnail == cardDeckThumbnail)
        {
            DeselectingCard();    
            return;
        }
        
        
        DeselectingCard();

        SelectingCardDeckThumbnail = cardDeckThumbnail;
        SelectingCardDeckThumbnail.ShowButtonRoot();


        
        switch (SelectingCardDeckThumbnail.CardStatus)
        {
            case CardDeckThumbnail.Status.InDeck:
                foreach (var slot in cardSlots)
                {
                    if (slot.PlacingCardDeckThumbnail != SelectingCardDeckThumbnail) continue;
                    slot.transform.SetAsLastSibling();
                    break;
                }
                break;
            case CardDeckThumbnail.Status.CardPool:
                SelectingCardDeckThumbnail.transform.SetAsLastSibling();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DeselectingCard()
    {
        if (SelectingCardDeckThumbnail != null) SelectingCardDeckThumbnail.HideButtonRoot();
        SelectingCardDeckThumbnail = null;
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
        SelectingCardDeckThumbnail = cardDeckThumbnail;
        cardSwapRoot.SetActive(true);
        swapCardUI.InitializeUI(SelectingCardDeckThumbnail.UCard);
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
        SelectingCardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.InDeck;
        DeselectingCard();
        PlayerCardDeck[slotIndex] = cardDeckThumbnail.UCard;
        RefreshCardScroller();
    }

    
    
    private void RemoveCard(CardDeckThumbnail cardDeckThumbnail)
    {
        // Remove
        cardDeckThumbnail.transform.SetParent(cardScrollerContent);
        SelectingCardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.CardPool;
        for (int i = 0; i < PlayerCardDeck.Length; i++) 
        {
            if (PlayerCardDeck[i] == cardDeckThumbnail.UCard)
            {
                PlayerCardDeck[i] = null;
                DeselectingCard();
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
                PlayerCardDeck[i] = SelectingCardDeckThumbnail.UCard;
                
                cardDeckThumbnail.transform.SetParent(cardScrollerContent);
                cardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.CardPool;
                SelectingCardDeckThumbnail.CardStatus = CardDeckThumbnail.Status.InDeck;
                cardSlots[i].PlaceCard(SelectingCardDeckThumbnail);
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
        cardPage.SetAsLastSibling();
        cardDeckContent.SetActive(true);
        turretDeckContent.SetActive(false);
    }
    
}