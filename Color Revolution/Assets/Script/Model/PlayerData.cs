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
            BaseHp = Constants.PlayerBaseHp;
            BaseCoin = Constants.PlayerBaseCoin;
            BaseCoinPerRainbowTurret = Constants.BaseCoinPerRainbowTurret;
            UPointDataList = new();
            UPointDataList.Add(new UPoint(PointType.RainbowCandy, 100));
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
            temp = new UCard(CardType.AddGreenAttack);
            UCardDataList.Add(temp);
            
            CardDeck = new UCard[Constants.DeckCardCount];
            for (int i = 0; i < CardDeck.Length; i++)
            {
                CardDeck[i] = UCardDataList[i];
            }


            ResearchList = new List<ResearchType>();
        }

        public int BaseHp;
        public int BaseCoin;
        public int BaseCoinPerRainbowTurret;
        public List<UPoint> UPointDataList;     // Todo : to dictionary
        public List<UCard> UCardDataList;       // Todo : to dictionary
        public UCard[] CardDeck;
        public List<ResearchType> ResearchList;
    }    
    
    
}
