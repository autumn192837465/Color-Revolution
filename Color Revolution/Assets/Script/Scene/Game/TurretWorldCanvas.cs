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

    public void Initialize(TurretData data)
    {
        attackRangeImage.localScale = new Vector2(data.AttackRange * 2, data.AttackRange * 2);
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