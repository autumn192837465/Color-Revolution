using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;
using Kinopi.Constants;

namespace CR.Game
{
    public class GameShopTurretButtonUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private IconWithTextUI costIcon;

        
        public TurretData TurretData { get; private set; }


        public Action<GameShopTurretButtonUI> OnClickTurret;
        private void Awake()
        {
            button.onClick.AddListener(() => OnClickTurret?.Invoke(this));
        }

        public void InitializeUI(TurretData data)
        {
            TurretData = data;
            image.sprite = data.Sprite;
            costIcon.SetText(data.Cost);
        }

        public void RefreshCostTextColor()
        {
            bool canBuy = GameManager.Instance.PlayerCoin >= TurretData.Cost; 
            costIcon.SetTextColor(canBuy ? Color.white : Constants.DisableColor);
        }
    }    
}
