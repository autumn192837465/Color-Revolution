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
            UPointDataList = new();
            UPointDataList.Add(new UPoint(PointType.RainbowCandy, 10000));
            UCardDataList = new List<UCard>();
            UCard temp = new UCard(CardType.AddRedAttack);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddBlueAttack);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddGreenAttack);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddAttackRange);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddAttackSpeed);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddHitRate);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddCriticalRate);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddPoisonRate);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddBurnRate);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddFreezeRate);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddSuperCriticalRate);
            UCardDataList.Add(temp);
            temp = new UCard(CardType.AddOperatingTime);
            UCardDataList.Add(temp);
            
            CardDeck = new UCard[Constants.DeckCardCount];
            for (int i = 0; i < CardDeck.Length; i++)
            {
                CardDeck[i] = UCardDataList[i];
            }


            ResearchList = new List<ResearchType>();
        }
        
        
        
        public List<UPoint> UPointDataList;     // Todo : to dictionary
        public List<UCard> UCardDataList;       // Todo : to dictionary
        public UCard[] CardDeck;
        public List<ResearchType> ResearchList;
    }    
    
    
}
