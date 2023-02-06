using System;
using CR.Model;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class UCard
    {
        public int Level;
        public CardType CardType;
        public MCard MCard => DataManager.Instance.GetCardData(CardType);
    }    
}
