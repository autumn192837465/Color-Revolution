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

        [Range(0, 100)] public int CriticalRate;  // 爆擊率
        [Range(0, 100)] public int HitRate;       // 命中率
        [Range(0, 100)] public int PoisonRate;       // 命中率
        [Range(0, 100)] public int FreezeRate;       // 命中率
        [Range(0, 100)] public int BurnRate;       // 命中率
        public float OperatingTime; // 運轉時間
        public float CooldownTime; // 運轉時間
    }    
}
