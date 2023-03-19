using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using CR.ScriptableObjects;
using Kinopi.Enums;
using TMPro;

public class EnemyIconUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private List<GameObject> hintObjects;
    public void InitializeUI(EnemyDataScriptableObject data)
    {
        icon.sprite = data.Thumbnail;
        hintObjects[0].SetActive(data.HasRed);
        hintObjects[1].SetActive(data.HasGreen);
        hintObjects[2].SetActive(data.HasBlue);
        
        
    }
}