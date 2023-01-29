using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using TMPro;

public class TurretPanelUI : MonoBehaviour
{
    [SerializeField] private Image turretImage;
    [SerializeField] private TextMeshProUGUI redDamageText;
    [SerializeField] private TextMeshProUGUI greenDamageText;
    [SerializeField] private TextMeshProUGUI blueDamageText;
    [SerializeField] private TextMeshProUGUI attackRangeDamageText;
    [SerializeField] private TextMeshProUGUI operatingTimeText;
    [SerializeField] private TextMeshProUGUI coolDownTimeText;
    
    public void InitializeUI(Sprite sprite, TurretBasicData turretBasicData)
    {

        turretImage.sprite = sprite;
        redDamageText.text = turretBasicData.AttackDamage.RedValue.ToString();
        greenDamageText.text = turretBasicData.AttackDamage.GreenValue.ToString();
        blueDamageText.text = turretBasicData.AttackDamage.BlueValue.ToString();

        attackRangeDamageText.text = turretBasicData.AttackRange.ToString();
        operatingTimeText.text = $"{turretBasicData.OperatingTime} sec";
        coolDownTimeText.text = $"{turretBasicData.CooldownTime} sec";
    }   
}