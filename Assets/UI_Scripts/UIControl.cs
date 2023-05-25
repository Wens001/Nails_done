using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TapticPlugin;

public class UIControl : MonoBehaviour
{
    Sequence mySequence;

    public float LogoDisplayTime = 1;

    #region UI物件声明

    public Canvas canvas;

    public RectTransform MainCanvas;
    public RectTransform BookPanel; //用于自动调整Ipad下图鉴的位置

    public Image LOGO;
    public Image LOGO_BG;

    public Image UI_Title;
    public Image UI_Setting;
    public Image UI_NoAD;
    public Image UI_Book;
    public Image UI_LikeNumBG;
    public Text UIText_LikeNum;
    public Text UIText_BookLikeNum;

    public Image UIText_StartBtn;
    public Image UI_SettingBG;
    public Image UI_ColorBtn1;
    public Image UI_ColorBtn2;
    public Image UI_ColorBtn3;
    public Image UI_ColorBtnS1;
    public Image UI_ColorBtnS2;
    public Image UI_ColorBtnS3;

    public Image UI_SpecialBtnGroup;

    public Image UI_Level;
    public Text UI_LevelNum;
    public Image UI_ResetBtn;
    public Image UI_HomeBtn;
    public Image UI_BigBubble;
    public RawImage UI_PreImg;
    public Image UI_BubbleBundle;
    public Image UI_Bubble1;
    public Image UI_Bubble2;
    public Image UI_Bubble3;
    public Image UI_Bubble4;
    public Image UI_NextFingerBtn;
    public Image UI_ShotBtn;
    public Image UI_Tips;

    public Image UI_LevelTag1;
    public Image UI_LevelTag2;
    public Image UI_LevelTag3;
    public Image UI_LevelTag4;
    public Image UI_LevelTag5;

    public Image FailedBG;
    public Image UI_BrokenFinger;
    public Image UI_Ouch;
    public Image UI_TryAgainBtn;

    public Image UI_FinishBG;
    public Image UI_InsBG;
    public Image UI_InsPhoto;
    public Image UI_NextLevelBtn;

    public Image UI_LikeNumX2Btn;
    public Image UI_EndWord;

    public Image UI_BigLike;
    public Text UIText_LikeNumForLevel;
    public Text UIText_InsText;
    public Text UIText_InsTag;

    public Image UI_CollectSelect_1;
    public Image UI_CollectSelect_2;
    public Image UI_CollectSelect_3;
    public Image UI_CollectSelect_4;

    public GameObject FlowerLeft;
    public GameObject FlowerRight;

    public Image WhileScreen;

    public static UIControl Instance;

    #endregion

    #region 状态接口【外部对接】

    //UI状态，进入到状态机
    public static string UIState = "LogoPage";
    /*
                                  
                                  * MainMenu
                                  * InGame
                                  * FailedGame
                                  * FinishGame

    */

    //UI想要切换到的场景，进入到状态机
    public static string UIWanna = "MainMenu";
    /*
                                  
                                  * MainMenu
                                  * InGame
                                  * FinishGame
    */

    //目前涂指甲的总层数
    public static int InGame_SumStep = 3;

    //目前涂指甲的总层数
    public static int InGame_NowStep = 0;
    /*                                 
                                     * 1  底色层
                                     * 2  第一层花
                                     * 3  第二层花
    */

    //是否显示手指点击泡泡的提示
    public static bool InGame_NextStepTips = false;

    //本关将获得的点赞数（结算页面）
    public static int FinishGame_LikeWillGet = 500;

    //玩家的总赞量
    public static int Player_LikeNum = 0;

    //玩家选择的肤色
    public static int HandColor_Select = 1;  //1白 2黄 3黑

    public static int Collect_Select = 0; //图鉴页数，决定加载页和按钮弹出状态


    #endregion

    #region 内部变量（不用管）

    private float UITimer = 0;
    private int MainMenu_ActionNum = 1; //用于处理MainMenu页面按钮出现动画稍微交错
    private int InGame_ActionNum = 1; //用于处理InGame页面按钮出现动画稍微交错
    public static bool InGame_BubbleDisplay = false;
    public static int FinishGame_ActionNum = 1; //用于处理FinishGame页面动画稍微交错
    private int FinishGame_LikeNumForLevelDisplay;

    

    private float UIPosOffset = 0;

    #endregion

    #region 内部函数（触发事件）


    void TopUIOffset()
    {
        V3_Setting_In -= new Vector3(0, UIPosOffset, 0);
        V3_LikeNumBG_In -= new Vector3(0, UIPosOffset, 0);
        V3_Level_In -= new Vector3(0, UIPosOffset, 0);
        V3_ResetBtn_In -= new Vector3(0, UIPosOffset, 0);
        V3_HomeBtn_In -= new Vector3(0, UIPosOffset, 0);
        V3_Title_In -= new Vector3(0, UIPosOffset, 0);


        UI_BigBubble.rectTransform.anchoredPosition -= new Vector2(0, UIPosOffset);
        UI_BubbleBundle.rectTransform.anchoredPosition -= new Vector2(0, UIPosOffset);

    }


    void BottomUIOffset()
    {
        v3_NextFingerBtn_In += new Vector3(0, UIPosOffset, 0);
        v3_ShotBtn_In += new Vector3(0, UIPosOffset, 0);

        V3_NextLevelBtn_In += new Vector3(0, UIPosOffset, 0);
        V3_LikeNumX2Btn_In += new Vector3(0, UIPosOffset, 0);

        v3_TryAgainBtn_In += new Vector3(0, UIPosOffset, 0);

        V3_StartBtn_In += new Vector3(0, UIPosOffset * 1.4f, 0);

        V3_SpecialBtnGroup_In += new Vector3(0, UIPosOffset, 0);
    }


