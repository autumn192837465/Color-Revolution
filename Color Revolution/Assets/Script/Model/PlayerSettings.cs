using System;
using System.Collections.Generic;
using CB.Model;
using Kinopi.Constants;
using Kinopi.Enums;
using UnityEngine;


namespace CR.Model
{
    [Serializable]
    public class PlayerSettings
    {
        public PlayerSettings()
        {
            MusicVolume = Constants.MaxVolume;
            SoundEffectVolume = Constants.MaxVolume;
        }

        public int MusicVolume;
        public int SoundEffectVolume;
        
    }    
    
    
}
