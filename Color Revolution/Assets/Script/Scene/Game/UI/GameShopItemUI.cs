using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CR.Game
{
    public abstract class GameShopItemUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action<GameShopItemUI, PointerEventData> OnPointerDownUI;
        public Action<GameShopItemUI, PointerEventData> OnPointerUpUI;
        void Start()
        {
            
        }
    
        void Update()
        {
        
        }    
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownUI?.Invoke(this, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpUI?.Invoke(this, eventData);
        }
    }    
}
