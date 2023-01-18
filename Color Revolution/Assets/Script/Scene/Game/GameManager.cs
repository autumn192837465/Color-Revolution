using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CR.Model;
using UnityEngine;

namespace CR.Game
{
    public class GameManager : Singleton<GameManager>
    {
        private List<Node> NodeList;
        public List<Enemy> tempEnemyList;

        [SerializeField] private GameShopUI GameShopUI;
        [SerializeField] private Turret tempRedTurret;
        [SerializeField] private Turret tempBlueTurret;
        [SerializeField] private Turret tempGreenTurret;

        
        private Turret _currentSelectingTurret;
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            NodeList = FindObjectsOfType<Node>().ToList();
            NodeList.ForEach(x => x.OnClickNode = (node) =>
            {
                if(_currentSelectingTurret == null)   return;
                if(node.HasTower)   return;
                var tower = Instantiate(_currentSelectingTurret);
                 node.PlaceTower(tower);
                 _currentSelectingTurret = null;
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
                        _currentSelectingTurret = tempRedTurret;
                        break;
                    case GameShopUI.ButtonType.BlueTower:
                        _currentSelectingTurret = tempBlueTurret;
                        break;
                    case GameShopUI.ButtonType.GreenTower:
                        _currentSelectingTurret = tempGreenTurret;
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
