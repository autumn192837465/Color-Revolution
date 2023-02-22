using System;
using System.Collections;
using System.Collections.Generic;
using CR.ScriptableObjects;
using Kinopi.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInformationUI : AnimatorBase
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
    [SerializeField] private TextMeshProUGUI levelNameText;
    public Action<ButtonType> OnClickButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private FeedbackButton challengeButton;
    
    [Header("Reward")]
    [SerializeField] private Transform rewardRoot;
    [SerializeField] private RewardItemIconUI rewardCellPrefab;
    private List<RewardItemIconUI> rewardCellList = new();
    public Action<LevelDataScriptableObject> OnClickChallengeButton;
    public LevelDataScriptableObject MLevel { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        foreach(ButtonInfo buttonInfo in buttonList)
        {
            buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
        }

        challengeButton.OnClick = () => OnClickChallengeButton?.Invoke(MLevel);
        closeButton.onClick.AddListener(Close);
    }
    
    void Start()
    {
        
    }

    public void InitializeUI(LevelDataScriptableObject mLevel)
    {
        MLevel = mLevel;
        levelNameText.text = mLevel.LevelName;

        var rewardList = mLevel.LevelReward;
        for (int i = rewardCellList.Count; i < rewardList.Count; i++)
        {
            var cell = Instantiate(rewardCellPrefab, rewardRoot);
            cell.InitializeUI(rewardList[i].PointType, rewardList[i].Count);
            rewardCellList.Add(cell);
        }
        
        
        for (int i = 0; i < rewardCellList.Count; i++)
        {
            var cell = rewardCellList[i];
            if (i >= rewardList.Count)
            {
                cell.SetActive(false);
                continue;
            }
            cell.SetActive(true);
            
        }
    }
}