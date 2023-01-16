using System;
using CB.Model;
using Kinopi.Enums;
using Kinopi.Extensions;
using UnityEngine;

namespace CR.Game
{
    public class TowerTimer
    {
        public TowerTimer()
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
            bulletTimer += deltaTime;
        }
        
        public void AddOperatingTime()
        {
            bulletTimer += deltaTime;
        }
    }
    
    public class Tower : MonoBehaviour
    {

        [SerializeField] private TowerDataScriptableObject towerDataScriptableObject;
        private TowerData towerData;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Enemy enemy;
        
        private readonly TowerTimer timer = new ();


        private TowerState currentState = TowerState.Idle;

        private void Awake()
        {
            towerData = towerDataScriptableObject.TowerData.DeepClone();
            Initialize();
        }


        private void Initialize()
        {
            
        }

            
        
    
        void Update()
        {
            timer.deltaTime = Time.deltaTime;
            switch (currentState)
            {
                case TowerState.Idle:
                    if (enemy != null)
                    {
                        currentState = TowerState.Operating;
                    }
                    break;
                case TowerState.Operating:
                    if (enemy == null)
                    {
                        currentState = TowerState.Idle;
                        break;
                    }
                    timer.AddOperatingTime();
                    timer.AddBulletTime();
                    AttackIfCan();
                    if (IsOverHeating())
                    {
                        currentState = TowerState.Cooldown;
                    }
                    break;
                case TowerState.Cooldown:
                    timer.AddCooldownTime();
                    if (IsCooldownOver())
                    {
                        if (enemy != null)
                        {
                            currentState = TowerState.Operating;
                        }
                        else
                        {
                            currentState = TowerState.Idle;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateBullet()
        {
            Bullet bullet =  Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Initialize(towerData.AttackDamage);
            //bullet.SetColor();
            bullet.SetDestination(enemy.gameObject);
        }

     
        
        private void AttackIfCan()
        {
            if (timer.bulletTimer < towerData.AttackSpeed)  return;
            timer.bulletTimer -= towerData.AttackSpeed;
            CreateBullet();
        }

        private bool IsOverHeating()
        {
            if (timer.operatingTimer >= towerData.OperatingTime)
            {
                timer.operatingTimer = 0;
                return true;
            }

            return false;
        }

        private bool IsCooldownOver()
        {
            if (timer.cooldownTimer >= towerData.CooldownTime)
            {
                timer.cooldownTimer = 0;
                return true;
            }

            return false;
        }
        
    }    
}
