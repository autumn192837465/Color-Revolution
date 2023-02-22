using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CB.Model;
using CR.Menu;
using Kinopi.Enums;
using Kinopi.Extensions;
using UnityEngine;
using UnityEngine.UI;


namespace CR.Menu
{
    public class MenuResearchUI : MonoBehaviour
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
        [SerializeField] private List<ResearchNodeUI> researchNodes;
        [SerializeField] private ResearchInformationPanelUI researchInformationPanelUI;
        public Action<ButtonType> OnClickButton;
        public Action<MResearch> OnClickResearch;

        private ResearchNodeUI selectingNode;

        private void Awake()
        {
            foreach(ButtonInfo buttonInfo in buttonList)
            {
                buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
            }

            researchInformationPanelUI.OnClickResearch = (mResearch) => OnClickResearch?.Invoke(mResearch);
            researchInformationPanelUI.SetActive(false);
        }

        public void InitializeUI()
        {
            selectingNode = null;
            foreach (var node in researchNodes)
            {
                node.InitializeUI();
                node.OnClickResearchNode = OnClickResearchNode;
            }
        }

        public void RefreshUI()
        {
            foreach (var node in researchNodes)
            {
                node.InitializeUI();
            }

            //researchInformationPanelUI.RefreshUI();
        }

        private void OnClickResearchNode(ResearchNodeUI node)
        {
            if (selectingNode == node)
            {
                selectingNode = null;
                researchInformationPanelUI.SetActive(false);
                return;
            }

            selectingNode = node;
            researchInformationPanelUI.SetActive(true);
            researchInformationPanelUI.InitializeUI(node.ResearchType);
        }
        
        
        [ContextMenu("Set Research Nodes")] 
        private void SetResearchNodes()
        {
            researchNodes = transform.GetComponentsInChildren<ResearchNodeUI>().ToList();
            CheckDuplicate();
        }
        
        [ContextMenu("Check Duplicate")] 
        private void CheckDuplicate()
        {
            HashSet<ResearchType> hashSet = new();
            foreach (var node in researchNodes)
            {
                if (hashSet.Contains(node.ResearchType))
                {
                    Debug.LogError($"{Enum.GetName(typeof(ResearchType), node.ResearchType)}Duplicated!");
                    continue;
                }

                hashSet.Add(node.ResearchType);
            }
        }
    }
}
