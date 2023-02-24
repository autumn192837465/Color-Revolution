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
    public class PlayerSettingsManager : Singleton<PlayerSettingsManager>
    {
        public PlayerSettings PlayerSettings;
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            LoadPlayerSettings();
        }
        


        
        public void SavePlayerSettings()
        {
            string dataString = JsonUtility.ToJson(PlayerSettings);
            PlayerPrefsManager.PlayerSettingsString = dataString;
        }

        private void LoadPlayerSettings()
        {
            string dataString = PlayerPrefsManager.PlayerSettingsString;
            PlayerSettings = JsonUtility.FromJson<PlayerSettings>(dataString) ?? new PlayerSettings();
        }

        
        
        [ContextMenu("Clear Data")]
        public void ClearData()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsManager.PlayerSettingsKey);
        }
    }    
}
