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
        public int Level;
        public Sprite Sprite;
        public int Cost;
        [TextArea(5, 3)] 
        public string Description;
    }


    [Serializable]
    public class MCardInt : MCard
    {
        public int ArgInt;
    }

    public class MCardFloat : MCard
    {
        public float ArgFloat;
    }
}
