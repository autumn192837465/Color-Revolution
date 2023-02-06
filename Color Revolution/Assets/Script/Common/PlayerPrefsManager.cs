using System;
using System.Collections;
using System.Collections.Generic;
using CR.Model;
using UnityEngine;
using Kinopi.Enums;

//using LanguagePackage;
namespace CR
{
    public class PlayerPrefsManager : Singleton<PlayerPrefsManager>
{       
    public const string LastLoginTime = "last_login_time";
    public const string LanguageKey = "language";
    public const string firstTime = "first_time";
    public const string MaxDepthKey = "max_depth";
    public const string CoinKey = "coin";
    public const string SensitiveKey = "sensitive";
    public const string VolumeKey = "volume";
    public const string MuteKey = "mute";
    public const string TutorialKey = "tutorial";
    public const string UserSettingsKey = "user_settings";
    public const string PlayerDataKey = "player_data";

    //  public Language languageSetting;

    [Serializable]
    public class FontClass
    {
        public Language language;
        public Font font;
    }

    public List<FontClass> FontList;
    public Font DefaultFont;


    public static Language CurrentLanguage;
    

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicate) return;
        //GetCurrentLanguageSetting();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static bool GetBool(string key, bool _default = false)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            //Debug.LogWarning($"No key named {key}");
        }        
        return PlayerPrefs.GetInt(key, (_default) ? 1 : 0) > 0 ? true : false;
    }
    public static void SetBool(string key, bool _default = false)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            //Debug.LogWarning($"No key named {key}");
        }
        PlayerPrefs.SetInt(key, (_default) ? 1 : 0);
    }
    public static int GetInt(string key, int _default = 0)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            //Debug.LogWarning($"No key named {key}");
        }
        return PlayerPrefs.GetInt(key, _default);
    }
    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public static float GetFloat(string key, float _default = 0)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            //Debug.LogWarning($"No key named {key}");
        }
        return PlayerPrefs.GetFloat(key, _default);
    }
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static string GetString(string key, string _default = "")
    {
        if (!PlayerPrefs.HasKey(key))
        {
            //Debug.LogWarning($"No key named {key}");
        }
        return PlayerPrefs.GetString(key, _default);
    }
    public static void SetString(string key, string value)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            //Debug.LogWarning($"No key named {key}");
        }
        PlayerPrefs.SetString(key, value);
    }



    public static string UserSettings
    {
        get
        {
            return GetString(UserSettingsKey, "");
        }
        set
        {
            SetString(UserSettingsKey, value);
        }
    }
    public static string MarkData
    {
        get
        {
            return GetString(MaxDepthKey, "");
        }
        set
        {
            SetString(MaxDepthKey, value);
        }
    }

    public static int Coin
    {
        get
        {
            return GetInt(CoinKey, 0);
        }
        set
        {
            SetInt(CoinKey, value);
        }
    }

    public static PlayerData PlayerData
    {
        get
        {
            string data = GetString(PlayerDataKey, JsonUtility.ToJson(new PlayerData())); 
            return JsonUtility.FromJson<PlayerData>(data);
        }
        set => SetString(PlayerDataKey, JsonUtility.ToJson(value));
    }
    
    public static bool IsMute
    {
        get
        {
            return GetBool(MuteKey, false);
        }
        set
        {
            SetBool(MuteKey, value);
        }
    }

    public static float Volume
    {
        get
        {
            return GetFloat(VolumeKey, 0.5f);
        }
        set
        {
            SetFloat(VolumeKey, value);
        }
    }

    public static bool HasFinishedTutorial
    {
        get
        {
            return GetBool(TutorialKey, false);
        }
        set
        {
            SetBool(TutorialKey, value);
        }
    }

    


    public static bool CanPurchase(int cost)
    {
        return Coin >= cost;        
    }

    public static void Purchase(string code, int cost, bool isOneTimePurchase = true)
    {
        if (!isOneTimePurchase)
        {
            Coin -= cost;
            return;
        }
        

        if (HasItem(code))
        {
            Debug.LogError("Already owned item");
            return;
        }

        if (cost > Coin)
        {
            Debug.LogError("Invalide purchase");
            return;
        }
        Coin -= cost;
        SetBool(code, true);
    }
    
    public static bool HasItem(string code)
    {
        return GetBool(code);
    }

    

    
    public static void Upgrade(string code)
    {
        SetInt(code, GetInt(code) + 1);
    }
    

    public static int GetItemLevel(string code, int _default)
    {
        return GetInt(code, _default);
    }
    public static void SetItemLevel(string code, int value)
    {
        SetInt(code, value);
    }

    public static void GetCurrentLanguageSetting()
    {
        int lan = GetInt(LanguageKey, 0);
        switch (lan)
        {
            case 0:
                CurrentLanguage = Language.Undefined;
                break;
            case 1:
                CurrentLanguage = Language.Chinese;
                break;
            case 2:
                CurrentLanguage = Language.English;
                break;
            case 3:
                CurrentLanguage = Language.Japanese;
                break;
        }        
    }
    public static void SetCurrentLanguage(Language language)
    {
        switch (language)
        {
            case Language.Chinese:
                CurrentLanguage = Language.Chinese;
                SetInt(LanguageKey, 1);
                break;
            case Language.English:
                CurrentLanguage = Language.English;
                SetInt(LanguageKey, 2);
                break;
            case Language.Japanese:
                CurrentLanguage = Language.Japanese;
                SetInt(LanguageKey, 3);
                break;
            default:
                CurrentLanguage = Language.English;
                SetInt(LanguageKey, 0);
                break;
        }
    }


    public void SetMute(bool doMute)
    {

    }


    public static void DefaultSetting()
    {
        PlayerPrefs.DeleteAll();
        //SetDefaultPlayerPrefs();
    }

    public static void FinishTutorial()
    {
        SetBool(firstTime, false); 
    }
    
    

    #region Debug Setting
    [ContextMenu("Delete all")]
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    [ContextMenu("Delete all but skip tutorial")]
    public void DeleteAllButSkipTutorial()
    {        
        DefaultSetting();
        //SetLanguage(Language.Chinese);
        FinishTutorial();

    }

    
    public static void AddCoins(int amount)
    {
        Coin += amount;
    }
    [ContextMenu("0 coin")]
    public static void To0Coin()
    {
        Coin = 0;
    }

    [ContextMenu("Reset items level")]
    public static void ResetItemLevel()
    {
        
    }
    [ContextMenu("Chinese Language")]
    public static void SetChinese()
    {
        
    }
    [ContextMenu("English Language")]
    public void SetEnglish()
    {
      
    }
    #endregion

}
}

