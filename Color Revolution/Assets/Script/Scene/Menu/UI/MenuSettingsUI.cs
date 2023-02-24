using Kinopi.Constants;
using Kinopi.Enums;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CR;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class MenuSettingsUI : AnimatorBase
{
    public enum ButtonType
    {        
        Back,
    }

    [Serializable]
    public class ButtonInfo
    {
        public ButtonType Type;
        public Button Button;
    }

    [SerializeField] private List<ButtonInfo> buttonList;
    [SerializeField] private Button closeButton;
    
    [Header("Music")]
    [SerializeField] private Button leftMusicButton;
    [SerializeField] private Button rightMusicButton;
    [SerializeField] private Transform musicVolumeRoot;
    private List<VolumeCell> musicVolumeCellList;
    private int currentMusicVolume;

    [Header("Sound Effect")] 
    [SerializeField] private Button leftSoundEffectButton;
    [SerializeField] private Button rightSoundEffectButton;
    [SerializeField] private Transform soundEffectVolumeRoot;
    private List<VolumeCell> soundEffectvolumeCellList;
    private int currentSoundEffectVolume;

    

        

    public Action<ButtonType> OnClickButton;        

    protected override void Awake()
    {
        base.Awake();
        foreach(ButtonInfo buttonInfo in buttonList)
        {
            buttonInfo.Button.onClick.AddListener(() => OnClickButton?.Invoke(buttonInfo.Type));
        }
        
        leftMusicButton.onClick.AddListener(DecreaseMusicVolume);
        rightMusicButton.onClick.AddListener(IncreaseMusicVolume);

        leftSoundEffectButton.onClick.AddListener(DecreaseSoundEffectVolume);
        rightSoundEffectButton.onClick.AddListener(IncreaseSoundEffectVolume);
        
        closeButton.onClick.AddListener(() =>
        {
            PlayerSettingsManager.Instance.SavePlayerSettings();
            Close();
        });
    }
    
    void Start()
    {
        
    }

    public void InitializeUI()
    {        
        musicVolumeCellList = musicVolumeRoot.GetComponentsInChildren<VolumeCell>().ToList();
        currentMusicVolume = PlayerSettingsManager.Instance.PlayerSettings.MusicVolume;
        for (int i = 0;i< musicVolumeCellList.Count; i++)
        {
            if (i < currentMusicVolume)
                musicVolumeCellList[i].SetFilledUI();
            else
                musicVolumeCellList[i].SetUnfilledUI();            
            musicVolumeCellList[i].InitializeUI(i + 1);
            musicVolumeCellList[i].OnClick = SetMusicVolume;
        }
        //SoundManager.Instance.SetMusicVolume(currentMusicVolume);

        soundEffectvolumeCellList = soundEffectVolumeRoot.GetComponentsInChildren<VolumeCell>().ToList();
        currentSoundEffectVolume = PlayerSettingsManager.Instance.PlayerSettings.SoundEffectVolume;
        for (int i = 0;i< soundEffectvolumeCellList.Count; i++)
        {
            if (i < currentSoundEffectVolume)
                soundEffectvolumeCellList[i].SetFilledUI();
            else
                soundEffectvolumeCellList[i].SetUnfilledUI();            
            soundEffectvolumeCellList[i].InitializeUI(i + 1);
            soundEffectvolumeCellList[i].OnClick = SetSoundEffectVolume;
        }
        //SoundManager.Instance.SetSoundEffectVolume(currentSoundEffectVolume);// ????
        
            
    }

    #region Music Volume
    private void IncreaseMusicVolume()
    {        
        if (currentMusicVolume + 1 > Constants.MaxVolume) return;
        musicVolumeCellList[currentMusicVolume].SetFilledUI();
        musicVolumeCellList[currentMusicVolume].PlayIncreaseVolumeFeedbacks();
        currentMusicVolume++;
        PlayerSettingsManager.Instance.PlayerSettings.MusicVolume = currentMusicVolume;
        //SoundManager.Instance.SetMusicVolume(currentMusicVolume);
    }
    private void DecreaseMusicVolume()
    {
        if (currentMusicVolume - 1 < 0) return;
        currentMusicVolume--;
        PlayerSettingsManager.Instance.PlayerSettings.MusicVolume = currentMusicVolume;
        musicVolumeCellList[currentMusicVolume].SetUnfilledUI();
        musicVolumeCellList[currentMusicVolume].PlayDecreaseVolumeFeedbacks();
        //SoundManager.Instance.SetMusicVolume(currentMusicVolume);
    }

    private void SetMusicVolume(int volume)
    {
        currentMusicVolume = volume;
        for (int i = 0; i < musicVolumeCellList.Count; i++)
        {
            if (i < currentMusicVolume)
            {
                musicVolumeCellList[i].SetFilledUI();
            }
            else
            {
                musicVolumeCellList[i].SetUnfilledUI();
            }
        }
        
        PlayerSettingsManager.Instance.PlayerSettings.MusicVolume = currentMusicVolume;
        //SoundManager.Instance.SetMusicVolume(currentMusicVolume);
        //SoundManager.Instance.PlaySE(SoundManager.SoundEffect.VolumeCellClick);
    }
    #endregion

    #region Sound Effect Volume
    private void IncreaseSoundEffectVolume()
    {        
        if (currentSoundEffectVolume + 1 > Constants.MaxVolume) return;
        soundEffectvolumeCellList[currentSoundEffectVolume].SetFilledUI();
        soundEffectvolumeCellList[currentSoundEffectVolume].PlayIncreaseVolumeFeedbacks();
        currentSoundEffectVolume++;
        PlayerSettingsManager.Instance.PlayerSettings.SoundEffectVolume = currentSoundEffectVolume;
        //SoundManager.Instance.SetSoundEffectVolume(currentSoundEffectVolume);
    }
    private void DecreaseSoundEffectVolume()
    {
        if (currentSoundEffectVolume - 1 < 0) return;
        currentSoundEffectVolume--;
        PlayerSettingsManager.Instance.PlayerSettings.SoundEffectVolume = currentSoundEffectVolume;
        soundEffectvolumeCellList[currentSoundEffectVolume].SetUnfilledUI();
        soundEffectvolumeCellList[currentSoundEffectVolume].PlayDecreaseVolumeFeedbacks();
        //SoundManager.Instance.SetSoundEffectVolume(currentSoundEffectVolume);
    }

    private void SetSoundEffectVolume(int volume)
    {
        currentSoundEffectVolume = volume;
        for (int i = 0; i < soundEffectvolumeCellList.Count; i++)
        {
            if (i < currentSoundEffectVolume)
            {
                soundEffectvolumeCellList[i].SetFilledUI();
            }
            else
            {
                soundEffectvolumeCellList[i].SetUnfilledUI();
            }
        }
        PlayerSettingsManager.Instance.PlayerSettings.SoundEffectVolume = currentSoundEffectVolume;
        //SoundManager.Instance.SetSoundEffectVolume(currentSoundEffectVolume);
        //SoundManager.Instance.PlaySE(SoundManager.SoundEffect.VolumeCellClick);
    }
    #endregion
    
    

}