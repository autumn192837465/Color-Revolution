using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : Singleton<SceneController>
{
    public Image grayout;
    public AnimationCurve fadeCurve;
    public static string MenuScene = "Menu";
    public static string GameScene = "Game";
    public static string UpgradeScene = "Upgrade";
    public static string MapScene = "Map";
    public static string TreasureHouseScene = "Treasure House";
    public static string LuckyCoinBoxScene = "Lucky Coin Box";
    public static string CoinRushScene = "Coin Rush";
    public static string ShopScene = "Shop";
    public static string AuctionScene = "Auction";
    public static string MasteryScene = "Mastery";
    public static string CardSelectionScene = "Ability Selection";
    public static string AntiqueSelectionScene = "Antique Selection";
    public static string TutorialScene = "Tutorial";
    public static string PatternCardSettingScene = "Pattern Card Setting";


    [HideInInspector]    
    public bool switching;
    private Action onSwitchAction;
    float fadetime = 0.5f;
    public Action OnFadeFinish;

    private bool doFadeIn = true;




    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected override void Awake()
    {
        base.Awake();
        if (isDuplicate) return;    
        /*
        
        if (PlayerPrefsManager.CurrentLanguage == ColorBall.Language.Language.Undefined)
        {
            LoadToLanguageScene(true);
            return;
        }
        if (!PlayerPrefsManager.HasFinishedTutorial)
        {
            LoadToTutorialScene();
            return;
        }
        */        
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (PlayerPrefsManager.Instance.languageSetting == Language.Undefined)
        {
            PlayerPrefsManager.Instance.DefaultSetting();
            LoadToLanguageScene(true);
            return;
        }
            
        if (PlayerPrefsManager.Instance.IsFirstTime())
        {            
            LoadToTutorialScene(true);
            return;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
        onSwitchAction?.Invoke();
        switching = false;
    }    
    public void LoadToMenuScene(bool fadeIn = true, Action onSwitch = null)
    {
        doFadeIn = fadeIn;
        SwitchScene(MenuScene, onSwitch);
    }
    public void LoadToGameScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(GameScene, onSwitch);
    }
    public void LoadToUpgradeScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(UpgradeScene, onSwitch);
    }

    public void LoadToMapScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(MapScene, onSwitch);
    }

    public void LoadToTreasureHouseScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(TreasureHouseScene, onSwitch);
    }

    public void LoadToLuckyCoinBoxScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(LuckyCoinBoxScene, onSwitch);
    }

    public void LoadToCoinRushScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(CoinRushScene, onSwitch);
    }

    public void LoadToShopScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(ShopScene, onSwitch);
    }

    public void LoadToAuctionScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(AuctionScene, onSwitch);
    }
    public void LoadToMasteryScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(MasteryScene, onSwitch);
    }
    public void LoadToCardSelectionScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(CardSelectionScene, onSwitch);
    }

    public void LoadToAntiqueSelectionScene(Action onSwitch = null)
    {
        doFadeIn = true;
        SwitchScene(AntiqueSelectionScene, onSwitch);
    }
    
    public void LoadToTutorialScene(bool fadeIn = true, Action onSwitch = null)
    {
        doFadeIn = fadeIn;
        SwitchScene(TutorialScene, onSwitch);
    }
    
    public void LoadToPatternCardSettingScene(bool fadeIn = true, Action onSwitch = null)
    {
        doFadeIn = fadeIn;
        SwitchScene(PatternCardSettingScene, onSwitch);
    }
    

    public void SwitchScene(string sceneName, Action onSwitch = null)
    {
        if (switching) return;
        switching = true;
        onSwitchAction = null;
        onSwitchAction = onSwitch;
        StartCoroutine(FadeTo(sceneName));        
    }
    public void SwitchSceneImmediately(string sceneName, Action onSwitch = null)
    {
        if (switching) return;
        switching = true;
        onSwitchAction = null;
        onSwitchAction = onSwitch;
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator FadeIn()
    {
        if (doFadeIn)
        {
            grayout.enabled = true;
            float t = fadetime;
            while (t > 0)
            {
                float alpha = fadeCurve.Evaluate(t / fadetime);
                Color c = grayout.color;
                c.a = alpha;
                grayout.color = c;
                t -= Time.deltaTime;
                yield return null;
            }
            
        }

        grayout.enabled = false;
        OnFadeFinish?.Invoke();
    }     
    IEnumerator FadeTo(string sceneName)
    {
        grayout.enabled = true;
        float t = 0;
        while(t < fadetime)
        {
            float alpha = fadeCurve.Evaluate(t / fadetime);
            Color c = grayout.color;
            c.a = alpha;
            grayout.color = c;
            t += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }

    private void OnValidate()
    {
        if (!dontDestroy) dontDestroy = true;
    }
}
