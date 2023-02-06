using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Kinopi.Constants;
using Kinopi.Extensions;
using UnityEngine.EventSystems;

namespace CR.Game
{
    public class GameTurretUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private IconWithTextUI costIcon;
        [SerializeField] private TurretInfoUI infoUI; 

        public Action<GameTurretUI, PointerEventData> OnPointerDownTurret;
        public Action<GameTurretUI, PointerEventData> OnPointerUpTurret;
        public TurretData TurretData { get; private set; }


        public Action<GameTurretUI> OnClickTurret;
        private void Awake()
        {
            button.onClick.AddListener(() => OnClickTurret?.Invoke(this));
        }

        public void InitializeUI(TurretData data)
        {
            TurretData = data;
            image.sprite = data.Sprite;
            costIcon.SetText(data.Cost);
            infoUI.InitializeUI(data);
        }

        public void RefreshCostTextColor()
        {
            bool canBuy = GameManager.Instance.PlayerCoin >= TurretData.Cost; 
            costIcon.SetTextColor(canBuy ? Color.white : Constants.DisableColor);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownTurret?.Invoke(this, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpTurret?.Invoke(this, eventData);
        }

        public void ShowInfo()
        {
            infoUI.SetActive(true);
        }

        public void HideInfo()
        {
            infoUI.SetActive(false);
        } 
    }    
}
