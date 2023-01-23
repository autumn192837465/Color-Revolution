using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace CR.Game
{
    public class GameShopTurretButtonUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;

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
        }   
    }    
}
