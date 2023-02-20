using System;
using System.Collections;
using System.Collections.Generic;
using CB.Model;
using CR.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class GameWinResultUI : AnimatorBase
{
    public enum ButtonType
    {
        Video,
        Restart,
        Menu,
    }

    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }

    [SerializeField] private List<ButtonInfo> buttonList;
    public Action<ButtonType> OnClickButton;


    [SerializeField] private Transform rewardRoot;
    [SerializeField] private RewardItemIconUI rewardCellPrefab;
    private List<RewardItemIconUI> rewardCellList;
    

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

    public void InitializeUI(List<PointTuple> rewardList)
    {
        for (int i = 0; i < rewardList.Count; i++)
        {
            var cell = Instantiate(rewardCellPrefab, rewardRoot);
            cell.InitializeUI(rewardList[i].PointType, rewardList[i].Count);
        }
    }
}