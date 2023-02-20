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
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            LoadPlayerData();
        }
    
        void Start()
        {
        
        }
    
        void Update()
        {
        
        }

        public void AddUPoint(PointTuple tuple)
        {
            AddUPoints(new List<PointTuple> { tuple });
        }
        
        public void AddUPoints([ItemCanBeNull] List<PointTuple> tupleList)
        {
            foreach (var tuple in tupleList)
            {
                UPoint uPoint = PlayerData.UPointDataList.FirstOrDefault(x => x.PointType == tuple.PointType);
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
        }
        
        

        public void SubUPoint(PointTuple tuple)
        {
            UPoint uPoint = PlayerData.UPointDataList.FirstOrDefault(x => x.PointType == tuple.PointType);
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
            
            SavePlayerData();
        }
        
        
        public UPoint GetUPoint(PointType pointType)
        {
            return PlayerData.UPointDataList.FirstOrDefault(x => x.PointType == pointType) ?? new UPoint(pointType);
        }
        
        

        public void SavePlayerData()
        {
            string dataString = JsonUtility.ToJson(PlayerData);
            PlayerPrefsManager.PlayerDataString = dataString;
        }

        public void LoadPlayerData()
        {
            string dataString = PlayerPrefsManager.PlayerDataString;
            
            PlayerData = JsonUtility.FromJson<PlayerData>(dataString) ?? new PlayerData();
        }
        
        #region AddUIEvent
        #endregion

        #region RemoveUIEvent
        #endregion
    }    
}
