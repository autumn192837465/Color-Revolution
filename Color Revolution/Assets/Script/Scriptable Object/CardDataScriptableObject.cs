using System;
using System.Collections.Generic;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using UnityEngine;


[CreateAssetMenu(fileName = "Card Data", menuName = "ScriptableObjects/Card Data", order = 1)]
public class CardDataScriptableObject : ScriptableObject
{
    public List<MCardInt> CardIntDataList;
    public List<MCardFloat> CardFloatDataList;
    public Dictionary<CardType, MCard> CardDataCache { get; private set; }

    public void InitializeCardData()
    {
        CardDataCache = new();
        foreach (var data in CardIntDataList)
        {
            CardDataCache.Add(data.CardType, data);
        }
        
        foreach (var data in CardFloatDataList)
        {
            CardDataCache.Add(data.CardType, data);
        }
    }
}
