using System;
using CR.Model;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class TurretBasicData
    {
        public RGB AttackDamage;  // 攻擊力
        public float AttackSpeed;   // 攻速
        public float AttackRange;   // 攻擊範圍
        public float CriticalRate;  // 爆擊率
        public float HitRate;       // 命中率
        public float OperatingTime; // 運轉時間
        public float CooldownTime; // 運轉時間
    }    
}
