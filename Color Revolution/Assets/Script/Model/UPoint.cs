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
        public int Count;
        public PointType PointType;
        public MPoint MPoint => DataManager.Instance.GetPointData(PointType);
    }    
}
