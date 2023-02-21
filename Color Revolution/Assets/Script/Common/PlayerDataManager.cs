using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR.Model;
using JetBrains.Annotations;
using Kinopi.Enums;
using UnityEngine;

namespace CR
{
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        public PlayerData PlayerData;
        public Dictionary<PointType, UPoint> UPointCache;
        public HashSet<ResearchType> ResearchCache;
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            LoadPlayerData();
        }
        
        public void AddUPoint(PointTuple tuple)
        {
            AddUPoints(new List<PointTuple> { tuple });
        }
        
        public void AddUPoints(List<PointTuple> tupleList)
        {
            foreach (var tuple in tupleList)
            {
                UPoint uPoint =  UPointCache.GetValueOrDefault(tuple.PointType);
                if (uPoint is null)
                {
                    uPoint = new UPoint(tuple);
                    PlayerData.UPointDataList.Add(uPoint);
                }
                else
                {
                    uPoint.Count += tuple.Count;
                }    
            }
            SavePlayerData();
            CreateUPointCache();
        }
        
        public void SubUPoint(PointTuple tuple)
        {
            SubUPoints(new List<PointTuple> { tuple });
        }
        
        public void SubUPoints(List<PointTuple> tupleList)
        {
            foreach (var tuple in tupleList)
            {
                UPoint uPoint = UPointCache.GetValueOrDefault(tuple.PointType);
                if (uPoint is null)
                {
                    Debug.LogError($"{Enum.GetName(typeof(PointType), tuple.PointType)} is not enough!");
                }
                else
                {
                    uPoint.Count -= tuple.Count;
                    if(uPoint.Count < 0)
                        Debug.LogError($"{Enum.GetName(typeof(PointType), tuple.PointType)} is not enough!");
                }

            }
          
            SavePlayerData();
            CreateUPointCache();
        }

        
        public UPoint GetUPoint(PointType pointType)
        {
            return UPointCache.GetValueOrDefault(pointType, new UPoint(pointType));
        }
        
        private void SavePlayerData()
        {
            string dataString = JsonUtility.ToJson(PlayerData);
            PlayerPrefsManager.PlayerDataString = dataString;
        }

        private void LoadPlayerData()
        {
            string dataString = PlayerPrefsManager.PlayerDataString;
            PlayerData = JsonUtility.FromJson<PlayerData>(dataString) ?? new PlayerData();
            CreateUPointCache();
            CreateResearchCache();
        }

        private void CreateUPointCache()
        {
            UPointCache = new Dictionary<PointType, UPoint>();
            foreach (var uPoint in PlayerData.UPointDataList)
            {
                UPointCache[uPoint.PointType] = uPoint;
            }
        }

        private void AddResearch(ResearchType researchType)
        {
            if (PlayerData.ResearchList.Contains(researchType)) return;
            
            PlayerData.ResearchList.Add(researchType);
            SavePlayerData();
            CreateResearchCache();

        }
        
        private void CreateResearchCache()
        {
            ResearchCache = PlayerData.ResearchList.ToHashSet();
        }

        public bool HasResearch(ResearchType type)
        {
            return ResearchCache.Contains(type);
        }
        
        
        

    }    
}
