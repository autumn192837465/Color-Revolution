using System;
using System.Collections;
using System.Collections.Generic;
using CR;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : AnimatorBase
{
    public enum ButtonType
    {
        
    }

    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }

    [SerializeField] private List<ButtonInfo> buttonList;
    public Action<ButtonType> OnClickButton;

    [SerializeField] private IconWithTextUI hpText;
    [SerializeField] private IconWithTextUI startCoinText;
    [SerializeField] private IconWithTextUI enemyCoinText;
    [SerializeField] private IconWithTextUI rainbowTurretCoinText;
    [SerializeField] private IconWithTextUI freezeEffectTimeText;
    [SerializeField] private IconWithTextUI poisonEffectTimeText;
    [SerializeField] private IconWithTextUI burnEffectTimeText;
    [SerializeField] private IconWithTextUI freezeSpeedDebuffText;
    [SerializeField] private IconWithTextUI burnAmplifierText;
    [SerializeField] private IconWithTextUI poisonActivateTimeText;
    [SerializeField] private IconWithTextUI bulletSpeedText;
    [SerializeField] private IconWithTextUI criticalDamageText;
    [SerializeField] private IconWithTextUI superCriticalDamageText;
    

    protected override void Awake()
    {
        base.Awake();
        foreach(ButtonInfo buttonInfo in buttonList)
        {
            buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
        }
    }
    
    void Start()
    {
        
    }

    public void InitializeUI()
    {
        PlayerDataManager playerDataManager = PlayerDataManager.Instance;
        hpText.SetText(playerDataManager.Hp);
        startCoinText.SetText(playerDataManager.StartCoin);
        enemyCoinText.SetText(playerDataManager.CoinsEarnedPerEnemyKilled);
        rainbowTurretCoinText.SetText(playerDataManager.CoinsEarnedPerEnemyKilled);
        freezeEffectTimeText.SetText($"{playerDataManager.FreezeEffectTime}s");
        poisonEffectTimeText.SetText($"{playerDataManager.PoisonEffectTime}s");
        burnEffectTimeText.SetText($"{playerDataManager.BurnEffectTime}s");
        freezeSpeedDebuffText.SetText($"-{playerDataManager.FreezeSpeedDebuffPercentage}");
        burnAmplifierText.SetText($"{playerDataManager.BurnAmplifier}");
        poisonActivateTimeText.SetText($"{playerDataManager.PoisonActivateTimer}s");
        bulletSpeedText.SetText(playerDataManager.BulletSpeed.ToString("0.#"));
        criticalDamageText.SetText($"{playerDataManager.CriticalAmplifier}");
        superCriticalDamageText.SetText($"{playerDataManager.SuperCriticalAmplifier}");
        
    }
}