using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class CardSlot : MonoBehaviour
{
    [SerializeField] private Transform cardRoot;

    public CardDeckThumbnail PlacingCardDeckThumbnail =>
        cardRoot.childCount > 0 ? cardRoot.GetChild(0).GetComponent<CardDeckThumbnail>() : null;
    
    public void InitializeUI()
    {

    }

    public void PlaceCard(CardDeckThumbnail card)
    {
        card.transform.SetParent(cardRoot);
    }
    
    
}