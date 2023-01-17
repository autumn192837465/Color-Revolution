using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CR.Game
{
    public class GameManager : Singleton<GameManager>
    {
        private List<Node> NodeList;
        public Enemy tempEnemy;

        [SerializeField] private GameShopUI GameShopUI;
        [SerializeField] private Tower tempRedTower;
        [SerializeField] private Tower tempBlueTower;
        [SerializeField] private Tower tempGreenTower;

        
        private Tower currentSelectingTower;
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            NodeList = FindObjectsOfType<Node>().ToList();
            NodeList.ForEach(x => x.OnClickNode = (node) =>
            {
                if(currentSelectingTower == null)   return;
                if(node.HasTower)   return;
                var tower = Instantiate(currentSelectingTower);
                 node.PlaceTower(tower);
                 currentSelectingTower = null;
            });
            
            
        }
    
        void Start()
        {
            Initialize();
        }
    
        void Update()
        {
        
        }

        private void Initialize()
        {
            AddGameShopUIEvent();
        }
        #region AddUIEvent

        private void AddGameShopUIEvent()
        {
            GameShopUI.OnClickButton = (type) =>
            {
                switch (type)
                {
                    case GameShopUI.ButtonType.RedTower:
                        currentSelectingTower = tempRedTower;
                        break;
                    case GameShopUI.ButtonType.BlueTower:
                        currentSelectingTower = tempBlueTower;
                        break;
                    case GameShopUI.ButtonType.GreemTower:
                        currentSelectingTower = tempGreenTower;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            };
        }
        #endregion

        #region RemoveUIEvent
        #endregion
    }    
}
