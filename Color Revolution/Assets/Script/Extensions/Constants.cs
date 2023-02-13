using Kinopi.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using CR.Model;
using UnityEngine;

namespace Kinopi.Constants
{
    public static class Constants
    {
        public static readonly Color DisableColor = new Color32(255, 36, 68, 255);
        
        public const int DrawCost = 5;
     
        public const float EffectTime = 5;
        public const uint DeckCardCount = 9;
        


        public const float FrozenSpeedDebuffPercentage = 0.5f;
        public const float PoisonActivateTimer = 1f;
        
        public static readonly RGB PoisonDamage = new RGB(1, 1, 1);
    }


}