    public  void CanBeNextFinger()
    {
        UI_NextFingerBtn.raycastTarget = true;
        UI_NextFingerBtn.rectTransform.DOAnchorPos3D(v3_NextFingerBtn_In, 0.5f).SetEase(Ease.OutBack);
    }

    public void OffNextFinger()
    {
        UI_NextFingerBtn.rectTransform.anchoredPosition = v3_NextFingerBtn_Out;
        
    }

    public  void CanBeShot()
    {
        UI_ShotBtn.rectTransform.DOAnchorPos3D(v3_ShotBtn_In, 0.5f).SetEase(Ease.OutBack);
    }

    public void OffShot()
    {
        UI_ShotBtn.rectTransform.anchoredPosition = v3_ShotBtn_Out;
    }

    public  void LevelFailed()
    {
        UITimer = 0;
        UIState = "FailedGame";
        InGameReady();
    }

     void CollectBtn_Control()
    {
        /*
        if (Collect_Select == 1)
        {
            UI_CollectSelect_1.color = new Color(1, 1, 1, 1);
            UI_CollectSelect_2.color = new Color(1, 1, 1, 0);
            UI_CollectSelect_3.color = new Color(1, 1, 1, 0);
            UI_CollectSelect_4.color = new Color(1, 1, 1, 0);
        }
        else if (Collect_Select == 2)
        {
            UI_CollectSelect_1.color = new Color(1, 1, 1, 0);
            UI_CollectSelect_2.color = new Color(1, 1, 1, 1);
            UI_CollectSelect_3.color = new Color(1, 1, 1, 0);
            UI_CollectSelect_4.color = new Color(1, 1, 1, 0);
        }
        else if (Collect_Select == 3)
        {
            UI_CollectSelect_1.color = new Color(1, 1, 1, 0);
            UI_CollectSelect_2.color = new Color(1, 1, 1, 0);
            UI_CollectSelect_3.color = new Color(1, 1, 1, 1);
            UI_CollectSelect_4.color = new Color(1, 1, 1, 0);
        }
        else if (Collect_Select == 4)
        {
            UI_CollectSelect_1.color = new Color(1, 1, 1, 0);
            UI_CollectSelect_2.color = new Color(1, 1, 1, 0);
            UI_CollectSelect_3.color = new Color(1, 1, 1, 0);
            UI_CollectSelect_4.color = new Color(1, 1, 1, 1);
        }
        */
    }

    void ColorBtn_Control()
    {
        if (HandColor_Select == 1)
        {
            UI_ColorBtnS1.color = new Color(1, 1, 1, 1);
            UI_ColorBtnS2.color = new Color(1, 1, 1, 0);
            UI_ColorBtnS3.color = new Color(1, 1, 1, 0);
        }
        else if (HandColor_Select == 2)
        {
            UI_ColorBtnS1.color = new Color(1, 1, 1, 0);
            UI_ColorBtnS2.color = new Color(1, 1, 1, 1);
            UI_ColorBtnS3.color = new Color(1, 1, 1, 0);
        }
        else if (HandColor_Select == 3)
        {
            UI_ColorBtnS1.color = new Color(1, 1, 1, 0);
            UI_ColorBtnS2.color = new Color(1, 1, 1, 0);
            UI_ColorBtnS3.color = new Color(1, 1, 1, 1);
        }
        Material handMat = ObjectData.MaterialRegister(ObjectData.HANDMAT);
        Texture texture = Resources.Load("Textrue/Skin" + HandColor_Select)as Texture;
        handMat.SetTexture("_MainTex",texture);
        Data.HandSkinSelect = HandColor_Select;
        PlayerPrefs.SetInt("HandSkinSelect", Data.HandSkinSelect);
    }


    void LevelTagControl()
    {
        if(Data.FingerId == 1)
        {
            UI_LevelTag1.enabled = true;
            UI_LevelTag2.enabled = false;
            UI_LevelTag3.enabled = false;
            UI_LevelTag4.enabled = false;
            UI_LevelTag5.enabled = false;
        }
        else if (Data.FingerId == 2)
        {
            UI_LevelTag1.enabled = false;
            UI_LevelTag2.enabled = true;
            UI_LevelTag3.enabled = false;
            UI_LevelTag4.enabled = false;
            UI_LevelTag5.enabled = false;
        }
        else if (Data.FingerId == 3)
        {
            UI_LevelTag1.enabled = false;
            UI_LevelTag2.enabled = false;
            UI_LevelTag3.enabled = true;
            UI_LevelTag4.enabled = false;
            UI_LevelTag5.enabled = false;
        }
        else if (Data.FingerId == 4)
        {
            UI_LevelTag1.enabled = false;
            UI_LevelTag2.enabled = false;
            UI_LevelTag3.enabled = false;
            UI_LevelTag4.enabled = true;
            UI_LevelTag5.enabled = false;
        }
        else if (Data.FingerId == 5)
        {
            UI_LevelTag1.enabled = false;
            UI_LevelTag2.enabled = false;
            UI_LevelTag3.enabled = false;
            UI_LevelTag4.enabled = false;
            UI_LevelTag5.enabled = true;
        }
    }

