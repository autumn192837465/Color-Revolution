using System;
using CB.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CR.Game
{
    public class GameCardUI : CardUI, IPointerDownHandler, IPointerUpHandler
    {
        //[SerializeField] private Draggable draggable;

        /*public Action<GameCardUI, PointerEventData> OnBeginDragCard;
    public Action<GameCardUI, PointerEventData> OnDraggingCard;
    public Action<GameCardUI, PointerEventData> OnEndDragCard;*/

        [SerializeField] private GameObject cover;
        
        
        public Action<GameCardUI, PointerEventData> OnPointerDownCard;
        public Action<GameCardUI, PointerEventData> OnPointerUpCard;


        private void Awake()
        {
            SetCoverActive(false);
        }

        public override void InitializeUI(UCard data)
        {
            base.InitializeUI(data);
            SetCoverActive(false);
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownCard?.Invoke(this, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpCard?.Invoke(this, eventData);
        }

        public void SetCoverActive(bool isActive)
        {
            cover.SetActive(isActive);
        }
    }
}