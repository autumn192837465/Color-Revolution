using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinopi.Enums
{

    public enum GameState
    {
        Initialize,
        SpawnEnemy,
        PlayerPreparing,
        End,
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
