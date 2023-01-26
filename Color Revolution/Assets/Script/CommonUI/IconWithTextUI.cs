using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Febucci.UI;
using TMPro;
using Kinopi.Extensions;
using MoreMountains.Feedbacks;

public class IconWithTextUI : MonoBehaviour
{
    //[SerializeField] protected BasicIconUI icon;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private MMF_Player changeTextFeedbacks;
    private TextAnimator contentTextAnimator;
    
    //public Action<BasicIconUI> OnPointerDownIcon;
    public Action OnPointerUpIcon;


    protected virtual void Awake()
    {
        //icon.OnPointerDownIcon = (cell) => OnPointerDownIcon?.Invoke(cell);
        //icon.OnPointerUpIcon = () => OnPointerUpIcon?.Invoke();

        contentTextAnimator = contentText.GetComponent<TextAnimator>();
        //icon.OnPointerEnterIcon = (cell) => OnPointerEnterIcon?.Invoke(cell);
        //icon.OnPointerExitIcon = () => OnPointerExitIcon?.Invoke();
    }

    public void SetIconActive(bool isActive)
    {
        //icon.SetActive(isActive);
    }


    public void SetIcon(Sprite sprite)
    {
        //icon.SetSprite(sprite);
    }
    public void SetText(string str)
    {
        if (contentTextAnimator != null)
        {
            contentTextAnimator.SetText(str, false);
        }
        else
        {
            contentText.text = str;
        }
        

        if(changeTextFeedbacks != null)
        {
            if (changeTextFeedbacks.IsPlaying)
            {
                changeTextFeedbacks.SkipToTheEnd();
                changeTextFeedbacks.StopFeedbacks();
            }
            changeTextFeedbacks.PlayFeedbacks();
        }
    }
    
    public void SetText(int value)
    {
        SetText(value.ToString());
    }
    
    public void SetTextColor(Color32 color)
    { 
        contentText.color = color;
        
    }

    private void OnValidate()
    {
        if (contentText == null)
        {
            contentText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}