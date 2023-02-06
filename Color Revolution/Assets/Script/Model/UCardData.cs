using System;
using CR.Model;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class UCardData
    {
        public int Level;
        public CardType CardType;
        public MCardData MCard => DataManager.Instance.GetCardData(CardType);
    }    
}
