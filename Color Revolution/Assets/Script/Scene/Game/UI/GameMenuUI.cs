using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : AnimatorBase
{
    public enum ButtonType
    {
        Continue,
        Restart,
        EndGame,
        Glossary,
        Close,
    }

    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }

    [SerializeField] private TextMeshProUGUI nameText;
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

    public void InitializeUI(MLevel mLevel)
    {
        nameText.text = mLevel.LevelName;
    }
}