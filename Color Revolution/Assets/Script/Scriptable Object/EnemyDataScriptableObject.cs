using CB.Model;
using CR.Model;
using UnityEditor;
using UnityEngine;

namespace CR.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy Data", order = 1)]
    public class EnemyDataScriptableObject : ScriptableObject
    {
        public Sprite Thumbnail;
        public EnemyData EnemyData;
        
        public void OnValidate()
        {
            if (EnemyData.Health.MaxHealth.RedValue != EnemyData.Health.CurrentHealth.RedValue || 
                EnemyData.Health.MaxHealth.GreenValue != EnemyData.Health.CurrentHealth.GreenValue || 
                EnemyData.Health.MaxHealth.BlueValue != EnemyData.Health.CurrentHealth.BlueValue)
            {
                EnemyData.Health.MaxHealth = new RGB(EnemyData.Health.CurrentHealth);
                
            }
        }
    }
    
   
}

