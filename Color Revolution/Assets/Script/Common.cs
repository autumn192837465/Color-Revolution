using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using CR.ScriptableObjects;
using UnityEngine;

public class Common : Singleton<Common>
{    

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicate) return;        
    }

    private MLevel seletedMLevel;


    public void SetSelectedMLevel(MLevel mLevel)
    {
        seletedMLevel = mLevel;
    }

    public MLevel GetAndClearSelectedMLevel()
    {
        var data = seletedMLevel;
        seletedMLevel = null;
        return data;
    }
}