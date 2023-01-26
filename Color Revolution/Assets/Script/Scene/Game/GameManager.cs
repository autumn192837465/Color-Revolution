using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
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
        [SerializeField] private GameUI GameUI;
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private MapCreator MapCreator;
        [SerializeField] private WaveDataScriptableObject tempWaveData;
        
        public MapDataScriptableObject tempMapData;

        public static GameState CurrentState = GameState.Initialize;
        public int PlayerCoin => playerData.Coin;
        public int PlayerHp => playerData.Hp;
        

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

            timer += Time.deltaTime;
            switch (CurrentState)
            {
                case GameState.PlayerPreparing:
                    if (timer >= tempWaveData.WaveInterval)
                    {
                        timer = 0;
                        MapCreator.CalculateAllNearestPath();
                        CurrentState = GameState.SpawnEnemy;
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
                                CurrentState = GameState.PlayerPreparing;
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
                    break;
                case GameState.PlayerPreparing:
                    CurrentState = GameState.PlayerPreparing;
                    break;
                case GameState.SpawnEnemy:
                    CurrentState = GameState.SpawnEnemy;
                    break;
                case GameState.End:
                    CurrentState = GameState.End;
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
                Hp = 1,
                Coin = 1000,
            };
            
            
            GameUI.InitializeUI();
            AddGMapCreatorEvent();
            AddGameUIEvent();
            
            MapCreator.CreateMap(tempMapData);
            
            ToState(GameState.PlayerPreparing);
            
            
        }
        #region AddUIEvent

        private void AddGMapCreatorEvent()
        {
            MapCreator.OnSelectAvailableEmptyNode = OnSelectEmptyNode;
        }
        
        private void AddGameUIEvent()
        {
            GameUI.OnClickButton = (type) =>
            {
                switch (type)
                {
                    case GameUI.ButtonType.SkipPreparing:
                        if (CurrentState != GameState.PlayerPreparing) return;
                        timer = tempWaveData.WaveInterval;
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

        private void OnSelectEmptyNode(Node selectedNode)
        {
            if (GameUI.SelectingTurretData is null) return;
            
            var selectingTurretData = GameUI.SelectingTurretData;
            int cost = selectingTurretData.Cost;
            if(cost > PlayerCoin)   return;
            ReducePlayerCoin(cost);
            var turret = Instantiate(selectingTurretData.Turret);
            selectedNode.PlaceTurret(turret);
            MapCreator.SetNodePlaceable();
            MapCreator.ShowPlaceable();
            
            // Todo : check cost
            //currentSelectingTurret = null;

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
                CurrentState = GameState.End;
                
            }
            GameUI.RefreshHp();
        }
        
        private void ReducePlayerCoin(int amount)
        {
            playerData.Coin -= amount;
            if (playerData.Coin < 0)
            {
                playerData.Coin = 0;
                CurrentState = GameState.End;
                Debug.LogError("Coin is negative!!");
                
            }
            GameUI.RefreshCoin();
        }
        #endregion
       
        
        
    }    
}
