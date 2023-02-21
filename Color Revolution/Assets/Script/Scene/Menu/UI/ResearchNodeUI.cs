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
        public Action<ResearchNodeUI> OnClickResearch;
        
        private void Awake()
        {
            button.OnClick = () => OnClickResearch?.Invoke(this);
        }

        public void InitializeUI()
        {
            cover.SetActive(!PlayerDataManager.Instance.HasResearch(ResearchType));
        }   
        
        
    }    
}
