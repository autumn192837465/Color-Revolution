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
    [Serializable]
    public class SpriteSet
    {
        public SpriteRenderer Sprite;
        public Color FinalColor;
    }

    
    [SerializeField] private EnemyWorldCanvas enemyWorldCanvas;
    [SerializeField] private EnemyDataScriptableObject enemyDataScriptableObject;
    
    [Header("Sprite Set")] 
    [SerializeField] private SpriteSet redSpriteSet;
    [SerializeField] private SpriteSet blueSpriteSet;
    [SerializeField] private SpriteSet greenSpriteSet;
    
    
    private Path path;
    private EnemyData enemyData;

    private int nodeIndex = 0;
    private float speed => enemyData.Speed;
    private Vector3 movingDirection;

    private Vector3 offset = new(0, 1, 0);
    private Node destinationNode => path.Nodes[nodeIndex];
    private bool IsDead => enemyData.Health.IsDead;
    private RGB MaxHealth => enemyData.Health.MaxHealth;
    private RGB CurrentHealth => enemyData.Health.CurrentHealth;

    public Action<Enemy> OnEnemyDeath;

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
        if (bullet == null) return;

        enemyData.Health.ReduceHealth(bullet.AttackDamage);
        enemyWorldCanvas.RefreshUI();
        if (IsDead)
        {
            Destroy(gameObject);
        }

        SetColor();
        Destroy(bullet.gameObject);
    }


    public bool CanBeAttacked(RGB rgb) =>(rgb.RedValue > 0 && CurrentHealth.RedValue > 0) ||
                                         (rgb.GreenValue > 0 && CurrentHealth.GreenValue > 0) ||
                                         (rgb.BlueValue > 0 && CurrentHealth.BlueValue > 0); 
    
    private void SetColor()
    {
        if (redSpriteSet.Sprite != null)
        {
            redSpriteSet.Sprite.color = Color.Lerp(Color.white, redSpriteSet.FinalColor,
                (float)(MaxHealth.RedValue - CurrentHealth.RedValue) / MaxHealth.RedValue);
        }
        if (greenSpriteSet.Sprite != null)
        {
            greenSpriteSet.Sprite.color = Color.Lerp(Color.white, greenSpriteSet.FinalColor,
                (float)(MaxHealth.GreenValue - CurrentHealth.GreenValue) / MaxHealth.GreenValue);
        }
        if (blueSpriteSet.Sprite != null)
        {
            blueSpriteSet.Sprite.color = Color.Lerp(Color.white, blueSpriteSet.FinalColor,
                (float)(MaxHealth.BlueValue - CurrentHealth.BlueValue) / MaxHealth.BlueValue);
        }
    }


    public void OnDestroy()
    {
        OnEnemyDeath?.Invoke(this);
    }
    
    
}