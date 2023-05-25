using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class ADSDK : MonoBehaviour
{
    public GameObject BannerBgObj;
    public GameObject GDPRTipParentObj;
    public GameObject GDPRTipObj;
    public GameObject GDPRBtnObj;
    public GDPRView GdprView;

    public static float DURATION;
    public static float MIN_LEVEL_SHOW_AD = 0;

    public static int ad_frequency;

    public static string variableValue;

    public static float InterAdTimer = 0;
    public static float SubmitGameTimer = 0;
    public static bool RewardADsSuccess = false;

    public static ADSDK Intance;
    public AudioSource MusicPlayer; 
    public Text experiment_group_Debug;

    private bool _isFBInitSuccess;
    
#if UNITY_IOS
    public static string rewardedAdUnitId = "6ad58084e96facd2";
    public static string bannerAdUnitId = "1182a8b3b8678513"; //横幅广告ID 
    string interstitialAdUnitId = "a6a56cc933c24402";
#elif UNITY_ANDROID
    public static string rewardedAdUnitId = "05f43100ff70be01";
    public static string bannerAdUnitId = "567b2581d44d9028"; //横幅广告ID 
    string interstitialAdUnitId = "91743f80dc13c472";
#endif



    //【注意】插屏广告应调用这个带关卡/时长判断的函数，已集成AB_Test(未测试）
    public void CallForShowInterAD()
    {
        if (PlayerPrefs.GetInt("Level", 1) >= MIN_LEVEL_SHOW_AD)
        {
            if (ad_frequency == 5)
            {
                if (Data.FingerId == 0)
                {
                    InterAdTimer = 0;
                    showInterstitialADs();
                }
            }
            else
            {
                if (InterAdTimer > DURATION)
                {
                    InterAdTimer = 0;
                    showInterstitialADs();
                }
            }
        }
    }

    void ABTestControl()
    {
        if(ad_frequency == 30)
        {
            DURATION = 30;
        }
        else if (ad_frequency == 60)
        {
            DURATION = 60;
        }
        else if (ad_frequency == 5)
        {
            DURATION = 9999;
        }
        else
        {
            DURATION = 60;
        }
    }

    #region 交叉广告

    private bool _isShowCross = true;

    /// <summary>
    /// 显示交叉广告
    /// </summary>
    public void ShowCrossPromo()
    {
        Debug.Log("显示交叉广告");
        
        GDPRTipParentObj.SetActive(true);
        
        if (DebugManager.IsDebug || Data.NoAd_isBought == 1) return;
		
        _isShowCross = true;
		
        if (!AppLovinCrossPromo.Instance()) return;
		
        if ((float)Screen.height / Screen.width >= 1.45f)
            AppLovinCrossPromo.Instance().ShowMRec(0f, 35f, 28f, 36f, 0);
        else        //平板
            AppLovinCrossPromo.Instance().ShowMRec(0f, 35f, 28f, 36f, 0);
    }

    public void HideCrossPromo()
    {
        Debug.Log("隐藏交叉广告");

        _isShowCross = false;
        
        GDPRTipParentObj.SetActive(false);

        if (!AppLovinCrossPromo.Instance()) return;
		
        AppLovinCrossPromo.Instance().HideMRec();
    }

    #endregion

    #region 关卡进度统计

    //---------------- FaceBook发送关卡进度------------------

    public void LogAchievedLevelEvent(string level)
    {
        if (!_isFBInitSuccess) return;

        var parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.Level] = level;
        FB.LogAppEvent(
            AppEventName.AchievedLevel,0,
            parameters
        );
        Debug.Log("FaceBook发送关卡进度：" + level);

    }

    //---------------- AppsFlyer发送关卡进度------------------

    public static System.Collections.Generic.Dictionary<string, string> _dic = 
            new System.Collections.Generic.Dictionary<string, string>();

    public static void SubmitLatestLevel(int latestLevel)
    {
//        AppsFlyer.trackRichEvent( "Level" + latestLevel.ToString("000"), _dic);
//        Debug.Log("AppsFlyer发送关卡进度：" + "Level" + latestLevel.ToString("000"));
    }

    public  void SubmitGameTime()
    {
//        AppsFlyer.trackRichEvent("10 Seconds", _dic);
    }

    #endregion

    #region 广告显示函数

    //----------------广告显示------------------
    
#if UNITY_EDITOR
    public bool IsLoadRewardedVideoComplete => true;
#else
	public bool IsLoadRewardedVideoComplete => MaxSdk.IsRewardedAdReady(rewardedAdUnitId) || DebugManager.IsDebug;
