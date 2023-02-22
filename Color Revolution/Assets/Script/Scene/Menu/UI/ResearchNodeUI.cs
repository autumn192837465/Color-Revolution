using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Kinopi.Enums;
using TMPro;

namespace CR.Menu
{
    public class ResearchNodeUI : MonoBehaviour
    {
        [SerializeField] private ResearchType researchType;
        [SerializeField] private GameObject cover;
        [SerializeField] private FeedbackButton button;
        public ResearchType ResearchType => researchType;
        public Action<ResearchNodeUI> OnClickResearchNode;
        
        private void Awake()
        {
            button.OnClick = () => OnClickResearchNode?.Invoke(this);
        }

        public void InitializeUI()
        {
            cover.SetActive(!PlayerDataManager.Instance.HasResearch(ResearchType));
        }   
        
        
    }    
}
