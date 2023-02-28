using System;
using CB.Model;
using CR.Game;
using CR.Model;
using CR.ScriptableObjects;
using Kinopi.Constants;
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
    public RGB CurrentHealth => enemyData.Health.CurrentHealth;

    public Action<Enemy, bool> OnEnemyDeath;


    public bool IsPoisoning => poisonTimer > 0;
    public bool IsBurning => burnTimer > 0;
    public bool IsFreezeing => freezeTimer > 0;
    
    
    // Timer
    private float freezeTimer;
    private float burnTimer;
    private float poisonTimer;
    private float poisonActivateTimer;
    private bool killByPlayer = false;
    
    //[HideInInspector] public bool HitEndNode;

   

    private void Awake()
    {
        enemyData = enemyDataScriptableObject.EnemyData.DeepClone();
        //HitEndNode = false;
        enemyWorldCanvas.Initialize(enemyData);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(GameManager.CurrentState != GameState.SpawningEnemy)    return;
        MoveEnemy();
        ActivateStatus();
    }

    private void MoveEnemy()
    {
        float moveSpeed = (freezeTimer > 0) ? speed * GameManager.Instance.FreezeSpeedDebuffPercentage.ToPercentage() : speed;
        Vector3 nextPosition = transform.position + movingDirection * (Time.deltaTime * moveSpeed);
        if (Vector3.Dot((destinationNode.transform.position - nextPosition).XZPosition(), movingDirection) < 0)
        {
            // Todo : to node position
            transform.position = nextPosition;
            if (destinationNode == path.EndNode)
            {
                //HitEndNode = true;
                killByPlayer = false;
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

    private void ActivateStatus()
    {
        float deltaTime = Time.deltaTime;
        if (poisonTimer > 0)
        {
            poisonTimer -= deltaTime;
            poisonActivateTimer += deltaTime;
            if (poisonActivateTimer >  GameManager.Instance.PoisonActivateTime)
            {
                ReduceHp(Constants.PoisonDamage);
                poisonActivateTimer -=  GameManager.Instance.PoisonActivateTime;
            }
        }

        if (freezeTimer > 0)
        {
            freezeTimer -= deltaTime;
        }

        if (burnTimer > 0)
        {
            burnTimer -= deltaTime;
        }
    }
    

    public void SetPath(Path path)
    {
        this.path = path;
        nodeIndex = 0;
        transform.localPosition = path.StartNode.transform.position.XZPosition() + offset;
        movingDirection = path.GetDirection(nodeIndex++);
    }



    public void ReduceHp(RGB damage)
    {
        RGB receivedDamage = enemyData.Health.ReduceHealth(damage);
        enemyWorldCanvas.RefreshUI();

        if (receivedDamage.RedValue > 0) FloatingText.CreateText(transform.position, FloatingText.Type.Red, receivedDamage.RedValue);
        if (receivedDamage.GreenValue > 0) FloatingText.CreateText(transform.position, FloatingText.Type.Green, receivedDamage.GreenValue);
        if (receivedDamage.BlueValue > 0) FloatingText.CreateText(transform.position, FloatingText.Type.Blue, receivedDamage.BlueValue);
        
        
        if (IsDead)
        {
            killByPlayer = true;
            Destroy(gameObject);
        }
        SetColor();
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

    #region Status

    public void PoisonEnemy()
    {
        poisonTimer = GameManager.Instance.PoisonEffectTime;
        poisonActivateTimer = 0;
    }

    public void FreezeEnemy()
    {
        freezeTimer = GameManager.Instance.FreezeEffectTime;
    }

    public void BurnEnemy()
    {
        burnTimer = GameManager.Instance.BurnEffectTime;
    }
    

    #endregion

    public void OnDestroy()
    {
        OnEnemyDeath?.Invoke(this, killByPlayer);
    }
    
    
    
    
}