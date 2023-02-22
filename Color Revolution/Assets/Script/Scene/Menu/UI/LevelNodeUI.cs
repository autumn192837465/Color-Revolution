using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using CR.ScriptableObjects;
using TMPro;

public class LevelNodeUI : MonoBehaviour
{
    [SerializeField] private LevelDataScriptableObject levelDataScriptableObject;
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private FeedbackButton button;

    public MLevel MLevel => levelDataScriptableObject.MLevel;
    public Action<LevelNodeUI> OnClickLevelNode;
    //public ResearchType ResearchType => researchType;
    
    private void Awake()
    {
        levelNameText.text = MLevel.LevelName;
        button.OnClick = () => OnClickLevelNode?.Invoke(this);
    }

    public void InitializeUI()
    {
        
    }   
}