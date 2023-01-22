using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CR.Model;
using CR.ScriptableObjects;
using Kinopi.Enums;
using Kinopi.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CR.Game
{
    public class GameManager : Singleton<GameManager>
    {
        
        public List<Enemy> EnemyList;
        public Camera MainCamera;
        [SerializeField] private GameShopUI GameShopUI;
        [SerializeField] private GameUI GameUI;
        [SerializeField] private Transform enemyRoot;
        

        [SerializeField] private WaveDataScriptableObject tempWaveData;
        
        public MapDataScriptableObject tempMapData;

        private GameState currentState = GameState.Initialize;
        private Node currentSelectingNode;

        private Turret currentSelectingTurret;
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            
            Initialize();
            
        }
    

        private int waveIndex; 
        private int spawnGroupIndex; 
        private int enemyCountIndex; 
        private float timer = 0;
        
        void Update()
        {
            if(waveIndex >= tempWaveData.WaveSpawnList.Count)   return;

            timer += Time.deltaTime;
            switch (currentState)
            {
                case GameState.PlayerPreparing:
                    if (timer >= tempWaveData.WaveInterval)
                    {
                        timer = 0;
                        MapManager.Instance.CalculateAllNearestPath();
                        currentState = GameState.SpawnEnemy;
                    }
                    break;
                case GameState.SpawnEnemy:
                    if (timer >= tempWaveData.GetEnemySpawnGroupInterval(waveIndex, spawnGroupIndex))
                    {
                        timer = 0;
                        SpawnEnemy(tempWaveData.GetEnemy(waveIndex, spawnGroupIndex));

                        if (++enemyCountIndex == tempWaveData.GetSpawnGroupEnemyCount(waveIndex, spawnGroupIndex))
                        {
                            enemyCountIndex = 0;
                            if (++spawnGroupIndex == tempWaveData.GetEnemySpawnGroupCount(waveIndex))
                            {
                                waveIndex++;
                                spawnGroupIndex = 0;
                                currentState = GameState.PlayerPreparing;
                            }
                        }
                    }
                    break;
                case GameState.Initialize:
                case GameState.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ToState(GameState state)
        {
            switch (state)
            {
                case GameState.Initialize:
                    currentState = GameState.Initialize;
                    break;
                case GameState.PlayerPreparing:
                    currentState = GameState.PlayerPreparing;
                    break;
                case GameState.SpawnEnemy:
                    currentState = GameState.SpawnEnemy;
                    break;
                case GameState.End:
                    currentState = GameState.End;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        private void Initialize()
        {
            AddGameShopUIEvent();
            AddGameUIEvent();
            var nodeMap =  MapManager.Instance.CreateMap(tempMapData);
            var nodeList = new List<Node>();
            foreach (var node in nodeMap)
            {
                if(node is null)    continue;
                nodeList.Add(node);
            }

            nodeList.ForEach(x => x.OnClickNode = OnSelectNode);
            
            ToState(GameState.PlayerPreparing);
            
            
        }
        #region AddUIEvent

        private void AddGameShopUIEvent()
        {
            GameShopUI.OnClickButton = (type) =>
            {
                ShowPlaceable();
                Sprite sprite = null;
                TurretData data = null;
                switch (type)
                {
                    case GameShopUI.ButtonType.RedTower:
                        data = DataManager.Instance.GetTurretData(TurretType.RedTurret);
                        break;
                    case GameShopUI.ButtonType.BlueTower:
                        data = DataManager.Instance.GetTurretData(TurretType.BlueTurret);
                        break;
                    case GameShopUI.ButtonType.GreenTower:
                        data = DataManager.Instance.GetTurretData(TurretType.GreenTurret);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
                GameUI.SetSelectingTurretSprite(data.Sprite);
                currentSelectingTurret = data.Turret;
            };
        }
        
        private void AddGameUIEvent()
        {
            bool showing = false;
            GameUI.OnClickButton = (type) =>
            {
                switch (type)
                {
                    case GameUI.ButtonType.SkipPreparing:
                        if (currentState != GameState.PlayerPreparing) return;
                        timer = tempWaveData.WaveInterval;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            };
            
            GameUI.showPlaceableButton.onClick.AddListener(() =>
            {
                if (showing)
                {
                    ShowPlaceable();
                }
                else
                {
                    HidePlaceable();
                }

                showing = !showing;
            });   
        }
        
        #endregion

        #region RemoveUIEvent


        
        #endregion

        private void SpawnEnemy(Enemy enemyPrefab)
        {
            Enemy enemy = Instantiate(enemyPrefab, enemyRoot);
            enemy.transform.position = Vector3.up + MapManager.Instance.startNode.transform.position;
            enemy.OnEnemyDeath = (e) =>
            {
                EnemyList.Remove(enemy);
                            
            };;
            enemy.SetPath(MapManager.Instance.AllPaths.GetRandomElement());
            EnemyList.Add(enemy);
        }
        
        private void ShowPlaceable()
        {
            foreach (var node in MapManager.Instance.NodeList)
            {
                if(node != MapManager.Instance.startNode && node != MapManager.Instance.endNode && !node.HasTurret)
                    node.ShowPlaceable();
            }
        }

        private void HidePlaceable()
        {
            foreach (var node in MapManager.Instance.NodeList)
            {
                if(node != MapManager.Instance.startNode && node != MapManager.Instance.endNode && !node.HasTurret)
                    node.HidePlaceable();
            }
        }
        
        public List<Enemy> GetInAttackRangeEnemyList(Turret turret)
        {
            List<Enemy> returnList = new();
            foreach (var enemy in EnemyList)
            {
                if (Vector3.Distance(enemy.transform.position, turret.transform.position) <= turret.TurretBasicData.AttackRange)
                {
                                    
                    returnList.Add(enemy);
                }
            }
            
            return returnList;
        }

        private void OnSelectNode(Node selectedNode)
        {
            if (selectedNode.HasTurret)
            {
                // Todo : Show attack range and detail


                if (currentSelectingNode != null)
                {
                    currentSelectingNode.HideAttackRange();
                }
                
                if(selectedNode.PlacingTurret.IsShowingTurretAttackRange) selectedNode.HideAttackRange();
                else selectedNode.ShowAttackRange();
                
                currentSelectingNode = selectedNode;
            }
            else
            {
                if(currentState != GameState.PlayerPreparing)   return;
                if (currentSelectingTurret is null) return;
                if(!selectedNode.CanPlace)  return;
                    
                HidePlaceable();
                var tower = Instantiate(currentSelectingTurret);
                selectedNode.PlaceTower(tower);
                MapManager.Instance.SetNodePlaceable();

                // Todo : check cost
                //currentSelectingTurret = null;
            }

        }
        
        
        
        
       
        
        
    }    
}
