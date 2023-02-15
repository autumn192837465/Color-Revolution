using System;
using System.Collections.Generic;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "Card Level Data", menuName = "ScriptableObjects/Card Level Data", order = 1)]
public class CardLevelDataScriptableObject : ScriptableObject
{
    public List<MCardLevel> CardLevelDataList;
    public Dictionary<int, MCardLevel> CardLevelDataCache { get; private set; }

    public void InitializeCardLevelData()
    {
        CardLevelDataCache = new();
        foreach (var data in CardLevelDataList)
        {
            CardLevelDataCache.Add(data.Level, data);
        }
    }

    [ContextMenu("Set Data")]
    public void SetData()
    {
        CardLevelDataList = new List<MCardLevel>();
        int baseCost = 50;
        int level = 10;
        for (int i = 1; i <= level; i++)
        {
            CardLevelDataList.Add(new MCardLevel()
            {
                Level = i,
                UpgradeCost = i < level ? baseCost : 0,
            });
            baseCost *= 2;
        }
    }
}
