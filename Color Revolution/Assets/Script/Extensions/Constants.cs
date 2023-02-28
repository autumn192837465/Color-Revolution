using CR.Model;
using UnityEngine;

namespace Kinopi.Constants
{
    public static class Constants
    {
        public static readonly Color EnableColor = new Color32(255, 255, 255, 255);
        public static readonly Color DisableColor = new Color32(255, 36, 68, 255);
        
        public const int DrawCost = 5;
        public const float FloatingTextLifeTime = 0.7f;
        public const float FloatingTextMoveFactor = 0.2f;
        public const float ResultOpeningWaitTime = 1f;
        public const int MaxVolume = 20;
        

        public const int PlayerBaseHp = 20;
        public const int PlayerBaseStartCoin = 100;
        public const int BaseCoinsEarnedPerEnemyKilled = 10;
        public const int BaseRainbowTurretCoinEarnings = 10;

        public const float BaseFreezeEffectTime = 3f;
        public const float BasePoisonEffectTime = 3f;
        public const float BaseBurnEffectTime = 3f;
        
        
        public static readonly Rate BaseFreezeSpeedDebuffPercentage = new(20);
        public const float BasePoisonActivateTimer = 1f;
        public static readonly Amplifier BaseBurnAmplifier = new(120);
        
        public static readonly Amplifier BaseCriticalAmplifier = new (150);
        public static readonly Amplifier BaseSuperCriticalAmplifier = new (250);

        public const float BaseBulletSpeed = 3f;
        
        
        public const uint DeckCardCount = 9;
        public const uint MaxCardLevel = 10;

        
        
        
        
        
        public static readonly RGB PoisonDamage = new RGB(1, 1, 1);
        
        // Research
        public const int AddHpAmount = 1;
        public const int AddCoinAmount = 10;
    }


}

