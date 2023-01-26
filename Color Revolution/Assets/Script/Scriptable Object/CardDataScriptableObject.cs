using System;
using System.Collections.Generic;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using UnityEngine;


[CreateAssetMenu(fileName = "Card Data", menuName = "ScriptableObjects/Card Data", order = 1)]
public class CardDataScriptableObject : ScriptableObject
{
    public List<CardData> CardDataList;
    public Dictionary<CardType, CardData> cardDataCache;

    public void InitializeCardData()
    {
        cardDataCache = new();
        foreach (var data in CardDataList)
        {
            cardDataCache.Add(data.CardType, data);
        }
    }
}
