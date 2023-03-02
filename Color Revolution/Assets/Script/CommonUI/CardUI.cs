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
    
    public MCard MCard { get; private set; }
    
    public virtual void InitializeUI(UCard data)
    {
        MCard = data.MCard;
        descriptionText.text = MCard.GetDescription(data.Level);
        costIcon.SetText(data.MCard.Cost);
    }   
    
}