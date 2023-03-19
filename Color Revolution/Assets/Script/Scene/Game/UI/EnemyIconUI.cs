using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using Kinopi.Enums;
using TMPro;

public class EnemyIconUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    
    public void InitializeUI(Sprite thumbnail)
    {
        icon.sprite = thumbnail;

    }
}