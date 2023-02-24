using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR.ScriptableObjects;
using Kinopi.Enums;
using UnityEngine;
using UnityEngine.UI;

public class MenuMainUI : MonoBehaviour
{
    public enum ButtonType
    {
        Settings,
    }

    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }


    [SerializeField] private LevelInformationUI LevelInformationUI;
    [SerializeField] private List<ButtonInfo> buttonList;
    public Action<ButtonType> OnClickButton;
    public Action<MLevel> OnClickChallengeButton;
    [SerializeField] private List<LevelNodeUI> levelNodes;
    
    

    private void Awake()
    {
        foreach(ButtonInfo buttonInfo in buttonList)
        {
            buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
        }

        LevelInformationUI.OnClickChallengeButton = (mLevel) => OnClickChallengeButton?.Invoke(mLevel);
    }
    
    void Start()
    {
        
    }

    public void InitializeUI()
    {
        foreach (var node in levelNodes)
        {
            node.OnClickLevelNode = OnClickLevelNode;
        }
    }


    private void OnClickLevelNode(LevelNodeUI node)
    {
        LevelInformationUI.InitializeUI(node.MLevel);
        LevelInformationUI.Open();
    }


    
    [ContextMenu("Set Level Nodes")] 
    private void SetResearchNodes()
    {
        levelNodes = transform.GetComponentsInChildren<LevelNodeUI>().ToList();
        CheckDuplicate();
    }
        
    [ContextMenu("Check Duplicate")] 
    private void CheckDuplicate()
    {
        HashSet<ResearchType> hashSet = new();
        foreach (var node in levelNodes)
        {
            if (false)//hashSet.Contains(node.))
            {
                //Debug.LogError($"{Enum.GetName(typeof(ResearchType), node.ResearchType)}Duplicated!");
                continue;
            }

            //hashSet.Add(node.ResearchType);
        }
    }
}