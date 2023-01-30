using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using CB.Model;
using CR.Model;
using CR.ScriptableObjects;
using Kinopi.Constants;
using Kinopi.Enums;
using Kinopi.Extensions;
using TMPro;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace CR.Game
{
    public class GameManager : Singleton<GameManager>
    {
        
        public List<Enemy> EnemyList;
        public Camera MainCamera;
        [SerializeField] private GameUI GameUI;
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private MapCreator MapCreator;
        [SerializeField] private WaveDataScriptableObject tempWaveData;
        [SerializeField] private TextMeshProUGUI logText;
        
        public MapDataScriptableObject tempMapData;

        public static GameState CurrentState = GameState.Initialize;
        public int PlayerCoin => playerData.Coin;
        public int PlayerHp => playerData.Hp;
        public List<CardType> PlayerCards => playerData.CardList;
        private Node selectingNode;
        

        private PlayerGameData playerData;
        
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

            
            switch (CurrentState)
            {
                case GameState.PlayerPreparing:
                    
                    break;
                case GameState.SpawnEnemy:
                    if(hasSpawnedAll)   return;
                    timer += Time.deltaTime;
                    if (timer >= tempWaveData.GetEnemySpawnGroupInterval(waveIndex, spawnGroupIndex))
                    {
                        timer = 0;
                        SpawnEnemy(tempWaveData.GetEnemy(waveIndex, spawnGroupIndex));

                        if (++enemyCountIndex == tempWaveData.GetSpawnGroupEnemyCount(waveIndex, spawnGroupIndex))
                        {
                            enemyCountIndex = 0;
                            if (++spawnGroupIndex == tempWaveData.GetEnemySpawnGroupCount(waveIndex))
                            {
                                print("aa");
                                waveIndex++;
                                spawnGroupIndex = 0;
                                hasSpawnedAll = true;
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
                    CurrentState = GameState.Initialize;
                    logText.text = "Initialize";
                    break;
                case GameState.PlayerPreparing:
                    CurrentState = GameState.PlayerPreparing;
                    logText.text = "PlayerPreparing";
                    break;
                case GameState.SpawnEnemy:
                    CurrentState = GameState.SpawnEnemy;
                    logText.text = "SpawnEnemy";
                    break;
                case GameState.End:
                    CurrentState = GameState.End;
                    logText.text = "End";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        private void Initialize()
        {
            // Todo : create data from common
            playerData = new PlayerGameData()
            {
                Hp = 20,
                Coin = 1000,
                CardList = new List<CardType>()
                {
                    CardType.AddRedAttack,
                    CardType.AddBlueAttack,
                    CardType.AddGreenAttack,
                    CardType.AddAttackRange,
                    CardType.AddAttackSpeed,
                }
            };
            
            
            GameUI.InitializeUI();
            AddMapCreatorEvent();
            AddGameUIEvent();
            
            MapCreator.CreateMap(tempMapData);
            ToState(GameState.PlayerPreparing);
            
            
        }
        #region AddUIEvent

        private void AddMapCreatorEvent()
        {
            MapCreator.OnSelectNode = OnSelectNode;

        }


        private bool hasSpawnedAll;
        private void AddGameUIEvent()
        {
            
            
            GameUI.OnClickButton = (type) =>
            {
                switch (type)
                {
                    case GameUI.ButtonType.Ready:
                        if (CurrentState != GameState.PlayerPreparing) return;
                        timer = 0;
                        MapCreator.CalculateAllNearestPath();
                        hasSpawnedAll = false;
                        ToState(GameState.SpawnEnemy);
                        break;
                    case GameUI.ButtonType.DrawCard:
                        if(PlayerCoin < Constants.DrawCost) return;
                        ReducePlayerCoin(Constants.DrawCost);
                        GameUI.DrawCards();
                        break;
                    case GameUI.ButtonType.SellTurret:
                        if (selectingNode is null)
                        {
                            Debug.LogError("No selecting node!");
                            return;
                        }
                        AddPlayerCoin(selectingNode.PlacingTurret.SellCost);
                        selectingNode.DestroyTurret();
                        DeselectNode();
                        MapCreator.SetNodePlaceable();
                        if(GameUI.SelectingTurretData != null)  MapCreator.ShowPlaceable();
                        else MapCreator.HidePlaceable();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            };

            GameUI.OnSelectTurret = () =>
            {
                MapCreator.ShowPlaceable();
                
            };
            
            GameUI.OnCancelSelection = () =>
            {
                MapCreator.HidePlaceable();
            };
            
            GameUI.OnDropTurret= (collider, turretData) =>
            {
                if (collider.CompareTag("Node"))
                {
                    Node node = collider.GetComponent<Node>();
                    if (node is null || node.HasTurret || !node.CanPlace) return false;

                    OnSelectNode(node);
                    return true;
                }

                return false;
            }; 

            GameUI.OnDropCard = (collider, cardData) =>
            {
                if (collider.CompareTag("Node"))
                {
                    Node node = collider.GetComponent<Node>();
                    if (node is null || !node.HasTurret) return false;
                    switch (cardData.CardType)
                    {
                        case CardType.AddRedAttack:
                            node.PlacingTurret.AddRedAttack(1);
                            break;
                        case CardType.AddBlueAttack:
                            node.PlacingTurret.AddBlueAttack(1);
                            break;
                        case CardType.AddGreenAttack:
                            node.PlacingTurret.AddGreenAttack(1);
                            break;
                        case CardType.AddAttackRange:
                            node.PlacingTurret.AddAttackRange(100);
                            break;
                        case CardType.AddAttackSpeed:
                            node.PlacingTurret.AddAttackSpeed(1);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    node.SetColor(Color.cyan);
                    node.PlacingTurret.AddTurretValue(cardData.Cost);
                    node.PlacingTurret.PlayEnhanceFeedbacks();
                    ReducePlayerCoin(cardData.Cost);
                    SetSelectingNode(node);
                    return true;
                }

                return false;
            };


        }
        
        #endregion

        #region RemoveUIEvent


        
        #endregion

        private void SpawnEnemy(Enemy enemyPrefab)
        {
            Enemy enemy = Instantiate(enemyPrefab, enemyRoot);
            enemy.transform.position = Vector3.up + MapCreator.StartNode.transform.position;
            enemy.OnEnemyDeath = (e) =>
            {
                EnemyList.Remove(enemy);
                if (enemy.HitEndNode)
                {
                    ReducePlayerHP(1);
                }

                if (EnemyList.Count == 0 && hasSpawnedAll)
                {
                    ToState(GameState.PlayerPreparing);
                }
            };;
            enemy.SetPath(MapCreator.AllPaths.GetRandomElement());
            EnemyList.Add(enemy);
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

        private void OnSelectNode(Node node)
        {
            if (node.HasTurret)
            {
                if (node == selectingNode)
                {
                    DeselectNode();
                    return;
                }

                SetSelectingNode(node);
            }
            else
            {
                var selectingTurretData = GameUI.SelectingTurretData; 
                if (selectingTurretData == null)
                {
                    DeselectNode();
                    return;
                }
                
                if (CurrentState != GameState.PlayerPreparing || !node.CanPlace || selectingTurretData.Cost > PlayerCoin)
                {
                    DeselectNode();
                    return;
                }
                
                
                
                
                ReducePlayerCoin(selectingTurretData.Cost);
                var turret = Instantiate(selectingTurretData.Turret);
                node.PlaceTurret(turret);
                MapCreator.SetNodePlaceable();
                MapCreator.ShowPlaceable();
                
                SetSelectingNode(node);
            }
        }

        private void SetSelectingNode(Node node)
        {
            if (selectingNode != null) selectingNode.HideAttackRange();
            selectingNode = node;
            selectingNode.ShowAttackRange();
            GameUI.InitializeTurretPanel(selectingNode.PlacingTurret);
        }
        
        private void DeselectNode()
        {
            if (selectingNode != null) selectingNode.HideAttackRange();
            GameUI.ClearTurretPanel();
            selectingNode = null;
        }

        #region Player Status
        private void AddPlayerHP(int amount)
        {
            playerData.Hp += amount;
            GameUI.RefreshHp();
        }
        
        private void AddPlayerCoin(int amount)
        {
            playerData.Coin += amount;
            GameUI.RefreshCoin();
        }
        
        private void ReducePlayerHP(int amount)
        {
            playerData.Hp -= amount;
            if (playerData.Hp <= 0)
            {
                playerData.Hp = 0;
                ToState(GameState.End);
                
            }
            GameUI.RefreshHp();
        }
        
        private void ReducePlayerCoin(int amount)
        {
            playerData.Coin -= amount;
            if (playerData.Coin < 0)
            {
                playerData.Coin = 0;
                Debug.LogError("Coin is negative!!");
                
            }
            GameUI.RefreshCoin();
        }
        #endregion
       
        
        
    }    
}

/*
1. 選空白的node 
    正在選擇Turret => 放置turret 
    沒有正在選擇Turret => 甚麼都沒發生 SelectingNode = null
2. 選有turret的node 
    正在選擇中 => 取消選擇
    不是正在選擇中 => 取消攻擊範圍, 並秀出資訊
    */
