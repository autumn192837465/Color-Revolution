using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR.Model;
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

        public void AddUPoint(PointType pointType, int count)
        {
            UPoint uPoint = PlayerData.UPointDataList.FirstOrDefault(x => x.PointType == pointType);
            if (uPoint is null)
            {
                uPoint = new UPoint(pointType, count);
                PlayerData.UPointDataList.Add(uPoint);
            }
            else
            {
                uPoint.Count += count;
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