    public void BackToMenuInEnd()
    {
        UI_FinishBG.rectTransform.DOAnchorPos3D(V3_FinishBG_Out, 0.5f).SetEase(Ease.InBack);
        UI_NextLevelBtn.rectTransform.DOAnchorPos3D(V3_NextLevelBtn_Out, 0.5f).SetEase(Ease.InBack);
        UI_LikeNumX2Btn.rectTransform.DOAnchorPos3D(V3_LikeNumX2Btn_Out, 0.5f).SetEase(Ease.InBack);
        UI_LikeNumBG.rectTransform.DOAnchorPos3D(V3_LikeNumBG_Out, 0.5f).SetEase(Ease.InBack);
        UIWanna = "MainMenu";
        UIState = "MainMenu";
        AllActionNumReset();
        UITimer = 0;
    }



    #endregion

    #region UI位置记录

    //主界面
    private Vector3 V3_Setting_In = new Vector3(-100, -100, 0);
    private Vector3 V3_Setting_Out = new Vector3(-100, 200, 0);

    private Vector3 V3_NoAD_In = new Vector3(-91, -450, 0);
    [HideInInspector]
    public Vector3 V3_NoAD_Out = new Vector3(91, -450, 0);


    private Vector3 V3_Book_In = new Vector3(0, 0f, 0);
    public Vector3 V3_Book_Out = new Vector3(182, 0f, 0);
    private Vector3 V3_Book_INMAX = new Vector3(-1080, 0f, 0);

    private Vector3 V3_LikeNumBG_In = new Vector3(156, -101, 0);
    private Vector3 V3_LikeNumBG_Out = new Vector3(156, 200, 0);

    private Vector3 V3_SettingBG_In = new Vector3(-540, 0, 0);
    private Vector3 V3_SettingBG_Out = new Vector3(540, 0, 0);

    private Vector3 V3_ColorBtn1_In = new Vector3(-74.5f, 46, 0);
    private Vector3 V3_ColorBtn1_Out = new Vector3(74.5f, 46, 0);

    private Vector3 V3_ColorBtn2_In = new Vector3(-74.5f, -93, 0);
    private Vector3 V3_ColorBtn2_Out = new Vector3(74.5f, -93, 0);

    private Vector3 V3_ColorBtn3_In = new Vector3(-74.5f, -233, 0);
    private Vector3 V3_ColorBtn3_Out = new Vector3(74.5f, -233, 0);

    private Vector3 V3_Title_In = new Vector3(0, -386, 0);

    private Vector3 V3_StartBtn_In = new Vector3(-4f, -154f, 0);

    private Vector3 V3_SpecialBtnGroup_In = new Vector3(0, 16, 0);


    //游戏中
    private Vector3 V3_Level_In = new Vector3(0, -118, 0);
    private Vector3 V3_Level_Out= new Vector3(0, 260, 0);

    private Vector3 V3_ResetBtn_In = new Vector3(-105, -105, 0);
    private Vector3 V3_ResetBtn_Out = new Vector3(-105, 260, 0);

    private Vector3 V3_HomeBtn_In = new Vector3(105, -105, 0);
    private Vector3 V3_HomeBtn_Out = new Vector3(105, 260, 0);

    private Vector3 v3_NextFingerBtn_In = new Vector3(0, 310, 0);
    private Vector3 v3_NextFingerBtn_Out = new Vector3(0, -300, 0);

    public Vector3 v3_ShotBtn_In = new Vector3(0, 340, 0);
    public Vector3 v3_ShotBtn_Out = new Vector3(0, -300, 0);

    private Vector3 v3_TryAgainBtn_In = new Vector3(0, 335, 0);
    private Vector3 v3_TryAgainBtn_Out = new Vector3(0, -300, 0);


    //结算
    private Vector3 V3_NextLevelBtn_In = new Vector3(376, 310, 0);
    private Vector3 V3_NextLevelBtn_Out = new Vector3(376, -300, 0);

    private Vector3 V3_LikeNumX2Btn_In = new Vector3(0, 310, 0);
    private Vector3 V3_LikeNumX2Btn_Out = new Vector3(0, -300, 0);
    
    private Vector3 V3_FinishBG_In = new Vector3(0, 0, 0);
    private Vector3 V3_FinishBG_Out = new Vector3(-2000, 0, 0);



    #endregion

    #region 按钮函数
    public void Btn_StartGame()
    {
        if (UIWanna == "MainMenu" && Data.isMenu ==1)
        {
            Btn_OptionBack();
            
            UIWanna = "InGame";
            AllActionNumReset();
            BubbleReady();
            UITimer = 0;
            
            ADSDK.Intance.HideCrossPromo();
        }
    }

    public void Btn_Option()
    {
        UI_SettingBG.rectTransform.DOAnchorPos3D(V3_SettingBG_In, 0.3f).SetEase(Ease.OutQuart);
        
        ADSDK.Intance.HideCrossPromo();
    }

    public void Btn_OptionBack()
    {
        UI_SettingBG.rectTransform.DOAnchorPos3D(V3_SettingBG_Out, 0.3f).SetEase(Ease.OutQuart);
        
        ADSDK.Intance.ShowCrossPromo();
    }

    public void Btn_Collection()
    {
        UI_Book.rectTransform.DOAnchorPos3D(V3_Book_INMAX, 0.3f).SetEase(Ease.OutQuart);
        BookScript.RefreshSwitch = true;
        
        ADSDK.Intance.HideBanner();
        ADSDK.Intance.HideCrossPromo();
    }

