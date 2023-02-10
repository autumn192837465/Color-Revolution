using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using Kinopi.Extensions;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CR.Game
{
    public class GameUI : MonoBehaviour
    {
        public Button showPlaceableButton;

        public enum ButtonType
        {
            DrawCard,
            SellTurret,
            Menu,
        }


        [Serializable]
        public class ButtonInfo
        {
            public ButtonType Type;
            public FeedbackButton Button;
        }

        [SerializeField] private List<ButtonInfo> buttonList;
        public Action<ButtonType> OnClickButton;

        public TurretData SelectingTurretData { get; private set; }

        [SerializeField] private IconWithTextUI waveIcon;
        [SerializeField] private IconWithTextUI hpIcon;
        [SerializeField] private IconWithTextUI coinIcon;
        [SerializeField] private TurretPanelInfoUI turretPanelInfoUI;
    
        
        
        public Action OnSellTurret;

        private void Awake()
        {
            foreach (ButtonInfo buttonInfo in buttonList)
            {
                buttonInfo.Button.OnClick = () => OnClickButton?.Invoke(buttonInfo.Type);
            }

            foreach (GameTurretUI turretButton in turretButtonList)
            {
                turretButton.OnClickTurret = SelectTurret;
            }

            pausePlayButton.OnClick = OnClickPauseButton;
            speedUpButton.OnClick = OnClickSpeedUpButton;
            cancelTurretSelectingButton.onClick.AddListener(CancelTurretSelection);
            readyButton.OnClick = () => OnClickReady?.Invoke();
        }

  

        private void Update()
        {
            if(GameManager.IsPausing)   return;
            if (isDraggingCard)
            {
                draggingCard.transform.position = Input.mousePosition;
            }
            
            if (isDraggingTurret)
            {
                draggingTurret.transform.position = Input.mousePosition;
            }
        }

        public void InitializeUI()
        {
            
            InitializeTurret();   
            CreateCards();
            RefreshWaveText();
            RefreshCoin();
            RefreshHp();
            ClearTurretPanel();
            CancelTurretSelection();
        }

        private void CreateCards()
        {
            for (int i = cardList.Count; i < 2; i++)
            {
                var card = Instantiate(cardUIPrefab, cardRoot);
                card.OnPointerDownCard = OnPointerDownCard;
                card.OnPointerUpCard = OnPointerUpCard;
                cardList.Add(card);
            }
            Canvas.ForceUpdateCanvases();
            cardList.ForEach(x => x.SetActive(false));
        }

        public void RefreshWaveText()
        {
            waveIcon.SetText($"{GameManager.WaveIndex + 1}/{GameManager.MaxWaveCount}");
        }

        public void RefreshCoin()
        {
            coinIcon.SetText(GameManager.Instance.PlayerCoin);
            RefreshTurretButtonCostColor();
        }

        public void RefreshHp()
        {
            hpIcon.SetText(GameManager.Instance.PlayerHp);
        }

        public void InitializeTurretPanel(Turret turret)
        {
            turretPanelInfoUI.SetActive(true);
            turretPanelInfoUI.InitializeUI(turret);
        }
        public void ClearTurretPanel()
        {
            turretPanelInfoUI.SetActive(false);
        }


        #region Game Speed

        [Header("Game Speed")] 
        [SerializeField] private FeedbackButton pausePlayButton;
        [SerializeField] private Image pausePlayButtonImage;

        [SerializeField] private Sprite pauseSprite;
        [SerializeField] private Sprite playSprite;

        [SerializeField] private FeedbackButton speedUpButton;
        [SerializeField] private Image speedUpButtonImage;
        
        public Action OnClickPause;
        //public Action OnClickPlay;
        
        public Action OnResumeGameSpeed;
        public Action OnSpeedUpGame;
        private void OnClickPauseButton()
        {
            //pausePlayButtonImage.color = Color.gray;
            OnClickPause?.Invoke();
        }
        
        private void OnClickSpeedUpButton()
        {
            if (GameManager.IsSpeedUp)
            {
                speedUpButtonImage.color = Color.white;
                OnResumeGameSpeed?.Invoke();
            }
            else
            {
                speedUpButtonImage.color = Color.gray;
                OnSpeedUpGame?.Invoke();
            }
        }
        #endregion

        
        [SerializeField] private FeedbackButton readyButton;
        public Action OnClickReady;
        public void SetReadyButtonActive(bool isActive)
        {
            readyButton.SetActive(isActive);
        }
        
        
        #region Turret
        [SerializeField] private Transform draggingTurret;
        [SerializeField] private Button cancelTurretSelectingButton;
        [SerializeField] private Image selectingTurretImage;
        [SerializeField] private List<GameTurretUI> turretButtonList;
        
        
        
        public Func<Collider, TurretData, bool> OnDropTurret;
        public Action OnSelectTurret;


        private void InitializeTurret()
        {
            turretButtonList[0].InitializeUI(DataManager.Instance.GetTurretData(TurretType.RedTurret));
            turretButtonList[1].InitializeUI(DataManager.Instance.GetTurretData(TurretType.BlueTurret));
            turretButtonList[2].InitializeUI(DataManager.Instance.GetTurretData(TurretType.GreenTurret));
            
            foreach (var gameTurretUI in turretButtonList)
            {
                gameTurretUI.OnPointerDownTurret = OnPointerDownTurret;
                gameTurretUI.OnPointerUpTurret = OnPointerUpTurret;
            }
        }
        
        
        private void SelectTurret(GameTurretUI turret)
        {
            if(GameManager.IsPausing)   return;
            // Todo : check cost
            selectingTurretImage.sprite = turret.TurretData.Sprite;
            SelectingTurretData = turret.TurretData;
            OnSelectTurret?.Invoke();
            selectingTurretImage.transform.parent.SetActive(true);
        }

        public Action OnCancelSelection;

        private void CancelTurretSelection()
        {
            if(GameManager.IsPausing)   return;
            SelectingTurretData = null;
            OnCancelSelection?.Invoke();
            selectingTurretImage.transform.parent.SetActive(false);
        }



        private void RefreshTurretButtonCostColor()
        {
            turretButtonList.ForEach(x => x.RefreshCostTextColor());
        }
        
        private bool isDraggingTurret = false;
        private void OnPointerDownTurret(GameTurretUI turret, PointerEventData pointerEventData)
        {
            if(GameManager.IsPausing)   return;
            if(GameManager.CurrentState != GameState.PlayerPreparing)   return;
            if (GameManager.Instance.PlayerCoin < turret.TurretData.Cost)
            {
                // Todo : play feedbacks
                return;
            }

            isDraggingTurret = true;
            SelectTurret(turret);
            draggingTurret.SetActive(true);
            draggingTurret.GetComponent<Image>().sprite = turret.TurretData.Sprite;
        }

        private void OnPointerUpTurret(GameTurretUI turret, PointerEventData pointerEventData)
        {
            if (!isDraggingTurret) return;
            isDraggingTurret = false;
            draggingTurret.SetActive(false);
            RaycastHit hit;
            
            Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                
                if (OnDropTurret?.Invoke(hit.collider, turret.TurretData)?? false)
                {
                    
                }
            }
            CancelTurretSelection();
        }

        #endregion

        #region Card

        [Header("Card")]
        [SerializeField] private Transform draggingCard;
        [SerializeField] private Transform cardRoot;
        [SerializeField] private GameCardUI cardUIPrefab;
        [SerializeField] private MMF_Player cardRevealFeedbacks;
        private List<GameCardUI> cardList = new();
        private bool isDraggingCard = false;
        public Func<Collider, MCard, bool> OnDropCard;
        
        public void DrawCards()
        {
            var drawnCardTypes = GameManager.Instance.PlayerCards.ToList().GetRandomElements(2);
            for (int i = 0; i < cardList.Count; i++)
            {
                var card = cardList[i];
                if (i >= drawnCardTypes.Count)
                {
                    card.SetActive(false);
                    continue;
                }

                card.SetActive(true);
                card.InitializeUI(drawnCardTypes[i]);
            }
            Canvas.ForceUpdateCanvases();
            cardRevealFeedbacks.PlayFeedbacks();
        }
        
        
        private void OnPointerDownCard(GameCardUI card, PointerEventData pointerEventData)
        {
            if(GameManager.IsPausing)   return;
            if (GameManager.Instance.PlayerCoin < card.MCard.Cost)
            {
                // Todo : play feedbacks
                return;
            }

            isDraggingCard = true;
            draggingCard.SetActive(true);
            card.SetCoverActive(true);
        }

        private void OnPointerUpCard(GameCardUI card, PointerEventData pointerEventData)
        {
            if (!isDraggingCard) return;
            isDraggingCard = false;
            draggingCard.SetActive(false);
            RaycastHit hit;
            Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                
                if (OnDropCard?.Invoke(hit.collider, card.MCard)?? false)
                {
                    card.SetActive(false);
                }
            }

            card.SetCoverActive(false);
        }
        #endregion
    }
}