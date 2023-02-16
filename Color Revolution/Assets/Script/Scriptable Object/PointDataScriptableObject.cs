using System.Collections.Generic;
using CB.Model;
using Kinopi.Enums;
using UnityEngine;


[CreateAssetMenu(fileName = "PointData", menuName = "ScriptableObjects/Point Data", order = 1)]
public class PointDataScriptableObject : ScriptableObject
{
    public List<MPoint> PointDataList;
    public Dictionary<PointType, MPoint> PointDataCache { get; private set; }
    
    public void InitializePointData()
    {
        PointDataCache = new();
        foreach (var data in PointDataList)
        {
            PointDataCache.Add(data.PointType, data);
        }
    }
}