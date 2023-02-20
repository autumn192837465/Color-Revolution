using System;
using System.Collections.Generic;
using CB.Model;
using Kinopi.Enums;
using Unity.Collections;
using UnityEngine;

namespace CR.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level Data", order = 1)]
    public class LevelDataScriptableObject : ScriptableObject
    {
        public string LevelName;
        
        
        [Header("Wave")]
        public List<WaveSpawnData> WaveSpawnList;
        public int MaxWaveCount => WaveSpawnList.Count;
        
        public Enemy GetEnemy(int spawnIndex, int enemyIndex) => WaveSpawnList[spawnIndex].EnemySpawnGroupList[enemyIndex].enemy;
        public float GetEnemySpawnGroupInterval(int waveIndex, int spawnGroupIndex) => WaveSpawnList[waveIndex].EnemySpawnGroupList[spawnGroupIndex].interval;
        public int GetSpawnGroupEnemyCount(int waveIndex, int spawnGroupIndex) => WaveSpawnList[waveIndex].EnemySpawnGroupList[spawnGroupIndex].count;
        public int GetEnemySpawnGroupCount(int waveIndex) => WaveSpawnList[waveIndex].EnemySpawnGroupList.Count;
        public List<PointTuple> LevelReward; 
        

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
#if UNITY_EDITOR
            for (int i = 0; i < WaveSpawnList.Count; i++)
            {
                WaveSpawnList[i].Name = $"Wave {i + 1}";
            }
#endif
        }
        
    }

    
}
