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
            Turret.TargetPriority = Turret.TargetPriority.Next();
            SetPriorityText();
        }

        private void PrevPriority()
        {
            Turret.TargetPriority = Turret.TargetPriority.Prev();
            SetPriorityText();
        }

        private void SetPriorityText()
        {
            targetPriorityText.text = Turret.TargetPriority switch 
            {
                TargetPriority.FirstTarget => "First",
                TargetPriority.MostRedHealth => "Most Red Health",
                TargetPriority.MostGreenHealth => "Most Green Health",
                TargetPriority.MostBlueHealth => "Most Blue Health",
                TargetPriority.Random => "Random",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void InitializeUI(Turret turret)
        {
            Turret = turret;
            base.InitializeUI(turret);
            sellCostText.text = turret.SellCost.ToString();
            SetPriorityText();
        }
        
    }    
}
