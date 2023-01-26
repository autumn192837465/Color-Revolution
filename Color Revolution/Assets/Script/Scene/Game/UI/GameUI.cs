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
        SkipPreparing,
        DrawCard,
    }

   
    
    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }

    [SerializeField] private List<ButtonInfo> buttonList;
    public Action<ButtonType> OnClickButton;

    public TurretData SelectingTurretData { get; private set; }

    [SerializeField] private IconWithTextUI hpIcon;
    [SerializeField] private IconWithTextUI coinIcon;
    
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
        RefreshCoin();
        RefreshHp();
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
        SelectingTurretData = turretButton.TurretData;
        OnSelectTurret?.Invoke();    
    }

    public Action OnCancelSelection;
    private void CancelSelection()
    {
        selectingTurretImage.SetActive(false);
        SelectingTurretData = null;
        OnCancelSelection?.Invoke();
    }

    public void RefreshCoin()
    {
        coinIcon.SetText(GameManager.Instance.PlayerCoin);
        RefreshTurretButtonCostColor();
    }
    
    public void RefreshHp()
    {
        hpIcon.SetText(GameManager.Instance.PlayerHp);
    }

    private void RefreshTurretButtonCostColor()
    {
        turretButtonList.ForEach(x => x.RefreshCostTextColor());
    }

    #region Card

    [SerializeField] private Transform cardRoot;
    [SerializeField] private CardUI cardUIPrefab;
    private List<CardUI> cardList = new();
    public void DrawCards()
    {
        for (int i = cardList.Count; i < 2; i++)
        {
            var card = Instantiate(cardUIPrefab, cardRoot);
            cardList.Add(card);
            
        }

        var drawnCardTypes = GameManager.Instance.PlayerCards.GetRandomElements(2);
        for (int i = 0; i < cardList.Count; i++)
        {
            var card = cardList[i];
            if (i >= drawnCardTypes.Count)
            {
                card.SetActive(false);
                continue;
            }
            
            card.SetActive(true);
            card.InitializeUI(DataManager.Instance.GetCardData(drawnCardTypes[i]));
        }
    }

    #endregion
    
    
    
}