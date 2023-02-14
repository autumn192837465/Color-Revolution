using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using CR.Model;
using Kinopi.Constants;
using Kinopi.Extensions;
using Kinopi.Utils;
using MoreMountains.Tools;
using UnityEngine;

public class BulletData
{
    public BulletData(TurretBasicData turretBasicData)
    {
        Damage = turretBasicData.AttackDamage.Copy();
        PoisonRate = turretBasicData.PoisonRate;
        BurnRate = turretBasicData.BurnRate;
        FreezeRate = turretBasicData.FreezeRate;
    }
        
    public RGB Damage;
    public float PoisonRate;
    public float BurnRate;
    public float FreezeRate;
}

public class Bullet : MonoBehaviour
{
  
    [SerializeField] private MeshRenderer meshRenderer;
    
    private float speed = 5;
    private Enemy targetEnemy;
    public BulletData BulletData { get; private set; }

    void Start()
    {
            
    }

    private bool destroying = false;
    void Update()
    {
        if(destroying)  return;
        if (targetEnemy.gameObject == null )
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = targetEnemy.transform.position - transform.position;
        dir = dir.normalized * speed;
        transform.position += dir * Time.deltaTime;
    }

    public void Initialize(BulletData bulletData)
    {
        BulletData = bulletData;
        meshRenderer.material.color = bulletData.Damage.GetColor();
    }

    public void SetDestination(Enemy enemy)
    {
        targetEnemy = enemy;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("Enemy")) return;
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != targetEnemy)    return;
        
        
        enemy.ReduceHp(enemy.IsBurning ? BulletData.Damage * Constants.BurningPercentage : BulletData.Damage);

        if (Utils.HitProbability(BulletData.BurnRate))
        {
            enemy.BurnEnemy();
        }
        
        if (Utils.HitProbability(BulletData.FreezeRate))
        {
            enemy.BurnEnemy();
        }
        
        if (Utils.HitProbability(BulletData.PoisonRate))
        {
            enemy.PoisonEnemy();
        }
        
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        destroying = true;
    }
}