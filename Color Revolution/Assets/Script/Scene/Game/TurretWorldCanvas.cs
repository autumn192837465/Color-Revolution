using System.Collections;
using System.Collections.Generic;
using CB.Model;
using Kinopi.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class TurretWorldCanvas : ObjectWorldCanvas
{

    [SerializeField] private RectTransform attackRangeImage;

    public bool IsShowingTurretAttackRange => attackRangeImage.ActiveSelf();
    
    void Start()
    {
            
    }
    
    void Update()
    {
        
    }

    public void Initialize(MOffensiveTurret basicData)
    {
        attackRangeImage.localScale = new Vector2(basicData.AttackRange * 2, basicData.AttackRange * 2);
    }

    public void ShowAttackRange()
    {
        attackRangeImage.SetActive(true);
    }
    
    public void HideAttackRange()
    {
        attackRangeImage.SetActive(false);
    }
}