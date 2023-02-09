using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorBase : MonoBehaviour
{

    [SerializeField] private MMF_Player openSoundFeedbacks;
    [SerializeField] private MMF_Player closeSoundFeedbacks;
    Animator animator;
    
    public Action OnOpen;
    public Action OnOpened;
    public Action OnClose;
    public Action OnClosed;
    public bool IsIdle => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    public bool IsOpened => !animator.GetCurrentAnimatorStateInfo(0).IsName("Closed");


    [HideInInspector] public string openTrigger = "open";
    [HideInInspector] public string closeTrigger = "close";

    protected virtual void Awake()
    {
        GetAnimator();
    }

    public virtual void Open(Action action)
    {                
        if (IsOpened)    return;

        OnOpen = action;
        animator.SetTrigger(openTrigger);
        openSoundFeedbacks?.PlayFeedbacks();    
    }
    public virtual void Open()
    {
        if (IsOpened) return;
        
        animator.SetTrigger(openTrigger);
        openSoundFeedbacks?.PlayFeedbacks();
    }
    
    public void OnOpenEvent()
    {
        OnOpen?.Invoke();
    }
    public void OnOpenedEvent()
    {
        OnOpened?.Invoke();
    }
    public void OnCloseEvent()
    {
        OnClose?.Invoke();
    }
    
    public void OnClosedEvent()
    {
        OnClosed?.Invoke();
    }


    
    public virtual void Close()
    {
        if(!IsOpened)    return;
        animator.SetTrigger(closeTrigger);
        if (closeSoundFeedbacks != null) closeSoundFeedbacks.PlayFeedbacks();
    }
    
    public virtual void Close(Action action)
    {        
        if (!IsOpened)   return;

        OnClosed = () =>
        {
            action?.Invoke();
            action = null;
        };
            
        animator.SetTrigger(closeTrigger);
        if (closeSoundFeedbacks != null) closeSoundFeedbacks.PlayFeedbacks();

    }
    
    private void GetAnimator()
    {
        animator = GetComponent<Animator>();
    }
    public void ClearAllAction()
    {
        OnOpen = null;
        OnClose = null;
        OnOpened = null;
        OnClosed = null;
    }

    private void OnValidate()
    {
        if(GetComponent<Animator>() == null)
        {
            gameObject.AddComponent<Animator>();
        }
    }
}
