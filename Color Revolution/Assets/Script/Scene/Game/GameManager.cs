using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{    

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicate) return;        
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    #region AddUIEvent
    #endregion

    #region RemoveUIEvent
    #endregion
}