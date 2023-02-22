using CB.Model;
using UnityEngine;

namespace CR.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level Data", order = 1)]
    public class LevelDataScriptableObject : ScriptableObject
    {
        public MLevel MLevel;
        
        private void OnValidate()
        {
#if UNITY_EDITOR
            for (int i = 0; i < MLevel.WaveSpawnList.Count; i++)
            {
                MLevel.WaveSpawnList[i].Name = $"Wave {i + 1}";
            }
#endif
        }
    }
}


