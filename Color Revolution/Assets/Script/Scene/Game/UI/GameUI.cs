using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using Kinopi.Extensions;
using MoreMountains.Feedbacks;
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
            Ready,
            DrawCard,
        }


        [Serializable]
        public class ButtonInfo
        {
            public ButtonType Type;
            public Button Button;
        }

        [SerializeField] private List<ButtonInfo> buttonList;
        public Action<ButtonType> OnClickButton;

        public TurretData SelectingTurretData { get; private set; }

        [SerializeField] private IconWithTextUI hpIcon;
        [SerializeField] private IconWithTextUI coinIcon;
        


        private void Awake()
        {
            foreach (ButtonInfo buttonInfo in buttonList)
            {
                buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
            }

            foreach (GameTurretUI turretButton in turretButtonList)
            {
                turretButton.OnClickTurret = SelectTurret;
            }

            cancelTurretSelectingButton.onClick.AddListener(CancelSelection);
        }

        void Start()
        {
        }

        private void Update()
        {
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
            RefreshCoin();
            RefreshHp();
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
        
        public void RefreshCoin()
        {
            coinIcon.SetText(GameManager.Instance.PlayerCoin);
            RefreshTurretButtonCostColor();
        }

        public void RefreshHp()
        {
            hpIcon.SetText(GameManager.Instance.PlayerHp);
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
            // Todo : check cost
            selectingTurretImage.SetActive(true);
            selectingTurretImage.sprite = turret.TurretData.Sprite;
            SelectingTurretData = turret.TurretData;
            OnSelectTurret?.Invoke();
        }

        public Action OnCancelSelection;

        private void CancelSelection()
        {
            selectingTurretImage.SetActive(false);
            SelectingTurretData = null;
            OnCancelSelection?.Invoke();
        }



        private void RefreshTurretButtonCostColor()
        {
            turretButtonList.ForEach(x => x.RefreshCostTextColor());
        }
        
        private bool isDraggingTurret = false;
        private void OnPointerDownTurret(GameTurretUI turret, PointerEventData pointerEventData)
        {
            if(GameManager.CurrentState != GameState.PlayerPreparing)   return;
            if (GameManager.Instance.PlayerCoin < turret.TurretData.Cost)
            {
                // Todo : play feedbacks
                return;
            }

            isDraggingTurret = true;
            SelectTurret(turret);
            draggingTurret.SetActive(true);
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
            CancelSelection();
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
        public Func<Collider, CardData, bool> OnDropCard;
        
        public void DrawCards()
        {
            
            var drawnCardTypes = GameManager.Instance.PlayerCards.GetRandomElements(2);
            for (int i = 0; i < cardList.Count; i++)
            {
                var card = cardList[i];
                if (i >= drawnCardTypes.Count)
                {
                    card.SetActive(false);
                    continue;
                }

                card.SetActive(true);
                card.InitializeUI(DataManager.Instance.GetCardData(drawnCardTypes[i]));
            }
            Canvas.ForceUpdateCanvases();
            cardRevealFeedbacks.PlayFeedbacks();
        }
        
        
        private void OnPointerDownCard(GameCardUI card, PointerEventData pointerEventData)
        {
            if (GameManager.Instance.PlayerCoin < card.CardData.Cost)
            {
                // Todo : play feedbacks
                return;
            }

            isDraggingCard = true;
            draggingCard.SetActive(true);
            print("Pointer down");
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
                
                if (OnDropCard?.Invoke(hit.collider, card.CardData)?? false)
                {
                    card.SetActive(false);
                }
            }

            card.SetCoverActive(false);
        }
        #endregion
    }
}