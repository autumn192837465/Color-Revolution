using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using TMPro;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private IconWithTextUI costIcon;
    
    public CardData CardData { get; private set; }
    
    public void InitializeUI(CardData data)
    {
        CardData = data;
        descriptionText.text = CardData.Description;
        costIcon.SetText(data.Cost);
    }   
}