    public void Btn_Collection_Home()
    {
        UI_Book.rectTransform.DOAnchorPos3D(V3_Book_In, 0.3f).SetEase(Ease.OutQuart).onComplete = () =>
        {
            ADSDK.Intance.ShowBanner();
            ADSDK.Intance.ShowCrossPromo();
        };
    }

    public void Btn_Collection_Select1()
    {
        Collect_Select = 1;
    }

    public void Btn_Collection_Select2()
    {
        Collect_Select = 2;
    }

    public void Btn_Collection_Select3()
    {
        Collect_Select = 3;
    }

    public void Btn_Collection_Select4()
    {
        Collect_Select = 4;
    }

    public void Btn_HandColor_Select1()
    {
        HandColor_Select = 1;
        ColorBtn_Control();
    }

    public void Btn_HandColor_Select2()
    {
        HandColor_Select = 2;
        ColorBtn_Control();
    }

    public void Btn_HandColor_Select3()
    {
        HandColor_Select = 3;
        ColorBtn_Control();
    }

    public void Btn_NoAD()
    {
        IOSPurchaseManager.Instance.PurchaseShop(IOSPurchaseManager.ShopAds);
    }

    public void Btn_BackHome()
    {
        if (UIWanna == "InGame")
        {
            UIWanna = "MainMenu";
            AllActionNumReset();
            UITimer = 0;
            
            ADSDK.Intance.ShowCrossPromo();
        }
    }

    public void Btn_JumpLevel()
    {

        //激励广告请求
        ADRewardControl.RewardType = 4;
        ADSDK.Intance.showRewardADs("skiplevel");

    }

    public void Btn_NextFinger()
    {
        InGame_NowStep = 0;
        InGame_BubbleDisplay = false;
        UI_NextFingerBtn.raycastTarget = false;
        UI_NextFingerBtn.rectTransform.DOAnchorPos3D(v3_NextFingerBtn_Out, 0.5f).SetEase(Ease.InBack);
    }


    public void Btn_Shot()
    {
        UITimer = 0;
        UIState = "FinishGame";
        FinishGame_LikeNumForLevelDisplay = 0;
        AllActionNumReset();
    }

    public void Btn_NextLevel()
    {
        UITimer = 0;
        AllActionNumReset();
        UIState = "InGame";
        UIWanna = "InGame";
        UI_FinishBG.rectTransform.DOAnchorPos3D(V3_FinishBG_Out, 0.5f).SetEase(Ease.InBack);
        UI_NextLevelBtn.rectTransform.DOAnchorPos3D(V3_NextLevelBtn_Out, 0.5f).SetEase(Ease.InBack);

        UI_LikeNumX2Btn.rectTransform.DOAnchorPos3D(V3_LikeNumX2Btn_Out,0.5f).SetEase(Ease.InBack);

        UI_LikeNumBG.rectTransform.DOAnchorPos3D(V3_LikeNumBG_Out, 0.5f).SetEase(Ease.InBack);


    }

    public void Btn_NextLevelX2()
    {

        //激励广告请求
        ADRewardControl.RewardType = 3;
        ADSDK.Intance.showRewardADs("double_settlement");

    }

    public void Btn_TryAgain()
    {
        UITimer = 0;
        BubbleReady();
        FailedGameReady();
        UIState = "InGame";
        UIWanna = "InGame";
        AllActionNumReset();
    }

    #endregion

    #region 初始化模块

    public void AllActionNumReset()
    {
        InGame_ActionNum = 1;
        MainMenu_ActionNum = 1;
        FinishGame_ActionNum = 1;
    }


    public void MainMenuReady()
    {
        UI_Title.color = new Color(1, 1, 1, 0);
        UI_Setting.rectTransform.anchoredPosition = V3_Setting_Out;
        UI_NoAD.rectTransform.anchoredPosition = V3_NoAD_Out;
        UI_Book.rectTransform.anchoredPosition = V3_Book_Out;
        UI_LikeNumBG.rectTransform.anchoredPosition = V3_LikeNumBG_Out;
        UIText_StartBtn.color -= new Color(0, 0, 0, UIText_StartBtn.color.a);

        UIText_StartBtn.enabled = false;

        UI_SettingBG.rectTransform.anchoredPosition = V3_SettingBG_Out;

        UI_ColorBtn1.rectTransform.anchoredPosition = V3_ColorBtn1_Out;
        UI_ColorBtn2.rectTransform.anchoredPosition = V3_ColorBtn2_Out;
        UI_ColorBtn3.rectTransform.anchoredPosition = V3_ColorBtn3_Out;

        //图鉴位置在ipad下的修正
        BookPanel.anchoredPosition = new Vector2(MainCanvas.rect.width, 0);
        V3_Book_INMAX = new Vector3(-MainCanvas.rect.width, 0, 0);

        UI_SpecialBtnGroup.rectTransform.anchoredPosition = V3_SpecialBtnGroup_In;
    }


    public void InGameReady()
    {
        UI_Level.rectTransform.anchoredPosition = V3_Level_Out;
        UI_HomeBtn.rectTransform.anchoredPosition = V3_HomeBtn_Out;
        UI_ResetBtn.rectTransform.anchoredPosition = V3_ResetBtn_Out;

        UI_Tips.color -= new Color(0, 0, 0, UI_Tips.color.a);

        UI_NextFingerBtn.rectTransform.anchoredPosition = v3_NextFingerBtn_Out;
        UI_ShotBtn.rectTransform.anchoredPosition = v3_ShotBtn_Out;

        InGame_NowStep = 0;

    }


