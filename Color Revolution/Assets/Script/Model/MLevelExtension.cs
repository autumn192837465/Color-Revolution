using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CR.Model;
using CR.ScriptableObjects;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    public partial class MLevel
    {
        public Enemy GetEnemy(int spawnIndex, int enemyIndex) => WaveSpawnList[spawnIndex].EnemySpawnGroupList[enemyIndex].enemy;
        public float GetEnemySpawnGroupInterval(int waveIndex, int spawnGroupIndex) => WaveSpawnList[waveIndex].EnemySpawnGroupList[spawnGroupIndex].interval;
        public int GetSpawnGroupEnemyCount(int waveIndex, int spawnGroupIndex) => WaveSpawnList[waveIndex].EnemySpawnGroupList[spawnGroupIndex].count;
        public int GetEnemySpawnGroupCount(int waveIndex) => WaveSpawnList[waveIndex].EnemySpawnGroupList.Count;
        public int MaxWaveCount => WaveSpawnList.Count;

        public List<Sprite> GetEnemyThumbnails
        {
            get
            {
                HashSet<Sprite> spriteHashSet = new();

                foreach (var enemySpawnGroup in WaveSpawnList.SelectMany(waveSpawnData => waveSpawnData.EnemySpawnGroupList))
                {
                    spriteHashSet.Add(enemySpawnGroup.enemy.EnemyDataScriptableObject.Thumbnail);
                }
                return spriteHashSet.ToList();
            }
        }
    }
}
    