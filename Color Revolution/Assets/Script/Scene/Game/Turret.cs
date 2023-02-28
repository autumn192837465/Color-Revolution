using System;
using System.Linq;
using CB.Model;
using CR.Model;
using Kinopi.Enums;
using Kinopi.Extensions;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CR.Game
{
  
    
    public abstract class Turret : MonoBehaviour
    {
        [SerializeField] private MMF_Player placeFeedbacks;
        public abstract TurretType TurretType { get; }
        
        protected int turretCost;
        public int SellCost => turretCost / 2;


        public abstract void Initialize();

        public void AddTurretValue(int amount)
        {
            turretCost += amount;
        }

        public void PlayPlaceFeedbacks()
        {
            placeFeedbacks.PlayFeedbacks();
        }
    }
    
    
}