#endif

    public void showRewardADs(string adsType)
    {
        if (DebugManager.IsDebug)
        {
            RewardADsSuccess = true;
            return;
        }
        
        Debug.Log("显示激励1");
        if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId))
        {
            Debug.Log("显示激励2");

            var parameters = new Dictionary<string, object>();
            parameters["RewardedAd"] = adsType;
            FB.LogAppEvent(
                "RewardedAd", 0,
                parameters
            );

            MaxSdk.ShowRewardedAd(rewardedAdUnitId);
        }
        
#if UNITY_EDITOR
        RewardADsSuccess = true;
#endif
    }

    public void showInterstitialADs()
    {
        Debug.Log("显示插屏");
        
        if (DebugManager.IsDebug || Data.NoAd_isBought == 1)
        {
            return;
        }

        if (MaxSdk.IsInterstitialReady(interstitialAdUnitId))
        {
            var parameters = new Dictionary<string, object>();
            parameters["ADType"] = "Interstitial";
            FB.LogAppEvent(
                "ADType", 0,
                parameters
            );

            MaxSdk.ShowInterstitial(interstitialAdUnitId);
        }
    }

    public void ShowBanner()
    {
        Debug.Log("显示横幅");

        //_isShowBanner = true;

        if (DebugManager.IsDebug || Data.NoAd_isBought == 1) return;
		
        MaxSdk.ShowBanner(bannerAdUnitId);
		
        //MaxSdk.SetBannerBackgroundColor(_bannerAdUnitId, new Color(0f, 0f, 0f, 0.5f));
		
        BannerBgObj.SetActive(true);
    }
	
    public void HideBanner()
    {
        Debug.Log("隐藏横幅");

        //_isShowBanner = false;
		
        MaxSdk.HideBanner(bannerAdUnitId);
		
        BannerBgObj.SetActive(false);
    }

    #endregion

    #region 激励广告

    //----------AppLovin 激励广告----------

    
    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        // Load the first RewardedAd
        LoadRewardedAd();
    }
    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);
    }
    private void OnRewardedAdLoadedEvent(string adUnitId)
    {
        Debug.Log("激励加载成功，" + adUnitId );
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
    }
    private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
    {
        // Rewarded ad failed to load. We recommend re-trying in 3 seconds.
        Debug.Log("激励加载失败，" + adUnitId + "," + errorCode);

        Invoke("LoadRewardedAd", 3);
    }
    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        LoadRewardedAd();
        
        MusicPlayer.Play();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId)
    {
        MusicPlayer.Pause();
    }
    
    private void OnRewardedAdClickedEvent(string adUnitId) { }
    private void OnRewardedAdDismissedEvent(string adUnitId)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
        
        MusicPlayer.Play();
    }
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
    {
        // Rewarded ad was displayed and user should receive the reward
        RewardADsSuccess = true;
        InterAdTimer = 0;
    }

    #endregion

    #region 横幅广告
    //----------AppLovin 横幅广告----------

    
    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, new Color(1, 1, 1, 0));
    }

    #endregion

    #region 插屏广告

    //----------AppLovin 插屏广告----------

    
    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.OnInterstitialDisplayedEvent += InterstitialSuccessToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }
    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
    }
    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        Debug.Log("插屏加载成功，" + adUnitId);

        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
    }
    private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
    {
        Debug.Log("插屏加载失败，" + adUnitId + "," + errorCode);
        // Interstitial ad failed to load. We recommend re-trying in 3 seconds.
        Invoke("LoadInterstitial", 3);
    }

    private void InterstitialSuccessToDisplayEvent(string adUnitId)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        Debug.LogError("插屏显示成功：" + adUnitId);

        Time.timeScale = 0;     //暂停游戏
        MusicPlayer.Pause();
    }


    private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        LoadInterstitial();
        
        Time.timeScale = 1;		//结束暂停
        MusicPlayer.Play();
    }

    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        LoadInterstitial();

        Time.timeScale = 1;
        MusicPlayer.Play();
    }

    #endregion

    #region Facebook 监听

    //----------Facebook SDK监听-----------
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            
            FB.Mobile.SetAutoLogAppEventsEnabled(false); 

            _isFBInitSuccess = true;

            Debug.Log("FB初始化成功");
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    //----------Facebook 暂停游戏函数-----------
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
            MusicPlayer.volume = 0f;
        }
        else
        {
            Time.timeScale = 1;
            if (Data.Setting_Sound == 1)
            {
                MusicPlayer.volume = 0.1f;
            }
            else
            {
                MusicPlayer.volume = 0f;
            }
        }
    }

    #endregion


    void Start()
    {

//        #region AppsFlyer SDK装载
//
//        //----------AppsFlyer SDK装载-----------
//        
//
//#if UNITY_IOS
//        AppsFlyer.setAppsFlyerKey("zcKrZYJWnrWWctCxcLNnyT");
//        AppsFlyer.setAppID("1487604820"); //商店AppID
//        AppsFlyer.trackAppLaunch();
//
//#elif UNITY_ANDROID
//        AppsFlyer.setAppsFlyerKey("7yZY6gN88m48knvBDVDCyB");
//        AppsFlyer.setAppID ("com.lioncel.nailsdone");
//        /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
//        AppsFlyer.init ("7yZY6gN88m48knvBDVDCyB","AppsFlyerTrackerCallbacks");
//
//#endif

//        #endregion

        //此处集成experiment_group事件
        #region AppLovin SDK装载

        //----------AppLovin SDK装载 step1-----------
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            Debug.Log("Max SDK 初始化成功：" + sdkConfiguration.ConsentDialogState);
            
            if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.Applies)
            {
                Debug.Log("需要弹询问同意对话框");

                var status = GDPRView.Instance.Status;
                if (status < 0)
                    GDPRView.Instance.Show(DialogFinished);
                else
                {
                    DialogFinished(GDPRView.Instance.IsAgreeAds);
                }

                GDPRBtnObj.SetActive(true);
                JudgeGDPRShow();
            }
            else if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.DoesNotApply)
            {
                Debug.Log("不需要弹询问同意对话框");

                InitAds();
                
                GDPRBtnObj.SetActive(false);
                GDPRTipObj.SetActive(false);
            }
            else
            {
                Debug.Log("判断未知，无视，直接初始化");
				
                InitAds();
                
                GDPRBtnObj.SetActive(false);
                GDPRTipObj.SetActive(false);
            }
        };


        //----------AppLovin SDK装载 step2-----------

        MaxSdk.SetSdkKey("pIZT0gTB19HmoBhtMBRD2fXDkHJC9HryZOUa4yn552suDTlakAomrJzZbGmlTbg6_Ah46SACU05iTqHG_40rN-");
        MaxSdk.InitializeSdk();
        
        AppLovinCrossPromo.Init();

        #endregion
    }
    
    public void OnClickGDPRBtn()
    {
        GDPRView.Instance.Show(CallBackConsent, false, false);
    }
    
    public void OnClickGDPRTip()
    {
        GDPRView.Instance.Show(CallBackConsent, false);
    }
    
    public void JudgeGDPRShow()
    {
        GDPRTipObj.SetActive(GDPRView.Instance.IsShowTipView);
    }
    
    public void CallBackConsent(bool result)
    {
        Debug.Log("用户是否同意：" + result);
		
        MaxSdk.SetHasUserConsent(result);
    }
	
    private void DialogFinished(bool result)
    {
        Debug.Log("用户是否同意：" + result);
		
        MaxSdk.SetHasUserConsent(result);
		
        InitAds();
    }

    private void InitAds()
    {
        InitializeBannerAds();
        InitializeInterstitialAds();
        InitializeRewardedAds();

        //if (_isShowCross) ShowCrossPromo();    //在闪屏消失后，会调用
        
        // Show Mediation Debugger
        if (DebugManager.IsDebug) MaxSdk.ShowMediationDebugger();

        //FaceBook获取 experiment_group 并提交
        variableValue = MaxSdk.VariableService.GetString("experiment_group");

        experiment_group_Debug.text = "experiment_group：" + variableValue;

        var parameters = new Dictionary<string, object>();
        parameters["experiment_group"] = variableValue;
        FB.LogAppEvent(
            "experiment_group", 0,
            parameters
        );
    }


    void Awake()
    {

        Intance = this; //单例
        
        GdprView.Init();

        #region Facebook SDK装载

        //----------Facebook SDK装载-----------

        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }

        #endregion

    }

    void Update()
    {

        //已集成AB_Test的DURATION
        #region 插屏广告计时器

        if (InterAdTimer <= DURATION)
        {
            InterAdTimer += Time.deltaTime;
        }

        #endregion

        #region 发送游戏时长统计

        if (SubmitGameTimer < 10)
        {
            SubmitGameTimer += Time.deltaTime;
        }
        else
        {
            SubmitGameTime();
            SubmitGameTimer = 0;
            //Debug.Log("十秒了！！");
        }

        #endregion

        ABTestControl();

    }

}
