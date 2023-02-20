using System;
using System.Linq;
using CB.Model;
using CR.Model;
using Kinopi.Enums;
using Kinopi.Extensions;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CR.Game
{
    public class TurretTimer
    {
        public TurretTimer()
        {
            deltaTime = operatingTimer = cooldownTimer = bulletTimer = 0;
        }
        
        public float deltaTime;
        public float operatingTimer;
        public float cooldownTimer;
        public float bulletTimer;

        public void AddBulletTime()
        {
            bulletTimer += deltaTime;
        }

        
        
        public void AddCooldownTime()
        {
            cooldownTimer += deltaTime;
        }
        
        public void AddOperatingTime()
        {
            operatingTimer += deltaTime;
        }
    }
    
    public class Turret : MonoBehaviour
    {
        public TurretType TurretType;
        [SerializeField] private TurretWorldCanvas worldCanvas;
        [SerializeField] private MMF_Player enhanceFeedbacks;


        [HideInInspector] public TargetPriority TargetPriority;
        public TurretData TurretData { get; private set; }
        public TurretBasicData TurretBasicData => turretBasicData;
        private TurretBasicData turretBasicData;


        private int turretCost;
        public int SellCost => turretCost / 2;


        [SerializeField] private Bullet bulletPrefab;
        public bool IsShowingTurretAttackRange => worldCanvas.IsShowingTurretAttackRange;
        
        
        private readonly TurretTimer timer = new ();


        private TurretState currentState = TurretState.Idle;

        private void Awake()
        {
            Initialize();
        }


        private void Initialize()
        {
            TurretData = DataManager.Instance.GetTurretData(TurretType);
            turretBasicData = TurretData.BasicDataScriptableObject.BasicData.DeepClone();
            worldCanvas.Initialize(turretBasicData);
            turretCost = TurretData.Cost;
        }


        void Update()
        {
            if(GameManager.CurrentState != GameState.SpawningEnemy)    return;
            timer.deltaTime = Time.deltaTime;
            switch (currentState)
            {
                case TurretState.Idle:
                    if (GameManager.Instance.EnemyList != null)
                    {
                        currentState = TurretState.Operating;
                    }
                    break;
                case TurretState.Operating:
                    Enemy enemy = GetRandomEnemy();
                    if (enemy == null)
                    {
                        currentState = TurretState.Idle;
                        break;
                    }
                    timer.AddOperatingTime();
                    timer.AddBulletTime();
                    AttackIfCan(enemy);
                    if (IsOverHeating())
                    {
                        currentState = TurretState.Cooldown;
                    }
                    break;
                case TurretState.Cooldown:
                    timer.AddCooldownTime();
                    if (IsCooldownOver())
                    {
                        if (GetRandomEnemy() != null)
                        {
                            currentState = TurretState.Operating;
                        }
                        else
                        {
                            currentState = TurretState.Idle;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateBullet(Enemy target)
        {
            Bullet bullet =  Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Initialize(new BulletData(turretBasicData));
            bullet.SetDestination(target);
        }

        private void AttackIfCan(Enemy target)
        {
            // Todo : enhance performance
            float time = 1.0f / turretBasicData.BulletPerSecond;
            if (timer.bulletTimer < time)  return;
            timer.bulletTimer -= time;
            CreateBullet(target);
        }

        private bool IsOverHeating()
        {
            if (timer.operatingTimer >= turretBasicData.OperatingTime)
            {
                timer.operatingTimer = 0;
                return true;
            }

            return false;
        }

        private bool IsCooldownOver()
        {
            if (timer.cooldownTimer >= turretBasicData.CooldownTime)
            {
                timer.cooldownTimer = 0;
                return true;
            }

            return false;
        }

        private Enemy GetRandomEnemy()
        {
            var enemyList = GameManager.Instance.GetInAttackRangeEnemyList(this).Where(x => x.CanBeAttacked(turretBasicData.AttackDamage));
            if (!enemyList.Any()) return null;
            switch (TargetPriority)
            {
                case TargetPriority.FirstTarget:
                    // Todo
                    return  enemyList.ToList().GetRandomElement();
                case TargetPriority.MostRedHealth:
                    return enemyList.Aggregate((x, y) => x.CurrentHealth.RedValue > y.CurrentHealth.RedValue ? x : y);
                case TargetPriority.MostGreenHealth:
                    return enemyList.Aggregate((x, y) => x.CurrentHealth.GreenValue > y.CurrentHealth.GreenValue ? x : y);
                case TargetPriority.MostBlueHealth:
                    return enemyList.Aggregate((x, y) => x.CurrentHealth.BlueValue > y.CurrentHealth.BlueValue ? x : y);
                case TargetPriority.Random:
                    return  enemyList.ToList().GetRandomElement();
                default:
                    throw new NotImplementedException();
            }
            
            
            
        }

        public void ShowAttackRange()
        {
            worldCanvas.ShowAttackRange();
        }

        public void HideAttackRange()
        {
            worldCanvas.HideAttackRange();
        }

        public void AddTurretValue(int amount)
        {
            turretCost += amount;
        }
        
        public void AddAttackRange(int amount)
        {
            turretBasicData.AttackRange += amount;
            worldCanvas.Initialize(turretBasicData);
        }

        public void AddRedAttack(int amount)
        {
            turretBasicData.AttackDamage.RedValue += amount;
        }
        public void AddBlueAttack(int amount)
        {
            turretBasicData.AttackDamage.BlueValue += amount;
        }
        public void AddGreenAttack(int amount)
        {
            turretBasicData.AttackDamage.GreenValue += amount;
        }
        
        public void AddAttackSpeed(int amount)
        {
            turretBasicData.BulletPerSecond += amount;
            
        }

        public void PlayEnhanceFeedbacks()
        {
            enhanceFeedbacks.PlayFeedbacks();
        }
        
    }
}
