using System;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public partial class MLevel
    {
        public string LevelName; 
        public MapDataScriptableObject MapData;
        [Header("Wave")]
        public List<WaveSpawnData> WaveSpawnList;
        
        [Header("Reward")]
        public List<PointTuple> LevelReward; 
    }
    
    [Serializable]
    public class WaveSpawnData
    {
#if UNITY_EDITOR
        [Unity.Collections.ReadOnly]
        public string Name;
#endif
        public List<EnemySpawnGroup> EnemySpawnGroupList;
    }
       
    [Serializable]
    public class EnemySpawnGroup
    {
        public Enemy enemy;
        public int count;
        public float interval;
    }
}
