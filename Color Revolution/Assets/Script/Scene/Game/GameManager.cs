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
        [SerializeField] private TextMeshProUGUI logText;
        [SerializeField] private GameWinResultUI GameWinResultUI;
        [SerializeField] private GameLoseResultUI GameLoseResultUI;
        [SerializeField] private AnimatorBase GlossaryUI;
        private MLevel levelData;
        
        

        public static GameState CurrentState = GameState.Initialize;

        #region Properties

        public int PlayerCoin => PlayerGameData.Coin;
        public int PlayerHp => PlayerGameData.Hp;
        public UCard[] PlayerCards => PlayerGameData.CardDeck;

        public Rate FreezeSpeedDebuffPercentage => PlayerGameData.FreezeSpeedDebuffPercentage;
        public float PoisonActivateTime => PlayerGameData.PoisonActivateTime;
        public Amplifier BurnAmplifier => PlayerGameData.BurnAmplifier;

        public float FreezeEffectTime => PlayerGameData.FreezeEffectTime;
        public float PoisonEffectTime => PlayerGameData.PoisonEffectTime;
        public float BurnEffectTime => PlayerGameData.BurnEffectTime;
        
        public Amplifier CriticalAmplifier => PlayerGameData.CriticalAmplifier;
        public Amplifier SuperCriticalAmplifier => PlayerGameData.SuperCriticalAmplifier;
        public float BulletSpeed => PlayerGameData.DrawCost;
        public int DrawCost => PlayerGameData.DrawCost;
        #endregion
        
        
        private Node selectingNode;
        public PlayerGameData PlayerGameData { get; private set; }

        public static float DeltaTime;
        public static int GameSpeed;
        
        
        public static int WaveIndex; 
        public static int MaxWaveCount; 
        private int spawnGroupIndex; 
        private int enemyCountIndex; 
        private float spawnTimer = 0;
        private float resuiltOpeningWaitTimer = Constants.ResultOpeningWaitTime;
        

        private void Start()
        {
            Initialize();
        }

        void Update()
        {
            if(CurrentState == GameState.End)   return;
            //DeltaTime = Time.deltaTime * GameSpeed; 

            switch (CurrentState)
            {
                case GameState.Initialize:
                    break;
                case GameState.PlayerPreparing:
                    break;
                case GameState.SpawningEnemy:
                    if(hasSpawnedAll)   break;
                    //if(WaveIndex >= tempLevelData.WaveSpawnList.Count)   break;
                    spawnTimer += Time.deltaTime;
                    if (spawnTimer >= levelData.GetEnemySpawnGroupInterval(WaveIndex, spawnGroupIndex))
                    {
                        spawnTimer = 0;
                        SpawnEnemy(levelData.GetEnemy(WaveIndex, spawnGroupIndex));

                        if (++enemyCountIndex == levelData.GetSpawnGroupEnemyCount(WaveIndex, spawnGroupIndex))
                        {
                            enemyCountIndex = 0;
                            if (++spawnGroupIndex == levelData.GetEnemySpawnGroupCount(WaveIndex))
                            {
                                WaveIndex++;
                                spawnGroupIndex = 0;
                                hasSpawnedAll = true;
                            }
                        }
                    }
                    break;
                case GameState.ShowWinResult:
                    if (resuiltOpeningWaitTimer > 0)
                    {
                        resuiltOpeningWaitTimer -= Time.deltaTime;
                        if (resuiltOpeningWaitTimer < 0) ShowWinResult();
                    }
                    break;
                case GameState.ShowLoseResult:
                    if (resuiltOpeningWaitTimer > 0)
                    {
                        resuiltOpeningWaitTimer -= Time.deltaTime;
                        if (resuiltOpeningWaitTimer < 0) ShowLoseResult();
                    }
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
                    PlayerPreparingState();
                    logText.text = "PlayerPreparing";
                    break;
                case GameState.SpawningEnemy:
                    CurrentState = GameState.SpawningEnemy;
                    logText.text = "SpawnEnemy";
                    break;
                case GameState.ShowWinResult:
                    CurrentState = GameState.ShowWinResult;
                    logText.text = "ShowWinResult";
                    break;
                case GameState.ShowLoseResult:
                    CurrentState = GameState.ShowLoseResult;
                    logText.text = "ShowLoseResult";
                    break;
                case GameState.End:
                    CurrentState = GameState.End;
                    logText.text = "End";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void PlayerPreparingState()
        {
            int rainbowTurretCount = 0;
            foreach (var node in MapCreator.NodeList)
            {
                if(node.PlacingTurret == null)  continue;
                if (node.PlacingTurret.TurretType != TurretType.Support) continue;
                SupportTurret turret = (SupportTurret)node.PlacingTurret;
                if (turret.SupportTurretData.SupportTurretType == SupportTurretType.RainbowTurret) rainbowTurretCount++;
            }

            if (rainbowTurretCount > 0)
            {
                AddPlayerCoin(rainbowTurretCount * PlayerGameData.RainbowTurretCoinEarnings);
            }
        }

        public LevelDataScriptableObject tempLevelData;
        private void Initialize()
        {
            levelData = Common.Instance.GetAndClearSelectedMLevel() ?? tempLevelData.MLevel;

            PlayerGameData = new PlayerGameData(PlayerDataManager.Instance);

            WaveIndex = 0;
            MaxWaveCount = levelData.MaxWaveCount;
            GameUI.InitializeUI(levelData);
            GameMenuUI.InitializeUI(levelData);
            AddMapCreatorEvent();
            AddGameUIEvent();
            AddGameMenuUIEvent();
            
            MapCreator.CreateMap(levelData.MapData);
            ToState(GameState.PlayerPreparing);
        }

     

        private void ShowWinResult()
        {
            var rewards = levelData.LevelReward; 
            foreach (var reward in rewards)
            {
                PlayerDataManager.Instance.AddUPoint(reward);    
            }
            
            GameWinResultUI.InitializeUI(levelData.LevelReward);
            GameWinResultUI.Open();
            AddGameWinResultUIEvent();
            ToState(GameState.End);
        }
        
        private void ShowLoseResult()
        {
            GameLoseResultUI.InitializeUI();
            GameLoseResultUI.Open();
            AddGameLoseResultUIEvent();
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
                        if(PlayerCoin < DrawCost) return;
                        ReducePlayerCoin(DrawCost);
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

                spawnTimer = levelData.GetEnemySpawnGroupInterval(WaveIndex, spawnGroupIndex);
                MapCreator.CalculateAllNearestPath();
                hasSpawnedAll = false;
                GameUI.RefreshWaveText();
                GameUI.SetReadyButtonActive(false);
                ToState(GameState.SpawningEnemy);
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
                    if (node.PlacingTurret.TurretType != TurretType.Offensive) return false;
                    OffensiveTurret turret = (OffensiveTurret)node.PlacingTurret;
                    switch (cardData.CardType)
                    {
                        case CardType.AddRedAttack:
                            turret.AddRedAttack(1);
                            break;
                        case CardType.AddBlueAttack:
                            turret.AddBlueAttack(1);
                            break;
                        case CardType.AddGreenAttack:
                            turret.AddGreenAttack(1);
                            break;
                        case CardType.AddAttackRange:
                            turret.AddAttackRange(100);
                            break;
                        case CardType.AddAttackSpeed:
                            turret.AddAttackSpeed(1);
                            break;
                        case CardType.AddHitRate:
                            break;
                        case CardType.AddCriticalRate:
                            break;
                        case CardType.AddPoisonRate:
                            break;
                        case CardType.AddBurnRate:
                            break;
                        case CardType.AddFreezeRate:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    
                    node.PlacingTurret.AddTurretValue(cardData.Cost);
                    turret.PlayEnhanceFeedbacks();
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
                        //SceneController.Instance.LoadToMenuScene();
                        break;
                    case GameMenuUI.ButtonType.Glossary:
                        GlossaryUI.Open();
                        break;
                    case GameMenuUI.ButtonType.EndGame:
                        SceneController.Instance.LoadToMenuScene();
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
        
        private void AddGameLoseResultUIEvent()
        {
            GameLoseResultUI.OnClickButton = (type) =>
            {
                switch (type)
                {
                    case GameLoseResultUI.ButtonType.Restart:
                        break;
                    case GameLoseResultUI.ButtonType.Menu:
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
            enemy.OnEnemyDeath = (e, killByPlayer) =>
            {
                EnemyList.Remove(enemy);
                if (!killByPlayer)
                {
                    ReducePlayerHP(1);
                    if(PlayerGameData.Hp <= 0)  return;
                }
                else
                {
                    AddPlayerCoin(PlayerGameData.CoinsEarnedPerEnemyKilled);
                }

                if (EnemyList.Count == 0 && hasSpawnedAll)
                {
                    if (WaveIndex == levelData.MaxWaveCount)
                    {
                        spawnTimer = 0;
                        ToState(GameState.ShowWinResult);    
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
        

        
        public List<Enemy> GetInAttackRangeEnemyList(OffensiveTurret offensiveTurret)
        {
            List<Enemy> returnList = new();
            foreach (var enemy in EnemyList)
            {
                if (Vector3.Distance(enemy.transform.position, offensiveTurret.transform.position) <= offensiveTurret.MOffensiveTurret.AttackRange)
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
                turret.Initialize();
                node.PlaceTurret(turret);
                turret.PlayPlaceFeedbacks();
                MapCreator.SetNodePlaceable();
                MapCreator.ShowPlaceable();
                
                SetSelectingNode(node);
            }
        }

        private void SetSelectingNode(Node node)
        {
            if (selectingNode != null) selectingNode.HideAttackRange();
            selectingNode = node;
            switch (selectingNode.PlacingTurret.TurretType)
            {
                case TurretType.Offensive:
                    selectingNode.ShowAttackRange();
                    GameUI.InitializeTurretPanel((OffensiveTurret)selectingNode.PlacingTurret);
                    break;
                case TurretType.Support:
                    GameUI.InitializeTurretPanel((SupportTurret)selectingNode.PlacingTurret);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
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
            PlayerGameData.Hp += amount;
            GameUI.RefreshHp();
        }
        
        private void AddPlayerCoin(int amount)
        {
            PlayerGameData.Coin += amount;
            GameUI.RefreshCoin();
        }
        
        private void ReducePlayerHP(int amount)
        {
            PlayerGameData.Hp -= amount;
            if (PlayerGameData.Hp <= 0)
            {
                PlayerGameData.Hp = 0;
                ToState(GameState.ShowLoseResult);
            }
            GameUI.RefreshHp();
        }
        
        private void ReducePlayerCoin(int amount)
        {
            PlayerGameData.Coin -= amount;
            if (PlayerGameData.Coin < 0)
            {
                PlayerGameData.Coin = 0;
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
