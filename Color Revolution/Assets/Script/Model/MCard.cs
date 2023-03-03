using System;
using CR.Model;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public abstract class MCard
    {
        public CardType CardType;
        public Sprite Sprite;
        public int Cost;
        [TextArea(5, 3)] 
        public string Description;

        public abstract string GetDescription(int level);
        public abstract string GetUpgradeDescription(int level);
    }


    [Serializable]
    public class MCardInt : MCard
    {
        public int ArgInt;
        public int GetValue(int level) => level * ArgInt;
        public override string GetDescription(int level) => string.Format(Description, GetValue(level));

        public override string GetUpgradeDescription(int level) => string.Format(Description, $"{GetValue(level)} -> <color=green>{GetValue(level + 1)}</color>");
        
            
    }

    [Serializable]
    public class MCardFloat : MCard
    {
        public float ArgFloat;

        public float GetValue(int level)
        {
            return level * ArgFloat;    
        } 
        
        public override string GetDescription(int level) => string.Format(Description, GetValue(level));
        public override string GetUpgradeDescription(int level) => string.Format(Description, $"{GetValue(level)} -> <color=green>{GetValue(level + 1)}</color>");
    }
}
