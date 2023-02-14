using System;
using CR.Model;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class TurretBasicData
    {
        public RGB AttackDamage;  // 攻擊力
        public float BulletPerSecond;   // 攻速   x bullet/sec
        public float AttackRange;   // 攻擊範圍
        public float CriticalRate;  // 爆擊率
        public float HitRate;       // 命中率
        public float PoisonRate;       // 命中率
        public float FreezeRate;       // 命中率
        public float BurnRate;       // 命中率
        public float OperatingTime; // 運轉時間
        public float CooldownTime; // 運轉時間
    }    
}
