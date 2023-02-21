using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinopi.Enums
{

    public enum GameState
    {
        Initialize,
        SpawningEnemy,
        PlayerPreparing,
        ShowWinResult,
        ShowLoseResult,
        End
        
    }

    public enum TurretState
    {
        Idle,
        Operating,
        Cooldown,
    }

    public enum NodeType
    {
        Empty = 0,
        Normal = 1,
        Start = 2,
        End = 3,
    }

    public enum TurretType
    {
        RedTurret,
        BlueTurret,
        GreenTurret,
    }

    public enum CardType
    {
        AddRedAttack,
        AddBlueAttack,
        AddGreenAttack,
        AddAttackRange,
        AddAttackSpeed,
        AddHitRate,
        AddCriticalRate,
        AddPoisonRate,
        AddBurnRate,
        AddFreezeRate,
        Test1,
        Test2,
        Test3,
        Test4,
        Test5,
        Test6,
        Test7,
    }

    public enum PointType
    {
        RainbowCandy,
    }

    public enum ResearchType
    {
        None = 0,
        Add1Hp_1 = 1,
        Add1Hp_2 = 2,
        Add1Hp_3 = 3,
        Add1Hp_4 = 4,
        Add1Hp_5 = 5,
        
    }

    public enum TargetPriority
    {
        FirstTarget,
        MostRedHealth,
        MostGreenHealth,
        MostBlueHealth,
        Random,
    }

    public enum Status
    {
        Burn,
        Freeze,
        Poison,
        
        
    }

    public enum MenuType
    {
        Main,
        Deck,
        Research,
        Shop,
    }

    public enum Language
    {
        Undefined,
        Chinese,
        English,
        Japanese,
    }

}
