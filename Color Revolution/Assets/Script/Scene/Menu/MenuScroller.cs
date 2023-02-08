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
    [Range(0f, 1f)] public float SmoothFactor;
    public RectTransform Content;
    
    private const int MaxIndex = 3;
    private float cellHeight = 1440f;
    
    
    public Action OnSnapContent;
    private bool snapping = false;
    public Action OpenRaycastBlocker;
    public Action CloseRaycastBlocker;
    
    private Vector2 finalPosition;
    private IEnumerator snappingCoroutine;
    
    private void Awake()
    {
        foreach(FooterButtonInfo buttonInfo in footerButtonList)
        {
            buttonInfo.Button.OnClick = () => SelectFooter(buttonInfo.Type);
        }
        
        SelectFooterWithoutAction(MenuType.Main);
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
        var anchoredPosition = Content.anchoredPosition;
        
        finalPosition = type switch
        {
            MenuType.Main => new Vector2(anchoredPosition.x, cellHeight * 0),
            MenuType.Deck => new Vector2(anchoredPosition.x, cellHeight * 1),
            MenuType.Research => new Vector2(anchoredPosition.x, cellHeight * 2),
            MenuType.Shop => new Vector2(anchoredPosition.x, cellHeight * 3),
            _ => finalPosition
        };
        
        
        if (!snapping)
        {
            snappingCoroutine = SnapCoroutine();
            StartCoroutine(snappingCoroutine);
        }
    }

    public void JumpImmediatelyTo(MenuType type)
    {
        var anchoredPosition = Content.anchoredPosition;
        anchoredPosition = type switch
        {
            MenuType.Main => new Vector2(anchoredPosition.x, cellHeight * 0),
            MenuType.Deck => new Vector2(anchoredPosition.x, cellHeight * 1),
            MenuType.Research => new Vector2(anchoredPosition.x, cellHeight * 2),
            MenuType.Shop => new Vector2(anchoredPosition.x, cellHeight * 3),
            _ => anchoredPosition
        };
        Content.anchoredPosition = anchoredPosition;
    }


    
    private IEnumerator SnapCoroutine()
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