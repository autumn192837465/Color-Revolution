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
        [SerializeField] private TextMeshProUGUI attackSpeedText;
        [SerializeField] private TextMeshProUGUI operatingTimeText;
        [SerializeField] private TextMeshProUGUI coolDownTimeText;
        [SerializeField] private TextMeshProUGUI sellCostText;
        [SerializeField] private TextMeshProUGUI costText;



        public void InitializeUI(Turret turret)
        {

            turretImage.sprite = turret.TurretData.Sprite;
            var data = turret.TurretBasicData;
            redDamageText.text = data.AttackDamage.RedValue.ToString();
            greenDamageText.text = data.AttackDamage.GreenValue.ToString();
            blueDamageText.text = data.AttackDamage.BlueValue.ToString();

            attackRangeDamageText.text = data.AttackRange.ToString();
            attackSpeedText.text = data.BulletPerSecond.ToString();
            operatingTimeText.text = $"{data.OperatingTime} sec";
            coolDownTimeText.text = $"{data.CooldownTime} sec";

            sellCostText.text = turret.SellCost.ToString();
        }

        public void InitializeUI(TurretData data)
        {
            turretImage.sprite = data.Sprite;
            var basicData = data.BasicDataScriptableObject.BasicData;
            redDamageText.text = basicData.AttackDamage.RedValue.ToString();
            greenDamageText.text = basicData.AttackDamage.GreenValue.ToString();
            blueDamageText.text = basicData.AttackDamage.BlueValue.ToString();

            attackRangeDamageText.text = basicData.AttackRange.ToString();
            attackSpeedText.text = basicData.BulletPerSecond.ToString();
            operatingTimeText.text = $"{basicData.OperatingTime} sec";
            coolDownTimeText.text = $"{basicData.CooldownTime} sec";
            costText.text = data.Cost.ToString();
        }
    }    
}
