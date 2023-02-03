using System;
using System.Collections;
using System.Collections.Generic;
using CR.Game;
using Febucci.UI;
using Kinopi.Enums;
using UnityEngine;
using UnityEngine.UI;


public class MenuScroller : MonoBehaviour
{
    [Serializable]
    public class FooterButtonInfo
    {
        public MenuType Type;
        public FeedbackButton Button;
        //public LayoutElement LayoutElement;
    }



    [SerializeField] private Sprite selectingSprite;
    [SerializeField] private Sprite unselectSprite;
    [SerializeField] private List<FooterButtonInfo> footerButtonList;
    private const int MaxIndex = 3;
    private float cellHeight = 1440f;
    public RectTransform Content;
    public float SmoothFactor;
    public Action OnSnapContent;
    private bool snapping = false;
    public Action OpenRaycastBlocker;
    public Action CloseRaycastBlocker;

    
    private void Awake()
    {
        foreach(FooterButtonInfo buttonInfo in footerButtonList)
        {
            buttonInfo.Button.OnClick = () => SelectFooter(buttonInfo.Type);
        }
        SelectFooter(MenuType.Main);
    }


    private void SelectFooter(MenuType type)
    {
        foreach (FooterButtonInfo info in footerButtonList)
        {
            bool isType = info.Type == type;
            //info.LayoutElement.preferredWidth = (isType) ? 300 : 200;
            info.Button.GetComponent<Image>().sprite = (isType) ? selectingSprite : unselectSprite;
            //info.TextAnimator.EnableBehaviorsLocally(isType);
        }
        JumpTo(type);
        
    }

    private void SelectFooterWithoutAction(MenuType type)
    {
        foreach (FooterButtonInfo info in footerButtonList)
        {
            bool isType = info.Type == type;
            //info.LayoutElement.preferredWidth = (isType) ? 300 : 200;
            info.Button.GetComponent<Image>().sprite = (isType) ? selectingSprite : unselectSprite;
            //info.TextAnimator.EnableBehaviorsLocally(isType);
        }       
        JumpImmediatelyTo(type);
    }
    
    
    
    public void JumpTo(MenuType type)
    {
        if (snapping) return;
        switch (type)
        {
            case MenuType.Main:
                StartCoroutine(SnapCoroutine(new Vector2(Content.anchoredPosition.x, cellHeight * 0)));
                break;
            case MenuType.Deck:                
                StartCoroutine(SnapCoroutine(new Vector2(Content.anchoredPosition.x, cellHeight * 1)));
                break;
            case MenuType.Shop:
                StartCoroutine(SnapCoroutine(new Vector2(Content.anchoredPosition.x, cellHeight * 2)));
                break;
        }
    }

    public void JumpImmediatelyTo(MenuType type)
    {        
        switch (type)
        {
            case MenuType.Main:
                Content.anchoredPosition = new Vector2(Content.anchoredPosition.x, -cellHeight * 0);
                break;
            case MenuType.Deck:
                Content.anchoredPosition =  new Vector2(Content.anchoredPosition.x, -cellHeight * 1);
                break;
            case MenuType.Shop:
                Content.anchoredPosition =  new Vector2(Content.anchoredPosition.x, -cellHeight * 2);
                break;
        }
    }


    IEnumerator SnapCoroutine(Vector2 finalPosition)
    {        
        snapping = true;
        OpenRaycastBlocker?.Invoke();
        while(Mathf.Abs(Content.anchoredPosition.y - finalPosition.y) > 0.1f)
        {
            Content.anchoredPosition = Vector2.Lerp(Content.anchoredPosition, finalPosition, SmoothFactor);
            yield return null;
        }
        Content.anchoredPosition = finalPosition;
        snapping = false;
        OnSnapContent?.Invoke();
        CloseRaycastBlocker?.Invoke();
    }
}