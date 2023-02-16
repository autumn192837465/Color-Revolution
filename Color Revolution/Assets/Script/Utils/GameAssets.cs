using Kinopi.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;

    public static GameAssets Instance
    {
        get
        {
            if (_instance == null) _instance = (Instantiate(Resources.Load("GameAssets")) as GameObject)?.GetComponent<GameAssets>();
            return _instance;
        }
    }
    public FloatingText floatingText;
}
