using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CR.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Wave Data", order = 1)]
    public class WaveDataScriptableObject : ScriptableObject
    {

        public float WaveInterval;
        public List<WaveSpawnData> WaveSpawnList;


        public Enemy GetEnemy(int spawnIndex, int enemyIndex)
        {
            return WaveSpawnList[spawnIndex].EnemySpawnGroupList[enemyIndex].enemy;
        }
        

        [Serializable]
        public class WaveSpawnData
        {
#if UNITY_EDITOR
            [ReadOnly]
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
        
        
        private void OnValidate()
        {
            for (int i = 0; i < WaveSpawnList.Count; i++)
            {
                WaveSpawnList[i].Name = $"Wave {i + 1}";
            }
        }
        
    }

    
}
