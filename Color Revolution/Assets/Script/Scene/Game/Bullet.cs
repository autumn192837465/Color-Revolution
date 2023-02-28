using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using CR.Game;
using CR.Model;
using Kinopi.Constants;
using Kinopi.Enums;
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
        SuperCriticalRate = mOffensiveTurret.SuperCriticalRate.Copy();
        PoisonRate = mOffensiveTurret.PoisonRate.Copy();
        BurnRate = mOffensiveTurret.BurnRate.Copy();
        FreezeRate = mOffensiveTurret.FreezeRate.Copy();
    }
        
    public RGB Damage;
    public Rate HitRate;
    public Rate CriticalRate;
    public Rate SuperCriticalRate;
    public Rate PoisonRate;
    public Rate BurnRate;
    public Rate FreezeRate;
}

public class Bullet : MonoBehaviour
{
  
    [SerializeField] private MeshRenderer meshRenderer;
    
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
        dir = dir.normalized * GameManager.Instance.BulletSpeed;
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

        CriticalType criticalType = GetCriticalType();
        float amplifier = 1;
        amplifier = criticalType switch
        {
            CriticalType.None => amplifier,
            CriticalType.Critical => GameManager.Instance.CriticalAmplifier.GetAmplifiedValue(amplifier),
            CriticalType.SuperCritical => GameManager.Instance.SuperCriticalAmplifier.GetAmplifiedValue(amplifier),
        };
        amplifier = enemy.IsBurning ? amplifier *  GameManager.Instance.BurnAmplifier.GetAmplifiedValue(amplifier) : amplifier;
        
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

    private CriticalType GetCriticalType()
    {
        if (BulletData.CriticalRate.HitProbability())
        {
            return BulletData.SuperCriticalRate.HitProbability() ? CriticalType.SuperCritical : CriticalType.Critical;
        }

        return CriticalType.None;
    }
    
    private void OnDestroy()
    {
        destroying = true;
    }
}