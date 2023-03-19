using System;
using CR.Model;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class EnemyData
    {
        public Sprite Thumbnail;
        public RGBHealth Health;  // 攻擊力
        public float Speed;   // 攻速
    }    
}
