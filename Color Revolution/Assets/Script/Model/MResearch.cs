using System;
using Kinopi.Enums;
using UnityEngine;

namespace CB.Model
{
    [Serializable]
    public class MResearch 
    {
        public ResearchType ResearchType;
        public ResearchType RequiredResearchType;
        public Sprite Sprite;
        public int Cost;
        [TextArea(5, 3)] 
        public string Description;
    }
}
