using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CR;
using Kinopi.Enums;
using UnityEngine;
using UnityEngine.UI;


namespace CR.Menu
{
    public class MenuHeaderUI : MonoBehaviour
    {
        [SerializeField] private IconWithValueTextUI rainbowCoinIcon;

        private void Awake()
        {

        }
    
        void Start()
        {
        
        }

        public void InitializePlayerData()
        {
            rainbowCoinIcon.SetText(PlayerDataManager.Instance.GetUPoint(PointType.RainbowCandy).Count);
        }
    
        
    }    
}
