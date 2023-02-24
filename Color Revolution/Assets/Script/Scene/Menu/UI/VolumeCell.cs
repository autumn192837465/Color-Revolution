using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using MoreMountains.Feedbacks;


public class VolumeCell : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private Sprite volumeFilledSprite;
    [SerializeField] private Sprite volumeUnfilledSprite;
    [SerializeField] private MMF_Player increaseVolumeFeedbacks;
    [SerializeField] private MMF_Player decreaseVolumeFeedbacks;

    public Action<int> OnClick;
    private int index;

    private void Awake()
    {
        button.onClick.AddListener(() => OnClick?.Invoke(index));
    }

    public void InitializeUI(int index)
    {
        this.index = index;
    }   

    public void SetFilledUI()
    {
        image.sprite = volumeFilledSprite;
    }

    public void SetUnfilledUI()
    {
        image.sprite = volumeUnfilledSprite;
    }

    public void PlayIncreaseVolumeFeedbacks()
    {
        increaseVolumeFeedbacks.PlayFeedbacks();
    }
    public void PlayDecreaseVolumeFeedbacks()
    {
        decreaseVolumeFeedbacks.PlayFeedbacks();
    }
}