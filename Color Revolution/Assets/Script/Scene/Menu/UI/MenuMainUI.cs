using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMainUI : MonoBehaviour
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

    public Button tempGoButton;

    private void Awake()
    {
        foreach(ButtonInfo buttonInfo in buttonList)
        {
            buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
        }
        tempGoButton.onClick.AddListener(ToGameScene);
    }
    
    void Start()
    {
        
    }

    public void InitializeUI()
    {

    }

    public void ToGameScene()
    {
        SceneController.Instance.LoadToGameScene();
    }
}