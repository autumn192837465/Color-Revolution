using System;
using System.Collections.Generic;
using CB.Model;
using CR.ScriptableObjects;
using Kinopi.Constants;
using Kinopi.Enums;
using Kinopi.Extensions;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CR.Game
{
    public class GameManager : Singleton<GameManager>
    {
        
        public List<Enemy> EnemyList;
        public Camera MainCamera;
        [Header("Layout UI")]
        [SerializeField] private GameUI GameUI;
        [SerializeField] private GameMenuUI GameMenuUI;
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private MapCreator MapCreator;
        [SerializeField] private LevelDataScriptableObject tempLevelData;
        [SerializeField] private TextMeshProUGUI logText;
        [SerializeField] private Transform floatingTextRoot;
        [SerializeField] private GameWinResultUI GameWinResultUI;
        
        public MapDataScriptableObject tempMapData;

        public static GameState CurrentState = GameState.Initialize;
        public int PlayerCoin => playerData.Coin;
        public int PlayerHp => playerData.Hp;
        public UCard[] PlayerCards => playerData.CardDeck;
        private Node selectingNode;
        

        
        private PlayerGameData playerData;
        
        protected override void Awake()
        {
            base.Awake();
            if (isDuplicate) return;
            
            Initialize();
            
        }

        public static float DeltaTime;
        public static int GameSpeed;
        
        
        public static int WaveIndex; 
        public static int MaxWaveCount; 
        private int spawnGroupIndex; 
        private int enemyCountIndex; 
        private float timer = 0;
        
        
        void Update()
        {
            if(CurrentState == GameState.End)   return;
            DeltaTime = Time.deltaTime * GameSpeed; 
            

            
            switch (CurrentState)
            {
                case GameState.Initialize:
                    break;
                case GameState.PlayerPreparing:
                    break;
                case GameState.SpawnEnemy:
                    if(hasSpawnedAll)   break;
                    //if(WaveIndex >= tempLevelData.WaveSpawnList.Count)   break;
                    timer += Time.deltaTime;
                    if (timer >= tempLevelData.GetEnemySpawnGroupInterval(WaveIndex, spawnGroupIndex))
                    {
                        timer = 0;
                        SpawnEnemy(tempLevelData.GetEnemy(WaveIndex, spawnGroupIndex));

                        if (++enemyCountIndex == tempLevelData.GetSpawnGroupEnemyCount(WaveIndex, spawnGroupIndex))
                        {
                            enemyCountIndex = 0;
                            if (++spawnGroupIndex == tempLevelData.GetEnemySpawnGroupCount(WaveIndex))
                            {
                                WaveIndex++;
                                spawnGroupIndex = 0;
                                hasSpawnedAll = true;
                            }
                        }
                    }
                    break;
                case GameState.ShowResult:
                    ShowResult();
                    break;
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
                case GameState.ShowResult:
                    CurrentState = GameState.ShowResult;
                    logText.text = "ShowResult";
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
                CardDeck = PlayerDataManager.Instance.PlayerData.CardDeck,
            };


            WaveIndex = 0;
            MaxWaveCount = tempLevelData.MaxWaveCount;
            GameUI.InitializeUI();
            AddMapCreatorEvent();
            AddGameUIEvent();
            AddGameMenuUIEvent();
            
            MapCreator.CreateMap(tempMapData);
            ToState(GameState.PlayerPreparing);
        }

        private void ShowResult()
        {
            GameWinResultUI.InitializeUI(tempLevelData.LevelReward);
            GameWinResultUI.Open();
            AddGameWinResultUIEvent();
            ToState(GameState.End);
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
                if(IsPausing)   return;
                switch (type)
                {
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

            GameUI.OnClickReady = () =>
            {
                if (CurrentState != GameState.PlayerPreparing) return;

                timer = 0;
                MapCreator.CalculateAllNearestPath();
                hasSpawnedAll = false;
                GameUI.RefreshWaveText();
                GameUI.SetReadyButtonActive(false);
                ToState(GameState.SpawnEnemy);
            };
            //GameUI.OnClickPlay = PlayGame;
            GameUI.OnClickPause = PauseGame;
            GameUI.OnResumeGameSpeed = ResumeGameSpeed;
            GameUI.OnSpeedUpGame = SpeedUpGame;
            
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
                    
                    
                    node.PlacingTurret.AddTurretValue(cardData.Cost);
                    node.PlacingTurret.PlayEnhanceFeedbacks();
                    ReducePlayerCoin(cardData.Cost);
                    SetSelectingNode(node);
                    return true;
                }

                return false;
            };


        }

        private void AddGameMenuUIEvent()
        {
            GameMenuUI.OnClickButton = (type) =>
            {
                switch (type)
                {
                    case GameMenuUI.ButtonType.Continue:
                    case GameMenuUI.ButtonType.Close:
                        ResumeGame();
                        break;
                    case GameMenuUI.ButtonType.Restart:
                        break;
                    case GameMenuUI.ButtonType.EndGame:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            };
        }

        private void AddGameWinResultUIEvent()
        {
            GameWinResultUI.OnClickButton = (type) =>
            {
                switch (type)
                {
                    case GameWinResultUI.ButtonType.Video:
                        break;
                    case GameWinResultUI.ButtonType.Restart:
                        break;
                    case GameWinResultUI.ButtonType.Menu:
                        SceneController.Instance.LoadToMenuScene();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
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
                    if (WaveIndex == tempLevelData.MaxWaveCount)
                    {
                        ToState(GameState.ShowResult);    
                    }
                    else
                    {
                        GameUI.SetReadyButtonActive(true);
                        ToState(GameState.PlayerPreparing);                        
                    }
                    
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
                ToState(GameState.ShowResult);
                
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


        #region GameSpeed

        public static bool IsPausing { get;private set; }
        public static bool IsSpeedUp { get;private set; }
        private float currentGameSpeed = 1;
        
        private void ResumeGame()
        {
            IsPausing = false;
            Time.timeScale = currentGameSpeed;
            GameMenuUI.Close();
        }

        private void PauseGame()
        {
            IsPausing = true;
            Time.timeScale = 0;
            GameMenuUI.Open();
        }

        private void SpeedUpGame()
        {
            IsSpeedUp = true;
            currentGameSpeed = 2;
            if(!IsPausing)
                Time.timeScale = currentGameSpeed;
        }

        private void ResumeGameSpeed()
        {
            IsSpeedUp = false;
            currentGameSpeed = 1;
            if(!IsPausing)
                Time.timeScale = currentGameSpeed;
        }

        #endregion

        
    }    
}
