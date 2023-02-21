using System.Collections.Generic;
using CB.Model;
using Kinopi.Enums;
using UnityEngine;


[CreateAssetMenu(fileName = "ResearchData", menuName = "ScriptableObjects/Research Data", order = 1)]
public class ResearchDataScriptableObject : ScriptableObject
{
    public List<MResearch> ResearchDataList;
    public Dictionary<ResearchType, MResearch> ResearchDataCache { get; private set; }
    
    public void InitializeResearchData()
    {
        ResearchDataCache = new();
        foreach (var data in ResearchDataList)
        {
            ResearchDataCache.Add(data.ResearchType, data);
        }
    }
}