using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using Kinopi.Extensions;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;


    [SerializeField] private EnemyDataScriptableObject enemyDataScriptableObject;
    private EnemyData enemyData;

    private void Awake()
    {
        enemyData = enemyDataScriptableObject.EnemyData.DeepClone();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        Vector3 dir = new Vector3(1, 0, 0);
        transform.Translate(dir * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Bullet>();
        if(bullet == null) return;

        enemyData.Health.ReduceHealth(bullet.AttackDamage);

        if (IsDead)
        {
            Destroy(gameObject);
            
        }
        
        SetColor();
        Destroy(bullet.gameObject);
    }
    
    

    private void SetColor()
    {
        meshRenderer.material.color = enemyData.Health.GetColor();
    }

    private bool IsDead => enemyData.Health.IsDead;

    public Action<Enemy> OnEnemyDeath;
    public void OnDestroy()
    {
        OnEnemyDeath?.Invoke(this);
    }
}