using System;
using CR.Model;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class UPoint
    {
        public UPoint(PointType type, int count = 0)
        {
            Count = count;
            PointType = type;
        }
        public UPoint(PointTuple tuple)
        {
            Count = tuple.Count;
            PointType = tuple.PointType;
        }
        
        public int Count;
        public PointType PointType;
        public MPoint MPoint => DataManager.Instance.GetPointData(PointType);
    }
    
    
    [Serializable]
    public class PointTuple
    {
        public PointTuple(PointType type)
        {
            Count = 0;
            PointType = type;
        }
        
        public PointTuple(PointType type, int count)
        {
            Count = count;
            PointType = type;
        }
        
        public PointType PointType;
        public int Count;
        
    }
}
