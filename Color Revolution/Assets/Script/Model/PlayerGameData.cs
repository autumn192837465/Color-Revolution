using System;
using System.Collections.Generic;
using CR;
using CR.Model;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class PlayerGameData
    {
        public int Hp;
        public int Coin;
        public readonly UCard[] CardDeck;
        public readonly int RainbowTurretCoinEarnings;
        public readonly int CoinsEarnedPerEnemyKilled;
        
        public readonly float FreezeEffectTime;
        public readonly float PoisonEffectTime;
        public readonly float BurnEffectTime;
        
        public readonly float FreezeSpeedDebuffPercentage;
        public readonly float PoisonActivateTime;
        public readonly float BurnAmplifier;
        
        public readonly float CriticalAmplifier;
        public readonly float SuperCriticalAmplifier;
        public readonly float BulletSpeed;
        
        
        public PlayerGameData(PlayerDataManager playerDataManager)
        {
            CardDeck = playerDataManager.CardDeck;
            
            Hp = playerDataManager.Hp;
            Coin = playerDataManager.StartCoin;
            RainbowTurretCoinEarnings = playerDataManager.RainbowTurretCoinEarnings;
            CoinsEarnedPerEnemyKilled = playerDataManager.CoinsEarnedPerEnemyKilled;

            FreezeEffectTime = playerDataManager.FreezeEffectTime;
            PoisonEffectTime = playerDataManager.PoisonEffectTime;
            BurnEffectTime = playerDataManager.BurnEffectTime;
            
            FreezeSpeedDebuffPercentage = playerDataManager.FreezeSpeedDebuffPercentage;
            PoisonActivateTime = playerDataManager.PoisonActivateTimer;
            BurnAmplifier = playerDataManager.BurnAmplifier;

            CriticalAmplifier = playerDataManager.CriticalAmplifier;
            SuperCriticalAmplifier = playerDataManager.SuperCriticalAmplifier;
            
            BulletSpeed = playerDataManager.BulletSpeed;

        }
    }    
}
