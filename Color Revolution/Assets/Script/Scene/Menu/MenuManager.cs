using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
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