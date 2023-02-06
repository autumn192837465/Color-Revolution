using System;
using System.Collections;
using System.Collections.Generic;
using CR.Model;
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
            PlayerData = PlayerPrefsManager.PlayerData;
        }
    
        void Start()
        {
        
        }
    
        void Update()
        {
        
        }
        #region AddUIEvent
        #endregion

        #region RemoveUIEvent
        #endregion
    }    
}
