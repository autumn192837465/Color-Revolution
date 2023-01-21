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
        [SerializeField] private TextMeshProUGUI timeText;
        private List<Node> NodeList;
        
        public List<Enemy> EnemyList;
        public Camera MainCamera;
        [SerializeField] private GameShopUI GameShopUI;
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private Turret tempRedTurret;
        [SerializeField] private Turret tempBlueTurret;
        [SerializeField] private Turret tempGreenTurret;

        [SerializeField] private WaveDataScriptableObject tempWaveData;
        
        public MapDataScriptableObject tempMapData;

        private GameState currentState = GameState.WaveInterval;
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

        private int waveIndex; 
        private int spawnGroupIndex; 
        private int enemyCountIndex; 
        private float time = 0;

        private float t = 0;
        void Update()
        {
            t += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(t);
            timeText.text = $"{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
            
            
            if(waveIndex >= tempWaveData.WaveSpawnList.Count)   return;

            time += Time.deltaTime;
            switch (currentState)
            {
                case GameState.WaveInterval:
                    if (time >= tempWaveData.WaveInterval)
                    {
                        currentState = GameState.SpawnEnemy;
                        waveIndex = 0;
                        spawnGroupIndex = 0;
                        time = 0;
                    }
                    break;
                case GameState.SpawnEnemy:
                    if (time >= tempWaveData.WaveSpawnList[waveIndex].EnemySpawnGroupList[spawnGroupIndex].interval)
                    {
                        time = 0;
                        Enemy enemy = Instantiate(tempWaveData.GetEnemy(waveIndex, spawnGroupIndex), enemyRoot);
                        enemy.transform.position = Vector3.up + MapManager.Instance.startNode.transform.position;
                        enemy.OnEnemyDeath = (e) =>
                        {
                            EnemyList.Remove(enemy);
                        };
                        enemy.SetPath(MapManager.Instance.AllPaths.GetRandomElement());
                        EnemyList.Add(enemy);

                        if (++enemyCountIndex == tempWaveData.WaveSpawnList[waveIndex].EnemySpawnGroupList[spawnGroupIndex].count)
                        {
                            enemyCountIndex = 0;
                            if (++spawnGroupIndex == tempWaveData.WaveSpawnList[waveIndex].EnemySpawnGroupList.Count)
                            {
                                spawnGroupIndex = 0;
                                waveIndex++;
                                currentState = GameState.WaveInterval;
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                if (Vector3.Distance(enemy.transform.position, turret.transform.position) <= turret.TurretData.AttackRange)
                {
                                    
                    returnList.Add(enemy);
                }
            }
            
            return returnList;
        } 
    }    
}
