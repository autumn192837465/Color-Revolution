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
    
    public UCard UCard { get; private set; }
    
    public virtual void InitializeUI(UCard data)
    {
        UCard = data;
        RefreshUI();
    }

    public virtual void RefreshUI()
    {
        descriptionText.text = UCard.GetDescription();
        costIcon.SetText(UCard.MCard.Cost);
    }
    
}