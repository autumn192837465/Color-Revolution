using CB.Model;
using UnityEngine;

namespace CR.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy Data", order = 1)]
    public class EnemyDataScriptableObject : ScriptableObject
    {
        public EnemyData EnemyData;
    }    
}

