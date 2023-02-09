using System;
using System.Collections.Generic;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using UnityEngine;


[CreateAssetMenu(fileName = "Card Data", menuName = "ScriptableObjects/Card Data", order = 1)]
public class CardDataScriptableObject : ScriptableObject
{
    public List<MCardInt> CardDataList;
    public Dictionary<CardType, MCard> cardDataCache;

    public void InitializeCardData()
    {
        cardDataCache = new();
        foreach (var data in CardDataList)
        {
            cardDataCache.Add(data.CardType, data);
        }
    }
}
