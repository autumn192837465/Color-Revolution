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
        
        public Enemy enemyPrefab;

        
        public List<Enemy> EnemyList;
        public Camera MainCamera;
        [SerializeField] private GameShopUI GameShopUI;
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private Turret tempRedTurret;
        [SerializeField] private Turret tempBlueTurret;
        [SerializeField] private Turret tempGreenTurret;
        public MapDataScriptableObject tempMapData;
        
        private Turret _currentSelectingTurret;
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            
            
            
        }
    
        void Start()
        {
            Initialize();
        }


        private float time = 5;
        void Update()
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 5;
                Enemy enemy = Instantiate(enemyPrefab, enemyRoot);
                enemy.transform.position = Vector3.up + MapManager.Instance.startNode.transform.position;
                enemy.OnEnemyDeath = (e) =>
                {
                    EnemyList.Remove(enemy);
                };
                EnemyList.Add(enemy);
            }
        }

        private void Initialize()
        {
            AddGameShopUIEvent();
            
            MapManager.Instance.CreateMap(tempMapData);
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
            foreach (var enemy in EnemyList)
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
