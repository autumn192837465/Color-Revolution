using System;
using CB.Model;
using Kinopi.Enums;
using UnityEngine;


namespace CR.Menu
{
    public class MenuManager : Singleton<MenuManager>
    {
        [SerializeField] private MenuMainUI MenuMainUI;
        [SerializeField] private MenuDeckUI MenuDeckUI;
        [SerializeField] private CardUpgradeUI CardUpgradeUI;
        [SerializeField] private MenuHeaderUI MenuHeaderUI;
        [SerializeField] private MenuResearchUI MenuResearchUI;
        [SerializeField] private MenuSettingsUI MenuSettingsUI;
        [SerializeField] private PlayerStatusUI PlayerStatusUI;
        
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            
        }
        
        void Start()
        {
            SoundManager.Instance.PlayBgm(SoundManager.BackgroundMusic.Menu);
            MenuHeaderUI.InitializePlayerData();
            MenuResearchUI.InitializeUI();
            MenuMainUI.InitializeUI();
            MenuSettingsUI.InitializeUI();
            PlayerStatusUI.InitializeUI();
            AddMenuMainUIEvent();
            AddMenuDeckUIEvent();
            AddCardUpgradeUIEvent();
            AddMenuResearchUIEvent();
        }
        
        void Update()
        {
            
        }
        #region AddUIEvent

        private void AddMenuMainUIEvent()
        {
            MenuMainUI.OnClickChallengeButton = (mLevel) =>
            {
                Common.Instance.SetSelectedMLevel(mLevel);
                SceneController.Instance.LoadToGameScene();
            };
            MenuMainUI.OnClickButton = (type) =>
            {
                switch (type)
                {
                    case MenuMainUI.ButtonType.Settings:
                        MenuSettingsUI.Open();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            };
        }
    
        private void AddMenuDeckUIEvent()
        {
            MenuDeckUI.OnClickCardUpgrade = (uCard) =>
            {
                CardUpgradeUI.Open();
                CardUpgradeUI.InitializeUI(uCard);
            };
        }
    
        private void AddCardUpgradeUIEvent()
        {
            CardUpgradeUI.OnClickUpgrade = () =>
            {
                PlayerDataManager.Instance.UpgradeCard(CardUpgradeUI.SelectingUCard.CardType);
                CardUpgradeUI.RefreshUI();
                MenuDeckUI.SelectingCardDeckThumbnail.RefreshUI();
                MenuHeaderUI.InitializePlayerData();
            };
        }

        private void AddMenuResearchUIEvent()
        {
            MenuResearchUI.OnClickResearch = (mResearch) =>
            {
                PlayerDataManager.Instance.SubUPoint(new PointTuple(PointType.RainbowCandy, mResearch.Cost));
                PlayerDataManager.Instance.AddResearch(mResearch.ResearchType);
                MenuHeaderUI.InitializePlayerData();
                PlayerStatusUI.InitializeUI();
                MenuResearchUI.RefreshUI();
            };
            MenuResearchUI.OnClickPlayerStatus = () =>
            {
                PlayerStatusUI.Open();
            };
        }

        #endregion
    
        #region RemoveUIEvent
        #endregion
    }
}
