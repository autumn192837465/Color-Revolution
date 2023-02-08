using System;
using System.Collections.Generic;
using CR.Model;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class PlayerGameData
    {
        public int Hp;
        public int Coin;
        public UCard[] CardDeck;
    }    
}
