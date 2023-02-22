using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CR.ScriptableObjects;
using TMPro;

public class LevelNodeUI : MonoBehaviour
{
    [SerializeField] private LevelDataScriptableObject levelData;
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private FeedbackButton button;

    public LevelDataScriptableObject MLevelData => levelData;
    public Action<LevelNodeUI> OnClickLevelNode;
    //public ResearchType ResearchType => researchType;
    
    private void Awake()
    {
        levelNameText.text = levelData.LevelName;
        button.OnClick = () => OnClickLevelNode?.Invoke(this);
    }

    public void InitializeUI()
    {
        
    }   
}