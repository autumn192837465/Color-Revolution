using System;
using System.Collections;
using System.Collections.Generic;
using Kinopi.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Button showPlaceableButton;
    
    public enum ButtonType
    {
        SkipPreparing
    }

    [SerializeField] private Image selectingTurretImage;
    
    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }

    [SerializeField] private List<ButtonInfo> buttonList;
    public Action<ButtonType> OnClickButton;    
        

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

    public void InitializeUI()
    {

    }

    public void SetSelectingTurretSprite(Sprite sprite)
    {
        if(sprite is null)  selectingTurretImage.SetActive(false);
        else
        {
            selectingTurretImage.SetActive(true);
            selectingTurretImage.sprite = sprite;
        }
    }
}