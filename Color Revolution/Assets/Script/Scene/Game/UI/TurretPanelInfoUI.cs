using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using Kinopi.Extensions;
using TMPro;
using UnityEditor;

namespace CR.Game
{
    public class TurretPanelInfoUI : TurretInfoUI
    {
        [SerializeField] private Image turretImage;
        [SerializeField] private TextMeshProUGUI sellCostText;
        
        
        [SerializeField] private FeedbackButton leftPriorityButton;
        [SerializeField] private FeedbackButton rightPriorityButton;
        [SerializeField] private TextMeshProUGUI targetPriorityText;

        public Turret Turret { get; private set; }
         
         
        private void Awake()
        {
            leftPriorityButton.OnClick = NextPriority;
            rightPriorityButton.OnClick = PrevPriority;
        }

        private void NextPriority()
        {
            ((OffensiveTurret)Turret).TargetPriority = ((OffensiveTurret)Turret).TargetPriority.Next();
            SetPriorityText();
        }

        private void PrevPriority()
        {
            ((OffensiveTurret)Turret).TargetPriority = ((OffensiveTurret)Turret).TargetPriority.Prev();
            SetPriorityText();
        }

        private void SetPriorityText()
        {
            targetPriorityText.text = ((OffensiveTurret)Turret).TargetPriority switch 
            {
                TargetPriority.FirstTarget => "First",
                TargetPriority.MostRedHealth => "Most Red Health",
                TargetPriority.MostGreenHealth => "Most Green Health",
                TargetPriority.MostBlueHealth => "Most Blue Health",
                TargetPriority.Random => "Random",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void InitializeUI(OffensiveTurret offensiveTurret)
        {
            Turret = offensiveTurret;
            turretImage.sprite = offensiveTurret.OffensiveTurretData.Sprite;
            base.InitializeUI(offensiveTurret.MOffensiveTurret);
            sellCostText.text = offensiveTurret.SellCost.ToString();
            SetPriorityText();
        }
        
        public void InitializeUI(SupportTurret supportTurret)
        {
            Turret = supportTurret;
            turretImage.sprite = supportTurret.SupportTurretData.Sprite;
            base.InitializeUI(supportTurret.SupportTurretData.MSupportTurret);
            sellCostText.text = supportTurret.SellCost.ToString();
        }
    }    
}
