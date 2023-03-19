using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using Kinopi.Enums;
using TMPro;

public class RewardItemIconUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;
    public MPoint MPointData { get; private set; }

    public void InitializeUI(PointType type, int count)
    {
        MPointData = DataManager.Instance.GetPointData(type);
        icon.sprite = MPointData.Sprite;
        countText.text = count.ToString();
    }
}