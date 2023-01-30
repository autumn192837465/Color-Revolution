using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using CR.Game;
using TMPro;

namespace CR.Game
{
    public class TurretPanelUI : MonoBehaviour
    {
        [SerializeField] private Image turretImage;
        [SerializeField] private TextMeshProUGUI redDamageText;
        [SerializeField] private TextMeshProUGUI greenDamageText;
        [SerializeField] private TextMeshProUGUI blueDamageText;
        [SerializeField] private TextMeshProUGUI attackRangeDamageText;
        [SerializeField] private TextMeshProUGUI operatingTimeText;
        [SerializeField] private TextMeshProUGUI coolDownTimeText;
        [SerializeField] private TextMeshProUGUI sellCostText;



        public void InitializeUI(Turret turret)
        {

            turretImage.sprite = turret.TurretData.Sprite;
            var data = turret.TurretBasicData;
            redDamageText.text = data.AttackDamage.RedValue.ToString();
            greenDamageText.text = data.AttackDamage.GreenValue.ToString();
            blueDamageText.text = data.AttackDamage.BlueValue.ToString();

            attackRangeDamageText.text = data.AttackRange.ToString();
            operatingTimeText.text = $"{data.OperatingTime} sec";
            coolDownTimeText.text = $"{data.CooldownTime} sec";

            sellCostText.text = turret.SellCost.ToString();
        }
    }    
}
