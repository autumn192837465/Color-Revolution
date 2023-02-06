using System;
using CR.Model;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class MCard
    {
        public CardType CardType;
        public Sprite Sprite;
        public int Cost;
        [TextArea(10, 3)] 
        public string Description;
    }    
}
