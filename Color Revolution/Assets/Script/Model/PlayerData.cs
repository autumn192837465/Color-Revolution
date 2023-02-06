using System;
using System.Collections.Generic;
using CB.Model;
using Kinopi.Constants;
using Kinopi.Enums;
using UnityEngine;


namespace CR.Model
{
    [Serializable]
    public class PlayerData
    {
        public PlayerData()
        {
            UCardDataList = new List<UCardData>();
            UCardData temp = new UCardData();
            temp.Level = 1;
            temp.CardType = CardType.AddAttackRange;
            UCardDataList.Add(temp);
            
            temp = new UCardData();
            temp.Level = 1;
            temp.CardType = CardType.AddAttackSpeed;
            UCardDataList.Add(temp);
            
            CardDeck = new UCardData[Constants.DeckCardCount];
            CardDeck[0] = UCardDataList[0];

        }
        public List<UCardData> UCardDataList;
        public UCardData[] CardDeck;
    }    
    
    
}
