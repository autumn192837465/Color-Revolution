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
        public readonly float CriticalAmplifier;
        public readonly float SuperCriticalAmplifier;
        public readonly float FrozenSpeedDebuffPercentage;
        public readonly float PoisonActivateTime;
        public readonly float BurningAmplifier;

        public PlayerGameData(PlayerDataManager playerDataManager)
        {
            Hp = playerDataManager.Hp;
            Coin = playerDataManager.StartCoin;
            CardDeck = playerDataManager.CardDeck;
            CoinPerRainbowTurret = playerDataManager.CoinPerRainbowTurret;
            CoinPerEnemyKilled = playerDataManager.CoinPerEnemyKilled;

            CriticalAmplifier = playerDataManager.CriticalAmplifier;
            SuperCriticalAmplifier = playerDataManager.SuperCriticalAmplifier;
            
            FrozenSpeedDebuffPercentage = playerDataManager.FrozenSpeedDebuffPercentage;
            PoisonActivateTime = playerDataManager.PoisonActivateTimer;
            BurningAmplifier = playerDataManager.BurningAmplifier;
            
        }
    }    
}
