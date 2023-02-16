using System;
using CR.Model;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class MPoint
    {
        public PointType PointType;
        public Sprite Sprite;
        [TextArea(5, 3)] 
        public string Description;
    }
}
