using System;
using System.Collections;
using System.Collections.Generic;
using CR.ScriptableObjects;
using UnityEngine;

public class Common : Singleton<Common>
{    

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicate) return;        
    }

    private LevelDataScriptableObject seletedMLevel;


    public void SetSelectedMLevel(LevelDataScriptableObject mLevel)
    {
        seletedMLevel = mLevel;
    }

    public LevelDataScriptableObject GetAndClearSelectedMLevel()
    {
        var data = seletedMLevel;
        seletedMLevel = null;
        return data;
    }
}