using System;
using CB.Model;
using CR.Game;
using CR.Model;
using CR.ScriptableObjects;
using Kinopi.Enums;
using Kinopi.Extensions;
using UnityEngine;

public class Enemy : UnitBase
{
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private EnemyWorldCanvas enemyWorldCanvas;
    [SerializeField] private EnemyDataScriptableObject enemyDataScriptableObject;
    private Path path;
    private EnemyData enemyData;

    private int nodeIndex = 0;
    private float speed => enemyData.Speed;
    private Vector3 movingDirection;
    
    private Vector3 offset = new Vector3(0, 1, 0);
    private Node destinationNode => path.Nodes[nodeIndex];

    [HideInInspector] public bool HitEndNode;

    private void Awake()
    {
        enemyData = enemyDataScriptableObject.EnemyData.DeepClone();
        HitEndNode = false;
        enemyWorldCanvas.Initialize(enemyData);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        Vector3 nextPosition = transform.position + movingDirection * (Time.deltaTime * speed);
        if (Vector3.Dot((destinationNode.transform.position - nextPosition).XZPosition(), movingDirection) < 0)
        {
            // Todo : to node position
            transform.position = nextPosition;
            if (destinationNode == path.EndNode)
            {
                HitEndNode = true;
                Destroy(gameObject);
            }
            else
            {
                movingDirection = path.GetDirection(nodeIndex++);
            }
        }
        else
        {
            transform.position = nextPosition;
        }
    }

    public void SetPath(Path path)
    {
        this.path = path;
        nodeIndex = 0;
        transform.localPosition = path.StartNode.transform.position.XZPosition() + offset;
        movingDirection = path.GetDirection(nodeIndex++); 
        
    }

    
    
    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Bullet>();
        if(bullet == null) return;

        enemyData.Health.ReduceHealth(bullet.AttackDamage);
        enemyWorldCanvas.RefreshUI();
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