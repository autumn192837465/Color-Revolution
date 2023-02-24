using System;
using Kinopi.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CR;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SoundManager : Singleton<SoundManager>
{
    private const float SoundEffectCooldownTime = 0.2f;
    
    
    [Serializable]
    public struct BgmClip
    {
        public BackgroundMusic bgm;
        public AudioClip clip;
    }
    
    [Serializable]
    public struct SoundEffectClip
    {
        public SoundEffect soundEffect;
        public AudioClip clip;
    }
    [Serializable]
    public struct LoopingSoundEffectAudioSource
    {
        public LoopingSoundEffect soundEffect;
        public AudioSource audioSource;
    }
    
    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioSource seAudioSource;
    
    [SerializeField] private BgmClip[] bgmClips;
    [SerializeField] private SoundEffectClip[] soundEffectClips;
    [SerializeField] private LoopingSoundEffectAudioSource[] loopingSoundEffectAudioSources;
    private Dictionary<BackgroundMusic, AudioClip> bgmDictionary;
    private Dictionary<SoundEffect, AudioClip> soundEffectDictionary;
    private readonly Dictionary<SoundEffect, float> soundEffectCooldownDictionary = new();
    private Dictionary<LoopingSoundEffect, AudioSource> loopingSoundEffectDictionary;
    private IEnumerator bgmCoroutine;
    


    private float musicVolumeSettings;
    private float soundEffectVolumeSettings;
    private float fadeDelta = 2f;
    public bool IsBgMute => bgAudioSource.mute;
    
    

    public enum BackgroundMusic
    {
        Tutorial,
        Menu,
        Map,
        Shop,
        CoinRush,
        ZombieGame = 10,
        SkeletonGame = 11,
        ElfGame = 12,
        DemonGame = 13,
        
    }
    public enum SoundEffect
    {
        HitFlash,
        DropItem,
        PickUpItem,
        NormalMagic,
        FireMagic,
        IceMagic,
        WindMagic,
        ThunderMagic,
        PoisonMagic,
        GameWin,
        GameLose,
        MasteryShow,
        CollectCoin,
        VolumeCellClick,
    }
    
    public enum LoopingSoundEffect
    {
        FootStep,
    }



    protected override void Awake()
    {
        base.Awake();
        SetMusicVolume(PlayerSettingsManager.Instance.PlayerSettings.MusicVolume);
        SetSoundEffectVolume(PlayerSettingsManager.Instance.PlayerSettings.SoundEffectVolume);
    }

    // Start is called before the first frame update
    void Start()
    {
        //bgAudioSource.mute = seAudioSource.mute = PlayerPrefsManager.Volume == 0;        
        //StartCoroutine(PlayBg());
    }

    private void Update()
    {
        foreach (var type in soundEffectCooldownDictionary.Keys.ToList())
        {
            soundEffectCooldownDictionary[type] -= Time.deltaTime;
            if (soundEffectCooldownDictionary[type] <= 0)
            {
                soundEffectCooldownDictionary.Remove(type);
            }
        }
    }

    private void AddButtonSE(Scene scene, LoadSceneMode mode)
    {        
        foreach(Button btn in FindObjectsOfType<Button>())
        {
            //btn.onClick.AddListener(() => PlaySE(Sound.button));
        }
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        //SceneManager.sceneLoaded += AddButtonSE;
    }

    


    public void PlayBgm(BackgroundMusic bgm)
    {
        bgmDictionary ??= bgmClips.ToDictionary(x => x.bgm, x => x.clip);
        if(bgAudioSource.clip == bgmDictionary[bgm])    return;
        if (bgmCoroutine != null)
        {
            StopCoroutine(bgmCoroutine);
        }
        bgmCoroutine = PlayBgCoroutine(bgm);
        StartCoroutine(bgmCoroutine);
    }

    private IEnumerator PlayBgCoroutine(BackgroundMusic bgm)
    {
        
        // Fade out
        while(bgAudioSource.volume > 0)
        {
            bgAudioSource.volume -= fadeDelta * Time.deltaTime;            
            yield return null;
        }
        bgAudioSource.volume = 0;
        

        bgAudioSource.clip = bgmDictionary[bgm];
        bgAudioSource.Play();
        
        //Fade in
        while(bgAudioSource.volume < musicVolumeSettings)
        {
            float volume = Mathf.Min(bgAudioSource.volume + fadeDelta * Time.deltaTime, musicVolumeSettings);
            bgAudioSource.volume = volume;
            yield return null;
        }
    }

    public void StopBgm()
    {
        if (bgmCoroutine != null)
        {
            StopCoroutine(bgmCoroutine);
        }
        bgmCoroutine = StopBgCoroutine();
        StartCoroutine(bgmCoroutine);
    }
    
    private IEnumerator StopBgCoroutine()
    {
        
        // Fade out
        while(bgAudioSource.volume > 0)
        {
            bgAudioSource.volume -= fadeDelta * Time.deltaTime;            
            yield return null;
        }
        bgAudioSource.volume = 0;
        bgAudioSource.Stop();
    }

    public void PlaySE(SoundEffect se, bool playWithCooldown = false, float coolDownTime = SoundEffectCooldownTime)
    {
        soundEffectDictionary ??= soundEffectClips.ToDictionary(x => x.soundEffect, x => x.clip);
        if (playWithCooldown)
        {
            if (soundEffectCooldownDictionary.ContainsKey(se))
            {
                return;
            }
            soundEffectCooldownDictionary.Add(se, coolDownTime);
        }
        
        seAudioSource.PlayOneShot(soundEffectDictionary[se]);
    }

    public void PlayLoopingSE(LoopingSoundEffect se)
    {
        loopingSoundEffectDictionary ??= loopingSoundEffectAudioSources.ToDictionary(x => x.soundEffect, x => x.audioSource);
        loopingSoundEffectDictionary[se].Play();
    }
    
    
    public void StopLoopingSE(LoopingSoundEffect se)
    {
        loopingSoundEffectDictionary[se].Stop();
    }
    
    public void StopAllLoopingSE()
    {
        if(loopingSoundEffectDictionary is null)    return;
        foreach (var audioSource in loopingSoundEffectDictionary.Values)
        {
            audioSource.Stop();
        }
    }
    
    

    public void SetMusicVolume(int volume)
    {
        musicVolumeSettings = (float)volume / Constants.MaxVolume;
        bgAudioSource.volume = musicVolumeSettings;
    }
    
    public void SetSoundEffectVolume(int volume)
    {
        soundEffectVolumeSettings = (float)volume / Constants.MaxVolume;
        seAudioSource.volume = soundEffectVolumeSettings;
        foreach (var loopingSoundEffectAudioSource in loopingSoundEffectAudioSources)
        {
            loopingSoundEffectAudioSource.audioSource.volume = soundEffectVolumeSettings;
        }
        MMSoundManager.Instance.SetVolumeMaster(soundEffectVolumeSettings);
        MMSoundManager.Instance.SaveSettings();
    }

    public void StopAllSe()
    {
        seAudioSource.Stop();
    }

    public void Mute(bool doMute)
    {
        bgAudioSource.mute = doMute;
        seAudioSource.mute = doMute;
        //PlayerPrefsManager.Instance.SetMute(doMute);
    }
    
}
