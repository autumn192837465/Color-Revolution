using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using UnityEngine;
using UnityEngine.UI;

public class CardUpgradeUI : AnimatorBase
{
    public enum ButtonType
    {
        Close,    
    }

    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }

    [SerializeField] private List<ButtonInfo> buttonList;
    public Action<ButtonType> OnClickButton;    
    

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

    public void InitializeUI(UCard uCard)
    {

    }
}