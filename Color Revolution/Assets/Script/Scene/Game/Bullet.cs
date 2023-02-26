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
    public BulletData(MOffensiveTurret mOffensiveTurret)
    {
        Damage = mOffensiveTurret.AttackDamage.Copy();
        HitRate = mOffensiveTurret.HitRate.Copy();
        CriticalRate = mOffensiveTurret.CriticalRate.Copy();
        PoisonRate = mOffensiveTurret.PoisonRate.Copy();
        BurnRate = mOffensiveTurret.BurnRate.Copy();
        FreezeRate = mOffensiveTurret.FreezeRate.Copy();
    }
        
    public RGB Damage;
    public Rate HitRate;
    public Rate CriticalRate;
    public Rate PoisonRate;
    public Rate BurnRate;
    public Rate FreezeRate;
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
        if (targetEnemy == null || targetEnemy.gameObject == null )
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
        
        Destroy(gameObject);
        if(!BulletData.HitRate.HitProbability())    
        {
            // Todo : play miss text
            return;
        }

        bool isCritical = BulletData.CriticalRate.HitProbability();
        float amplifier = 1;
        amplifier = isCritical ? amplifier * Constants.CriticalPercentage : amplifier;
        amplifier = enemy.IsBurning ? amplifier * Constants.BurningPercentage : amplifier;
        
        
        enemy.ReduceHp(BulletData.Damage * amplifier);

        if (BulletData.BurnRate.HitProbability())
        {
            enemy.BurnEnemy();
        }
        
        if (BulletData.FreezeRate.HitProbability())
        {
            enemy.FreezeEnemy();
        }
        
        if (BulletData.PoisonRate.HitProbability())
        {
            enemy.PoisonEnemy();
        }
        
        
        
    }

    private void OnDestroy()
    {
        destroying = true;
    }
}