    public void LevelTagReady()
    {
        UI_LevelTag1.enabled = false;
        UI_LevelTag2.enabled = false;
        UI_LevelTag3.enabled = false;
        UI_LevelTag4.enabled = false;
        UI_LevelTag5.enabled = false;
    }


    public void BubbleReady()
    {
        UI_BigBubble.rectTransform.localScale = new Vector3(0.4f, 0.4f, 1);
        UI_BigBubble.color -= new Color(0, 0, 0, UI_BigBubble.color.a);
        UI_PreImg.color -= new Color(0, 0, 0, UI_PreImg.color.a);

        UI_BubbleBundle.rectTransform.localScale = new Vector3(0.4f, 0.4f, 1);
        UI_BubbleBundle.rectTransform.eulerAngles = new Vector3(0, 0, 350);
        UI_BubbleBundle.color -= new Color(0, 0, 0, UI_BubbleBundle.color.a);

        UI_Bubble1.color -= new Color(0, 0, 0, UI_Bubble1.color.a);
        UI_Bubble2.color -= new Color(0, 0, 0, UI_Bubble2.color.a);
        UI_Bubble3.color -= new Color(0, 0, 0, UI_Bubble3.color.a);
        UI_Bubble4.color -= new Color(0, 0, 0, UI_Bubble4.color.a);
    }

    
    public void FailedGameReady()
    {
        FailedBG.enabled = false;
        UI_BrokenFinger.enabled = false;
        UI_Ouch.enabled = false;

        UI_TryAgainBtn.rectTransform.anchoredPosition = v3_TryAgainBtn_Out;
    }


    public void FinishGameReady()
    {
        UI_BigLike.color = new Color(1, 1, 1, 0);
        UI_EndWord.color = new Color(1, 1, 1, 0);
        WhileScreen.color = new Color(1, 1, 1, 0);
        UI_BigLike.rectTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        UI_FinishBG.rectTransform.anchoredPosition = V3_FinishBG_Out;
        UI_NextLevelBtn.rectTransform.anchoredPosition = V3_NextLevelBtn_Out;
        UI_LikeNumX2Btn.rectTransform.anchoredPosition = V3_LikeNumX2Btn_Out;
    }


    

    public void NextStpe(int stype,int sumStep)
    {
        if (stype <= sumStep+1)
        {
            //sumStep++;
            UI_BubbleBundle.rectTransform.DORotate(new Vector3(0, 0, 180 - 34 * (stype - 1)), 0.5f).SetEase(Ease.OutBack);
        }
    }

    public void SmallBubbleReset()
    {
        UI_BubbleBundle.rectTransform.DORotate(new Vector3(0, 0, 180), 0.5f).SetEase(Ease.OutBack);
    }


    #endregion


