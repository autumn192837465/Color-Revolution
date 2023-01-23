using System;
using System.Collections;
using System.Collections.Generic;
using CR.Game;
using Kinopi.Enums;
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

   
    
    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }

    [SerializeField] private List<ButtonInfo> buttonList;
    public Action<ButtonType> OnClickButton;

    public Turret SelectingTurret { get; private set; }

    [SerializeField] private Button cancelSelectingButton;
    [SerializeField] private Image selectingTurretImage;
    [SerializeField] private List<GameShopTurretButtonUI> turretButtonList;
    
        

    private void Awake()
    {
        foreach(ButtonInfo buttonInfo in buttonList)
        {
            buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
        }

        foreach (GameShopTurretButtonUI turretButton in turretButtonList)
        {
            turretButton.OnClickTurret = SelectTurret;
        }
        cancelSelectingButton.onClick.AddListener(CancelSelection);
    }
    
    void Start()
    {
        
    }

    public void InitializeUI()
    {
        turretButtonList[0].InitializeUI(DataManager.Instance.GetTurretData(TurretType.RedTurret));
        turretButtonList[1].InitializeUI(DataManager.Instance.GetTurretData(TurretType.BlueTurret));
        turretButtonList[2].InitializeUI(DataManager.Instance.GetTurretData(TurretType.GreenTurret));
    }

    public void SetSelectingTurretSprite(Sprite sprite)
    {
        /*
        if(sprite is null)  selectingTurretImage.SetActive(false);
        else
        {
            selectingTurretImage.SetActive(true);
            selectingTurretImage.sprite = sprite;
        }*/
    }

    public Action OnSelectTurret;
    private void SelectTurret(GameShopTurretButtonUI turretButton)
    {
        // Todo : check cost
        selectingTurretImage.SetActive(true);
        selectingTurretImage.sprite = turretButton.TurretData.Sprite;
        SelectingTurret = turretButton.TurretData.Turret;
        OnSelectTurret?.Invoke();    
    }

    public Action OnCancelSelection;
    private void CancelSelection()
    {
        selectingTurretImage.SetActive(false);
        SelectingTurret = null;
        OnCancelSelection?.Invoke();
    }
    
    
    
}