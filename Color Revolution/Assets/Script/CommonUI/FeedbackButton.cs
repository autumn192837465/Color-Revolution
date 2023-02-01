using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using Febucci.UI;
using MoreMountains.Feedbacks;

public class FeedbackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button button;
    [SerializeField] private TextAnimator textAnimator;
    [SerializeField] private MMF_Player onPointerEnterFeedback;
    [SerializeField] private MMF_Player onPointerExitFeedback;
    [SerializeField] private MMF_Player onClickFeedback;
    [SerializeField] private MMF_Player disableFeedback;
    public bool OnlyPlayFeedbacksWhenIsEnable = true;
    private bool interactable;
    private bool isEntering = false;
    
    
    public bool Interactable
    {
        get => interactable;
        set
        {
            interactable = value;
            //button.interactable = value;
            if(!value) textAnimator?.EnableBehaviorsLocally(false);
            
        }
    }

    public Action OnClick;


    private void Awake()
    {
        interactable = button.interactable;
        button.interactable = true;
        button.onClick.AddListener(() =>
        {
            if (!Interactable)
            {
                disableFeedback?.PlayFeedbacks();
                return;
            }
            
            onPointerEnterFeedback?.StopFeedbacks();
            if(!onPointerExitFeedback.IsPlaying)
                onPointerExitFeedback?.PlayFeedbacks();
            isEntering = false;
            onClickFeedback?.PlayFeedbacks();
            OnClick?.Invoke();
        });
        textAnimator?.EnableBehaviorsLocally(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEntering = true;
        if (OnlyPlayFeedbacksWhenIsEnable && !Interactable) return;
        onPointerExitFeedback?.StopFeedbacks();
        onPointerEnterFeedback?.PlayFeedbacks();        
        textAnimator?.EnableBehaviorsLocally(true);        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isEntering) return;
        isEntering = false;
        if (OnlyPlayFeedbacksWhenIsEnable && !Interactable) return;
        onPointerEnterFeedback?.StopFeedbacks();
        onPointerExitFeedback?.PlayFeedbacks();
        textAnimator?.EnableBehaviorsLocally(false);
    }

    private void OnValidate()
    {
        if(button == null)
            button = GetComponent<Button>();
    }
}