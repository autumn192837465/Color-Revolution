using UnityEngine;
using MoreMountains.Feedbacks;

public class IconWithValueTextUI : IconWithTextUI
{
    [SerializeField] private MMF_Player increaseFeedbacks;
    [SerializeField] private MMF_Player decreaseFeedbacks;
    private int currentValue;
    

    protected void PlayIncreaseFeedbacks()
    {
        if(decreaseFeedbacks.IsPlaying) decreaseFeedbacks.StopFeedbacks();
        increaseFeedbacks.PlayFeedbacks();
    }
    
    protected void PlayDecreaseFeedbacks()
    {
        if(increaseFeedbacks.IsPlaying) increaseFeedbacks.StopFeedbacks();
        decreaseFeedbacks.PlayFeedbacks();
    }

    public override void SetText(int value)
    {
        if (value > currentValue)
        {
            currentValue = value;
            PlayIncreaseFeedbacks();
        }
        else if(value < currentValue)
        {
            currentValue = value;
            PlayDecreaseFeedbacks();
        }
        else
        {
            return;
        }
        base.SetText(value);
    }
}