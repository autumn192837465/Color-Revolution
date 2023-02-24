﻿using Kinopi.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
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
        

        public const int PlayerBaseHp = 5;
        public const int PlayerBaseCoin = 100;
        
        public const float EffectTime = 5;
        public const uint DeckCardCount = 9;
        public const uint MaxCardLevel = 10;

        public const float CriticalPercentage = 2f;
        
        public const float FrozenSpeedDebuffPercentage = 0.5f;
        public const float PoisonActivateTimer = 1f;
        public const float BurningPercentage = 1.5f;
        
        
        public static readonly RGB PoisonDamage = new RGB(1, 1, 1);
    }


}

