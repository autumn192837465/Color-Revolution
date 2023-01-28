using System.Collections;
using System.Collections.Generic;
using CB.Model;
using Kinopi.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWorldCanvas : ObjectWorldCanvas
{

    [SerializeField] private IconWithTextUI redHpBarImage;
    [SerializeField] private IconWithTextUI blueHpBarImage;
    [SerializeField] private IconWithTextUI greenHpBarImage;


    private EnemyData enemyData;
    
    void Start()
    {
            
    }
    
    void Update()
    {
        
    }

    public void Initialize(EnemyData data)
    {
        enemyData = data;
        RefreshUI();
    }

    public void RefreshUI()
    {
        redHpBarImage.SetText(enemyData.Health.CurrentHealth.RedValue);
        blueHpBarImage.SetText(enemyData.Health.CurrentHealth.BlueValue);
        greenHpBarImage.SetText(enemyData.Health.CurrentHealth.GreenValue);
    }

    
}