using System;
using CR.Model;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class MOffensiveTurret
    {
        public RGB AttackDamage;  // 攻擊力
        public float BulletPerSecond;   // 攻速   x bullet/sec
        public float AttackRange;   // 攻擊範圍

        public Rate CriticalRate;  // 爆擊率
        public Rate SuperCriticalRate;  // 超級爆擊率
        public Rate HitRate;       // 命中率
        public Rate PoisonRate;       // 命中率
        public Rate FreezeRate;       // 命中率
        public Rate BurnRate;       // 命中率
        public float OperatingTime; // 運轉時間
        public float CooldownTime; // 運轉時間
        public int Cost;
    }    
}