    void FunctionDebug()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InGame_NextStepTips = !InGame_NextStepTips;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CanBeNextFinger();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CanBeShot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            LevelFailed();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameManger.Intance.Reward_JumpLevel();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ADSDK.RewardADsSuccess = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Data.LikeNum += 10000;
        }

    }


    public void InitializeUI()
    {
        MainMenuReady();
        InGameReady();
        LevelTagReady();
        BubbleReady();
        FailedGameReady();
        FinishGameReady();
    }


    void Start()
    {
        
        Instance = this;
        //-------摄像机fov校正--------
        //float ratioCam = (720f / 1280) / (Screen.width * 1f / Screen.height);
        //Camera.main.fieldOfView = Mathf.Max(52, ratioCam * 52);


        //-------刘海屏顶部UI校正--------
        float ratio = Screen.width * 1f / Screen.height;
        if (ratio <= 0.5f)
        {
            UIPosOffset = 90;
        }
        else
        {
            UIPosOffset = 0;
        }

        TopUIOffset();
        BottomUIOffset();
        InitializeUI();

        ColorBtn_Control();

    }

    
    void Update()
    {

        
        FunctionDebug();
        CollectBtn_Control();
        LevelTagControl();

        UIText_LikeNumForLevel.text = FinishGame_LikeNumForLevelDisplay.ToString();



        float LikeNumTemp = Mathf.Floor(Data.LikeNum * 0.01f) * 0.1f;

        if (Data.LikeNum < 1000)
        {
            UIText_LikeNum.text = Data.LikeNum.ToString();
            UIText_BookLikeNum.text = Data.LikeNum.ToString();
        }
        else if (Data.LikeNum >= 1000 && Data.LikeNum < 100000)
        {
            UIText_LikeNum.text = LikeNumTemp.ToString("0.0") + "K";
            UIText_BookLikeNum.text = LikeNumTemp.ToString("0.0") + "K";
        }
        else if (Data.LikeNum >= 100000)
        {
            UIText_LikeNum.text = LikeNumTemp.ToString("n0") + "K";
            UIText_BookLikeNum.text = LikeNumTemp.ToString("n0") + "K";
        }



        //终极UI状态机，设计模式【时间轴】

        //【LOGO页面】
        if (UIState == "LogoPage")
        {
            UITimer += Time.deltaTime;

            if (UITimer > LogoDisplayTime)
            {
                if (LOGO.color.a > 0)
                {
                    LOGO.color -= new Color(0, 0, 0, Time.deltaTime * 2F);
                    LOGO_BG.color -= new Color(0, 0, 0, Time.deltaTime * 2F);
                }
                else
                {
                    UITimer = 0;
                    UIWanna = "MainMenu";
                    UIState = "MainMenu"; //播放完LOGO后，状态跳到【主菜单】
                    
                    ADSDK.Intance.ShowBanner();//播放横幅广告
                    ADSDK.Intance.ShowCrossPromo();
                }
            }
        }

        //【主页面】-----------------------------------------------------------------------
        else if (UIState == "MainMenu")
        {
            //进入动画集
            if (UIWanna == "MainMenu")
            {
                if (UITimer < 1.5f)
                {
                    UITimer += Time.deltaTime;
                    if (UITimer > 0.2f && MainMenu_ActionNum == 1)
                    {
                        UI_Setting.rectTransform.DOAnchorPos3D(V3_Setting_In, 0.5f).SetEase(Ease.OutBack);
                        MainMenu_ActionNum++;
                    }
                    if (UITimer > 0.3f && MainMenu_ActionNum == 2)
                    {

                        if (Data.NoAd_isBought == 0)
                        {
                            UI_NoAD.rectTransform.DOAnchorPos3D(V3_NoAD_In, 0.5f).SetEase(Ease.OutQuart);
                        }
                        MainMenu_ActionNum++;
                    }
                    if (UITimer > 0.4f && MainMenu_ActionNum == 3)
                    {
                        UI_Title.rectTransform.anchoredPosition = V3_Title_In;
                        UI_Title.DOFade(1, 1f);
                        UI_Book.rectTransform.DOAnchorPos3D(V3_Book_In, 0.5f).SetEase(Ease.OutQuart);
                        MainMenu_ActionNum++;
                    }
                    if (UITimer > 0.5f && MainMenu_ActionNum == 4)
                    {
                        UI_LikeNumBG.rectTransform.DOAnchorPos3D(V3_LikeNumBG_In, 0.5f).SetEase(Ease.OutBack);

                        UIDailyControl.Intance.ShowDailyReward(); // 展示每日奖励时间，顺而触发离线奖励

                        MainMenu_ActionNum++;
                    }

                    if (UITimer > 0.6f && MainMenu_ActionNum == 5)
                    {
                        UI_ColorBtn1.rectTransform.DOAnchorPos3D(V3_ColorBtn1_In, 0.5f).SetEase(Ease.OutBack);
                        MainMenu_ActionNum++;
                    }

                    if (UITimer > 0.7f && MainMenu_ActionNum == 6)
                    {
                        UI_ColorBtn2.rectTransform.DOAnchorPos3D(V3_ColorBtn2_In, 0.5f).SetEase(Ease.OutBack);
                        MainMenu_ActionNum++;
                    }

                    if (UITimer > 0.8f && MainMenu_ActionNum == 7)
                    {
                        UI_ColorBtn3.rectTransform.DOAnchorPos3D(V3_ColorBtn3_In, 0.5f).SetEase(Ease.OutBack);
                        MainMenu_ActionNum++;
                    }

                    if (UIText_StartBtn.color.a < 1)
                    {
                        UIText_StartBtn.enabled = true;
                        UIText_StartBtn.rectTransform.anchoredPosition = V3_StartBtn_In;
                        UIText_StartBtn.color += new Color(0, 0, 0, Time.deltaTime * 2f);
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    if (MainMenu_ActionNum == 8)
                    {
                        Data.isMenu = 1;
                        MainMenu_ActionNum++;
                    }
                }
            }
            //离开动画集
            else if (UIWanna != "MainMenu")
            {
                if (UITimer < 1.5f)
                {
                    UITimer += Time.deltaTime;
                    if (UITimer > 0.2f && MainMenu_ActionNum == 1)
                    {
                        UI_Setting.rectTransform.DOAnchorPos3D(V3_Setting_Out, 0.5f).SetEase(Ease.InBack);
                        MainMenu_ActionNum++;
                    }
                    if (UITimer > 0.3f && MainMenu_ActionNum == 2)
                    {
                        UI_NoAD.rectTransform.DOAnchorPos3D(V3_NoAD_Out, 0.5f).SetEase(Ease.OutQuart);
                        MainMenu_ActionNum++;
                    }
                    if (UITimer > 0.4f && MainMenu_ActionNum == 3)
                    {
                        UI_Title.DOFade(0, 1f);
                        UI_Book.rectTransform.DOAnchorPos3D(V3_Book_Out, 0.5f).SetEase(Ease.OutQuart).onComplete = () =>
                        {
                            ADSDK.Intance.ShowBanner();
                        };
                        MainMenu_ActionNum++;
                    }
                    if (UITimer > 0.5f && MainMenu_ActionNum == 4)
                    {
                        UI_LikeNumBG.rectTransform.DOAnchorPos3D(V3_LikeNumBG_Out, 0.5f).SetEase(Ease.InBack);
                        MainMenu_ActionNum++;
                    }

                    if (UITimer > 0.6f && MainMenu_ActionNum == 5)
                    {
                        UI_ColorBtn1.rectTransform.DOAnchorPos3D(V3_ColorBtn1_Out, 0.5f).SetEase(Ease.InBack);
                        MainMenu_ActionNum++;
                    }

                    if (UITimer > 0.7f && MainMenu_ActionNum == 6)
                    {
                        UI_ColorBtn2.rectTransform.DOAnchorPos3D(V3_ColorBtn2_Out, 0.5f).SetEase(Ease.InBack);
                        MainMenu_ActionNum++;
                    }

                    if (UITimer > 0.8f && MainMenu_ActionNum == 7)
                    {
                        UI_ColorBtn3.rectTransform.DOAnchorPos3D(V3_ColorBtn3_Out, 0.5f).SetEase(Ease.InBack);
                        MainMenu_ActionNum++;
                    }

                    if (UIText_StartBtn.color.a > 0)
                    {
                        UIText_StartBtn.color -= new Color(0, 0, 0, Time.deltaTime * 2f);
                    }
                    else
                    {
                        UIText_StartBtn.enabled = false;

                        

                    }
                }
                else
                {
                    UITimer = 0;
                    UIState = "InGame";
                }
            }
        }

        //【游戏中】-----------------------------------------------------------------------
        else if (UIState == "InGame")
        {
            //进入动画集
            if (UIWanna == "InGame")
            {
                if (UITimer < 2)
                {
                    UITimer += Time.deltaTime;
                    if (UITimer > 0.1f && InGame_ActionNum == 1)
                    {
                        UI_Level.rectTransform.DOAnchorPos3D(V3_Level_In, 0.5f).SetEase(Ease.OutBack);
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.2f && InGame_ActionNum == 2)
                    {
                        UI_HomeBtn.rectTransform.DOAnchorPos3D(V3_HomeBtn_In, 0.5f).SetEase(Ease.OutBack);
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.3f && InGame_ActionNum == 3)
                    {
                        if (Data.isSpecialLevel)
                            UI_ResetBtn.gameObject.SetActive(false);
                        else
                        {
                            UI_ResetBtn.gameObject.SetActive(true);
                            UI_ResetBtn.rectTransform.DOAnchorPos3D(V3_ResetBtn_In, 0.5f).SetEase(Ease.OutBack);
                        }
                        
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.4f && InGame_ActionNum == 4)
                    {
                        UI_BigBubble.rectTransform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.OutBack);
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.41f)
                    {
                        if (UI_BigBubble.color.a < 1)
                        {
                            UI_BigBubble.color += new Color(0, 0, 0, Time.deltaTime * 2f);
                            UI_PreImg.color += new Color(0, 0, 0, Time.deltaTime * 2f);

                        }
                    }
                    if (UITimer > 0.5f && InGame_ActionNum == 5)
                    {
                        InGame_BubbleDisplay = true;
                        UI_BubbleBundle.rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
                        UI_BubbleBundle.rectTransform.DORotate(new Vector3(0, 0, 180), 1f);
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.6f)
                    {
                        if (UI_Bubble1.color.a < 1)
                        {
                            InGame_BubbleDisplay = true;
                            //SmallBubbleReset();
                        }
                    }
                }
                else
                {
                    //InGame_BubbleDisplay = true;
                }
            }
            //离开动画集
            else if (UIWanna != "InGame")
            {
                if (UITimer < 1.3f)
                {
                    UITimer += Time.deltaTime;
                    if (UITimer > 0.2f && InGame_ActionNum == 1)
                    {
                        UI_Level.rectTransform.DOAnchorPos3D(V3_Level_Out, 0.5f).SetEase(Ease.InBack);
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.3f && InGame_ActionNum == 2)
                    {
                        UI_HomeBtn.rectTransform.DOAnchorPos3D(V3_HomeBtn_Out, 0.5f).SetEase(Ease.InBack);
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.4f && InGame_ActionNum == 3)
                    {
                        if (Data.isSpecialLevel)
                            UI_ResetBtn.gameObject.SetActive(false);
                        else
                        {
                            UI_ResetBtn.gameObject.SetActive(true);
                            UI_ResetBtn.rectTransform.DOAnchorPos3D(V3_ResetBtn_Out, 0.5f).SetEase(Ease.InBack);
                        }
                        
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.5f && InGame_ActionNum == 4)
                    {
                        UI_BigBubble.rectTransform.DOScale(new Vector3(0.4f, 0.4f, 1), 1f).SetEase(Ease.OutBack);
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.51f)
                    {
                        if (UI_BigBubble.color.a > 0)
                        {
                            UI_BigBubble.color -= new Color(0, 0, 0, Time.deltaTime * 2f);
                            UI_PreImg.color -= new Color(0, 0, 0, Time.deltaTime * 2f);
                        }
                    }
                    if (UITimer > 0.6f && InGame_ActionNum == 5)
                    {
                        UI_BubbleBundle.rectTransform.DOScale(new Vector3(0.8f, 0.8f, 0.4f), 1f);
                        UI_BubbleBundle.rectTransform.DORotate(new Vector3(0, 0, 350), 1f);
                        InGame_ActionNum++;
                    }
                    if (UITimer > 0.61f)
                    {
                        InGame_BubbleDisplay = false;
                    }
                }
                else
                {
                    UITimer = 0;
                    UIState = UIWanna;
                }
            }
        }
        else if (UIState == "FailedGame")
        {
            if (UITimer < 2)
            {
                FailedBG.enabled = true;
                UI_BrokenFinger.enabled = true;
                UI_Ouch.enabled = true;

                UITimer += Time.deltaTime;
                if (UITimer > 1.8f)
                {
                    UI_TryAgainBtn.rectTransform.DOAnchorPos3D(v3_TryAgainBtn_In, 0.5f).SetEase(Ease.OutBack);
                    UITimer = 3;
                }
            }
        }
        else if (UIState == "FinishGame")
        {
            if (true)
            {
                UITimer += Time.deltaTime;
                if (FinishGame_ActionNum == 1)
                {
                    if (WhileScreen.color.a < 1)
                    {
                        if (UITimer > 0.08f)
                        {
                            WhileScreen.color += new Color(0, 0, 0, 0.4f);
                        }
                    }
                    else
                    {
                        UI_FinishBG.rectTransform.anchoredPosition = V3_FinishBG_In;
                        InGameReady();
                        BubbleReady();
                        FinishGame_ActionNum++;
                    }
                }
                else if (FinishGame_ActionNum == 2)
                {
                    if (WhileScreen.color.a > 0)
                    {
                        WhileScreen.color -= new Color(0, 0, 0, 1f * Time.deltaTime);
                    }
                    else
                    {
                        FinishGame_ActionNum++;
                    }
                }
                else if (FinishGame_ActionNum == 3)
                {
                    UI_BigLike.DOFade(1, 0.4f);
                    UI_EndWord.DOFade(1, 0.4f);
                    mySequence = DOTween.Sequence();
                    mySequence.Append(UI_BigLike.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.4f));
                    mySequence.Append(UI_BigLike.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.4f));
                    mySequence.Append(UI_BigLike.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.4f));
                    mySequence.Append(UI_BigLike.DOFade(0f, 0.4f));
                    mySequence.Append(UI_BigLike.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 1f));
                    mySequence.Append(UI_EndWord.DOFade(0f, 0.4f));
                    //.Append(UI_BigLike.DOFade(0, 1))

                    UI_LikeNumBG.rectTransform.DOAnchorPos3D(V3_LikeNumBG_In, 0.5f).SetEase(Ease.OutBack);


                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    FlowerLeft.GetComponent<ParticleSystem>().Play();
                    FlowerRight.GetComponent<ParticleSystem>().Play();
                    TapticManager.Impact(ImpactFeedback.Heavy);
                    TapticManager.Impact(ImpactFeedback.Heavy);
                    TapticManager.Impact(ImpactFeedback.Heavy);
                    TapticManager.Impact(ImpactFeedback.Heavy);
                    TapticManager.Impact(ImpactFeedback.Heavy);
                    TapticManager.Impact(ImpactFeedback.Heavy);

                    FinishGame_ActionNum++;


                }
                else if (FinishGame_ActionNum == 4)
                {
                    if (Data.Setting_Vibration==1)
                    {
                        TapticManager.Impact(ImpactFeedback.Heavy);
                    }
                    Data.LikeNum += 1+(int)((FinishGame_LikeWillGet - FinishGame_LikeNumForLevelDisplay) / 50f);
                    FinishGame_LikeNumForLevelDisplay += 1 + (int)((FinishGame_LikeWillGet - FinishGame_LikeNumForLevelDisplay) / 50f);
                    if ((FinishGame_LikeWillGet - FinishGame_LikeNumForLevelDisplay) < 1)
                    {
                        FinishGame_ActionNum++;
                    }
                }
                else if(FinishGame_ActionNum == 5)
                {

                    PlayerPrefs.SetInt("LikeNum", Data.LikeNum);
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                    UI_LikeNumX2Btn.rectTransform.DOAnchorPos3D(V3_LikeNumX2Btn_In, 0.5f).SetEase(Ease.OutBack);

                    FinishGame_ActionNum++;
                }
                else if (FinishGame_ActionNum == 6 && UITimer > 7f)
                {
                    UI_NextLevelBtn.rectTransform.DOAnchorPos3D(V3_NextLevelBtn_In, 0.5f).SetEase(Ease.OutBack);
                    FinishGame_ActionNum++;
                }

                else if (FinishGame_ActionNum == 8)
                {
                    UI_NextLevelBtn.rectTransform.DOAnchorPos3D(V3_NextLevelBtn_Out, 0.5f).SetEase(Ease.InBack);
                    UI_LikeNumX2Btn.rectTransform.DOAnchorPos3D(V3_LikeNumX2Btn_Out, 0.5f).SetEase(Ease.InBack);
                    FinishGame_ActionNum++;
                }
                

                else if (FinishGame_ActionNum == 9)
                {
                    if (Data.Setting_Vibration == 1)
                    {
                        TapticManager.Impact(ImpactFeedback.Heavy);
                    }
                    Data.LikeNum += 1 + (int)((FinishGame_LikeWillGet - FinishGame_LikeNumForLevelDisplay) / 50f);
                    FinishGame_LikeNumForLevelDisplay += 1 + (int)((FinishGame_LikeWillGet - FinishGame_LikeNumForLevelDisplay) / 50f);
                    if ((FinishGame_LikeWillGet - FinishGame_LikeNumForLevelDisplay) < 1)
                    {
                        FinishGame_ActionNum++;
                    }
                }
                else if (FinishGame_ActionNum == 10)
                {

                    PlayerPrefs.SetInt("LikeNum", Data.LikeNum);

                    UITimer = 0;
                    AllActionNumReset();
                    UIState = "InGame";
                    UIWanna = "InGame";
                    UI_FinishBG.rectTransform.DOAnchorPos3D(V3_FinishBG_Out, 0.5f).SetEase(Ease.InBack);
                    UI_NextLevelBtn.rectTransform.DOAnchorPos3D(V3_NextLevelBtn_Out, 0.5f).SetEase(Ease.InBack);

                    UI_LikeNumX2Btn.rectTransform.DOAnchorPos3D(V3_LikeNumX2Btn_Out, 0.5f).SetEase(Ease.InBack);

                    UI_LikeNumBG.rectTransform.DOAnchorPos3D(V3_LikeNumBG_Out, 0.5f).SetEase(Ease.InBack);

                    GameManger.Intance.NextLevel();
                    FinishGame_ActionNum++;
                }

            }
        }
        

    }
}
