using System;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR.Model;
using Kinopi.Constants;
using Kinopi.Enums;
using UnityEngine;

namespace CR
{
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        [HideInInspector] public PlayerData PlayerData;
        public Dictionary<PointType, UPoint> UPointCache;
        public HashSet<ResearchType> ResearchCache;
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            LoadPlayerData();
        }

        public int PlayerHp
        {
            get
            {
                int hp = PlayerData.BaseHp;
                if (HasResearch(ResearchType.Add1Hp_1)) hp += Constants.AddHpAmount;
                if (HasResearch(ResearchType.Add1Hp_2)) hp += Constants.AddHpAmount;
                if (HasResearch(ResearchType.Add1Hp_3)) hp += Constants.AddHpAmount;
                if (HasResearch(ResearchType.Add1Hp_4)) hp += Constants.AddHpAmount;
                if (HasResearch(ResearchType.Add1Hp_5)) hp += Constants.AddHpAmount;
                return hp;
            }
        }

        public int PlayerBaseCoin
        {
            get
            {
                int coin = PlayerData.BaseCoin;
                if (HasResearch(ResearchType.AddCoin_1)) coin += Constants.AddCoinAmount;
                if (HasResearch(ResearchType.AddCoin_2)) coin += Constants.AddCoinAmount;
                if (HasResearch(ResearchType.AddCoin_3)) coin += Constants.AddCoinAmount;
                if (HasResearch(ResearchType.AddCoin_4)) coin += Constants.AddCoinAmount;
                if (HasResearch(ResearchType.AddCoin_5)) coin += Constants.AddCoinAmount;
                return coin;
            }   
        }
        
        public int PlayerBaseCoinPerRainbowTurret
        {
            get
            {
                int coin = PlayerData.BaseCoinPerRainbowTurret;
                return coin;
            }   
        }

        public int PlayerBaseCoinPerEnemyKilled
        {
            get
            {
                int coin = PlayerData.BaseCoinPerEnemyKilled;
                return coin;
            }
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

        public void AddResearch(ResearchType researchType)
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

        public bool CanResearch(ResearchType type)
        {
            var researchData = DataManager.Instance.GetResearchData(type);
            return !ResearchCache.Contains(type) && (researchData.RequiredResearchType == ResearchType.None || HasResearch(researchData.RequiredResearchType));
        }
        
        [ContextMenu("Clear Data")]
        public void ClearData()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsManager.PlayerDataKey);
        }
    }    
}
