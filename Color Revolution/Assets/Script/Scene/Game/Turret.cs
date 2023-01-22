using System;
using CB.Model;
using Kinopi.Enums;
using Kinopi.Extensions;
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
        [SerializeField] private TurretBasicDataScriptableObject turretBasicDataScriptableObject;
        [SerializeField] private TurretWorldCanvas worldCanvas;
        public TurretBasicData TurretBasicData => turretBasicData;
        private TurretBasicData turretBasicData;
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
            turretBasicData = turretBasicDataScriptableObject.BasicData.DeepClone();
            worldCanvas.Initialize(turretBasicData);
        }


        void Update()
        {
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
            bullet.Initialize(turretBasicData.AttackDamage);
            bullet.SetDestination(target.gameObject);
        }

        private void AttackIfCan(Enemy target)
        {
            if (timer.bulletTimer < turretBasicData.AttackSpeed)  return;
            timer.bulletTimer -= turretBasicData.AttackSpeed;
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
            var enemyList = GameManager.Instance.GetInAttackRangeEnemyList(this);
            return enemyList.GetRandomElement();
        }

        public void ShowAttackRange()
        {
            worldCanvas.ShowAttackRange();
        }

        public void HideAttackRange()
        {
            worldCanvas.HideAttackRange();
        }
    }
}
