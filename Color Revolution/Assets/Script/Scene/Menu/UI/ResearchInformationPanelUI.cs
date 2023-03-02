using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using CB.Model;
using Kinopi.Constants;
using Kinopi.Enums;
using Kinopi.Extensions;
using TMPro;


namespace CR.Menu
{
    public class ResearchInformationPanelUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private FeedbackButton researchButton;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI requiredResearchText;
        public Action<MResearch> OnClickResearch;
        public MResearch ResearchData { get; private set; }

        private void Awake()
        {
            researchButton.OnClick = () =>
            {
                researchButton.SetActive(false);
                OnClickResearch?.Invoke(ResearchData);
            };
        }

        public void InitializeUI(ResearchType type)
        {
            ResearchData = DataManager.Instance.GetResearchData(type);
            icon.sprite = ResearchData.Sprite;
            nameText.text = ResearchData.ResearchName;
            descriptionText.text = ResearchData.Description;
            
            
            if (ResearchData.RequiredResearchType == ResearchType.None)
            {
                requiredResearchText.SetActive(false);    
            }
            else
            {
                var requiredResearchData = DataManager.Instance.GetResearchData(ResearchData.RequiredResearchType);
                requiredResearchText.SetActive(true);
                requiredResearchText.text = $"Required Research : {requiredResearchData.ResearchName}";
            }
            
            
            
            if (!PlayerDataManager.Instance.CanResearch(type))
            {
                requiredResearchText.color = Constants.DisableColor;
                researchButton.SetActive(false);
                return;
            }
            requiredResearchText.color = Constants.EnableColor;
            researchButton.SetActive(true);
            // Todo : make class
            int cost = ResearchData.Cost;
            costText.text = cost.ToString();
            
            costText.color = PlayerDataManager.Instance.GetUPoint(PointType.RainbowCandy).Count >= cost
                ? Constants.EnableColor
                : Constants.DisableColor;
        }

    }    
}
