using System;
using System.Collections;
using System.Collections.Generic;
using CR;
using Kinopi.Enums;
using UnityEngine;
using UnityEngine.UI;

public class MenuHeaderUI : MonoBehaviour
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

    [SerializeField] private IconWithValueTextUI rainbowCoinIcon;

    private void Awake()
    {
        foreach(ButtonInfo buttonInfo in buttonList)
        {
            buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
        }
    }
    
    void Start()
    {
        
    }

    public void InitializePlayerData()
    {
        rainbowCoinIcon.SetText(PlayerDataManager.Instance.GetUPoint(PointType.RainbowCandy).Count);
    }
}