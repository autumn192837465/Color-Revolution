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

        public int Hp
        {
            get
            {
                int value = Constants.PlayerBaseHp;
                if (HasResearch(ResearchType.Add1Hp_1)) value += Constants.AddHpAmount;
                if (HasResearch(ResearchType.Add1Hp_2)) value += Constants.AddHpAmount;
                if (HasResearch(ResearchType.Add1Hp_3)) value += Constants.AddHpAmount;
                if (HasResearch(ResearchType.Add1Hp_4)) value += Constants.AddHpAmount;
                if (HasResearch(ResearchType.Add1Hp_5)) value += Constants.AddHpAmount;
                return value;
            }
        }

        public int StartCoin
        {
            get
            {
                int value = Constants.PlayerBaseStartCoin;
                if (HasResearch(ResearchType.AddStartCoin_1)) value += Constants.AddCoinAmount;
                if (HasResearch(ResearchType.AddStartCoin_2)) value += Constants.AddCoinAmount;
                if (HasResearch(ResearchType.AddStartCoin_3)) value += Constants.AddCoinAmount;
                if (HasResearch(ResearchType.AddStartCoin_4)) value += Constants.AddCoinAmount;
                if (HasResearch(ResearchType.AddStartCoin_5)) value += Constants.AddCoinAmount;
                return value;
            }   
        }

        public UCard[] CardDeck => PlayerData.CardDeck;
        
        public int RainbowTurretCoinEarnings
        {
            get
            {
                int value =  Constants.BaseRainbowTurretCoinEarnings;
                return value;
            }   
        }

        public int CoinsEarnedPerEnemyKilled
        {
            get
            {
                int value = Constants.BaseCoinsEarnedPerEnemyKilled;
                return value;
            }
        }
        
        public float FreezeEffectTime
        {
            get
            {
                float value = Constants.BaseFreezeEffectTime;
                return value;
            }
        }
        
        public float PoisonEffectTime
        {
            get
            {
                float value = Constants.BasePoisonEffectTime;
                return value;
            }
        }
        
        public float BurnEffectTime
        {
            get
            {
                float value = Constants.BaseBurnEffectTime;
                return value;
            }
        }
        
        
        public float FreezeSpeedDebuffPercentage
        {
            get
            {
                float value = Constants.BaseFreezeSpeedDebuffPercentage;
                return value;
            }
        }
        
        
        public float PoisonActivateTimer
        {
            get
            {
                float value = Constants.BasePoisonActivateTimer;
                return value;
            }
        }
        
        public float BurnAmplifier
        {
            get
            {
                float value = Constants.BaseBurnAmplifier;
                return value;
            }
        }

        public float CriticalAmplifier
        {
            get
            {
                float value = Constants.BaseCriticalAmplifier;
                return value;
            }
        }
        public float SuperCriticalAmplifier
        {
            get
            {
                float value = Constants.BaseSuperCriticalAmplifier;
                return value;
            }
        }
        
        public float BulletSpeed
        {
            get
            {
                float value = Constants.BaseBulletSpeed;
                return value;
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
