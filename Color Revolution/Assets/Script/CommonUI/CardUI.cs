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
    
    public MCardData MCardData { get; private set; }
    
    public virtual void InitializeUI(MCardData data)
    {
        MCardData = data;
        descriptionText.text = MCardData.Description;
        costIcon.SetText(data.Cost);
    }   
}