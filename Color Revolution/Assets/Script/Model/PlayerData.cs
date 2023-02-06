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
            UCardDataList = new List<UCard>();
            UCard temp = new UCard(CardType.AddAttackRange);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddAttackSpeed);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.Test1);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.Test2);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.Test3);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.Test4);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.Test5);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.Test6);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.Test7);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddBlueAttack);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddRedAttack);
            UCardDataList.Add(temp);
            
            CardDeck = new UCard[Constants.DeckCardCount];
            for (int i = 0; i < CardDeck.Length; i++)
            {
                CardDeck[i] = UCardDataList[i];
            }

        }
        public List<UCard> UCardDataList;
        public UCard[] CardDeck;
    }    
    
    
}
