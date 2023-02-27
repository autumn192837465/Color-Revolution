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
        public readonly int CoinPerRainbowTurret;
        public readonly int CoinPerEnemyKilled;
        
        public readonly float FreezeEffectTime;
        public readonly float PoisonEffectTime;
        public readonly float BurnEffectTime;
        
        public readonly float FrozenSpeedDebuffPercentage;
        public readonly float PoisonActivateTime;
        public readonly float BurningAmplifier;
        
        public readonly float CriticalAmplifier;
        public readonly float SuperCriticalAmplifier;
        public readonly float BulletSpeed;
        
        
        public PlayerGameData(PlayerDataManager playerDataManager)
        {
            CardDeck = playerDataManager.CardDeck;
            
            Hp = playerDataManager.Hp;
            Coin = playerDataManager.StartCoin;
            CoinPerRainbowTurret = playerDataManager.CoinPerRainbowTurret;
            CoinPerEnemyKilled = playerDataManager.CoinPerEnemyKilled;

            FreezeEffectTime = playerDataManager.FreezeEffectTime;
            PoisonEffectTime = playerDataManager.PoisonEffectTime;
            BurnEffectTime = playerDataManager.BurnEffectTime;
            
            FrozenSpeedDebuffPercentage = playerDataManager.FrozenSpeedDebuffPercentage;
            PoisonActivateTime = playerDataManager.PoisonActivateTimer;
            BurningAmplifier = playerDataManager.BurningAmplifier;

            CriticalAmplifier = playerDataManager.CriticalAmplifier;
            SuperCriticalAmplifier = playerDataManager.SuperCriticalAmplifier;
            
            BulletSpeed = playerDataManager.BulletSpeed;

        }
    }    
}
