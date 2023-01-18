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

        public Camera MainCamera;
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
            NodeList.ForEach(x => x.OnClickNode = (selectedNode) =>
            {
                if (selectedNode.HasTurret)
                {
                    foreach (var node in NodeList.Where(node => node != selectedNode && node.HasTurret))
                    {
                        node.PlacingTurret.HideAttackRange();
                    }

                    if(selectedNode.PlacingTurret.IsShowingTurretAttackRange) selectedNode.PlacingTurret.HideAttackRange();
                    else selectedNode.PlacingTurret.ShowAttackRange();
                }
                
                if(_currentSelectingTurret == null)   return;
                if(selectedNode.HasTurret)   return;
                var tower = Instantiate(_currentSelectingTurret);
                 selectedNode.PlaceTower(tower);
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

        public List<Enemy> GetInAttackRangeEnemyList(Turret turret)
        {
            List<Enemy> returnList = new();
            foreach (var enemy in tempEnemyList)
            {
                print(Vector3.Distance(enemy.transform.position, turret.transform.position));
                if (Vector3.Distance(enemy.transform.position, turret.transform.position) <= turret.TurretData.AttackRange)
                {
                    
                    returnList.Add(enemy);
                }
            }
            
            return returnList;
        } 
    }    
}
