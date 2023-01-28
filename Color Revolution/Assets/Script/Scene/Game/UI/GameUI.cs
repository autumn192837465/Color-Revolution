using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using CR.Game;
using Kinopi.Enums;
using Kinopi.Extensions;
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
            SkipPreparing,
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

        [SerializeField] private Button cancelSelectingButton;
        [SerializeField] private Image selectingTurretImage;
        [SerializeField] private List<GameShopTurretButtonUI> turretButtonList;


        private void Awake()
        {
            foreach (ButtonInfo buttonInfo in buttonList)
            {
                buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
            }

            foreach (GameShopTurretButtonUI turretButton in turretButtonList)
            {
                turretButton.OnClickTurret = SelectTurret;
            }

            cancelSelectingButton.onClick.AddListener(CancelSelection);
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
        }

        public void InitializeUI()
        {
            turretButtonList[0].InitializeUI(DataManager.Instance.GetTurretData(TurretType.RedTurret));
            turretButtonList[1].InitializeUI(DataManager.Instance.GetTurretData(TurretType.BlueTurret));
            turretButtonList[2].InitializeUI(DataManager.Instance.GetTurretData(TurretType.GreenTurret));
            RefreshCoin();
            RefreshHp();
        }

        public void SetSelectingTurretSprite(Sprite sprite)
        {
            /*
            if(sprite is null)  selectingTurretImage.SetActive(false);
            else
            {
                selectingTurretImage.SetActive(true);
                selectingTurretImage.sprite = sprite;
            }*/
        }

        public Action OnSelectTurret;

        private void SelectTurret(GameShopTurretButtonUI turretButton)
        {
            // Todo : check cost
            selectingTurretImage.SetActive(true);
            selectingTurretImage.sprite = turretButton.TurretData.Sprite;
            SelectingTurretData = turretButton.TurretData;
            OnSelectTurret?.Invoke();
        }

        public Action OnCancelSelection;

        private void CancelSelection()
        {
            selectingTurretImage.SetActive(false);
            SelectingTurretData = null;
            OnCancelSelection?.Invoke();
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

        private void RefreshTurretButtonCostColor()
        {
            turretButtonList.ForEach(x => x.RefreshCostTextColor());
        }

        #region Card

        [SerializeField] private Transform draggingCard;
        [SerializeField] private Transform cardRoot;
        [SerializeField] private GameCardUI cardUIPrefab;
        public Func<Collider, CardData, bool> OnDropCard;
        private List<GameCardUI> cardList = new();

        public void DrawCards()
        {
            for (int i = cardList.Count; i < 2; i++)
            {
                var card = Instantiate(cardUIPrefab, cardRoot);
                card.OnPointerDownCard = OnPointerDownCard;
                card.OnPointerUpCard = OnPointerUpCard;
                cardList.Add(card);
            }

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
        }

        /*private void OnBeginDragCard(GameCardUI card, PointerEventData pointerEventData)
        {
            card.transform.SetParent(draggingRoot);
            print("Begin Dragging card");
        }

        private void OnDraggingCard(GameCardUI card, PointerEventData pointerEventData)
        {
            print("Dragging card");
            card.transform.position = Input.mousePosition;
        }
        private void OnEndDragCard(GameCardUI card, PointerEventData pointerEventData)
        {
            print("End drag card");
        }*/
        private bool isDraggingCard = false;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                
                if (OnDropCard?.Invoke(hit.collider, card.CardData)?? false)
                {
                    card.SetActive(false);
                }
            }
            else
            {
                print("Not put");
            }

            card.SetCoverActive(false);
        }

        #endregion
    }
}