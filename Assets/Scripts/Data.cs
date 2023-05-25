using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour
{

    public Toggle Sound_Toggle;
    public Toggle Vibration_Toggle;


    public void OnSettingChanged()
    {

        Setting_Sound = System.Convert.ToInt32(Sound_Toggle.isOn);

        PlayerPrefs.SetInt("Setting_Sound", Setting_Sound);
        

    }

    public void OnSettingVibrationChanged()
    {


        Setting_Vibration = System.Convert.ToInt32(Vibration_Toggle.isOn);


        PlayerPrefs.SetInt("Setting_Vibration", Setting_Vibration);


    }


    #region 要存档的内容

    /*玩家货币量*/
    public static int LikeNum = 0;


    /*设置界面——音效*/
    public static int Setting_Sound = 1;

   

    /*设置界面——震动*/
    public static int Setting_Vibration = 1;

    /*去广告*/
    public static int NoAd_isBought = 0; // 是否买了去广告

    /*关卡数*/
    public static int LEVEL = 1;

    public static int SpecialTheme1 = 0;
    public static int SpecialTheme2 = 0;
    public static int SpecialTheme3 = 0;

    public static int SpecialLevel1 = 0;

    public static int SpecialLevel2 = 0;

    public static int SpecialLevel3 = 0;

    public static int HandSkinSelect = 1;

    /*每日奖励计算*/
    public static int DailyCount = 1; //1203新增

    /*离线奖励 上次领取时间*/
    public static string LastOfflineRewardTime = ""; //1203新增





    #endregion


    public static bool isSpecialLevel = false;

    /*状态属性*/
    public static string NAILCLIPPERS = "NailClippers";
    public static string BRUSH = "Brush";


    /*标签属性*/
    public static string NAIL = "Nail";//指甲标签
    public static string NAILSHELL = "NailShell";//多余指甲标签
    public static string TOOL = "Tool";
    public static string HAND = "Hand";
    public static string HULL = "Hull";//碰撞网格标签
    public static string UI = "Ui";//UI
    public static string SHELLPARENT = "ShellParent";
    public static string BIGHAND = "BigHand";
        
    /*基础属性*/
    public static int HP = 3;
    
    public static int FingerId= 1;
    public static int LEVELPART = 1;
    public static float NEXTSTEPTIMER=6;
    public static float ONELAYERTIMER = 3;
    public static int isFirstOpen = 1;
    public static int isMenu=0;
    public static int isFirstClick=1;//判定是否第一次点击开始游戏按钮    

    public static int MAXLEVEL = 35;//关卡最大数量
    public static int Count = 0;
    public static int SHELLCOUNT;//多余指甲碎片的数量
    public static int NAILCOUNT = 5;//手指指甲的数量
    public static int COLLIDERCOUNT;//碰撞网格数
    public static int CHECKNUM = 9;//涂色满色块数据

    public static float BULLINGTIME = 1;//指甲碎片闪烁时间
    public static float RATE = (80.0f / 255.0f);//总速率
    public static float DAMGETIMER = 0.5f;//受伤变红时间
    public static float DAMAGERATE = (120.0f / 255.0f);//总速率
    public static float TIPRATE = (102.0f/255);//外层指甲显示速率
    public static float TIPTIMER = 1;//外层指甲闪烁时间

    /*坐标属性*/
    public static float LimitMax_Y = 0.26f;
    public static float LimitMin_Y = -0.036f;
    public static float LimitMin_X = -0.027f;
    public static float LimitMax_X = 0.130f;
    public static Vector3 IniPosition = new Vector3(0.055f, 0.109f, 0.158f);//摄像机修剪指甲位置
    public static Vector3 ShowPostion = new Vector3(1.1656f, -0.03f, -0.985f);//摄像机展示指甲
    public static Vector3 FingerInit = new Vector3(0.0598f, -0.0008f, 0.5889f);//单根手指初始位置指甲
    public static Vector3 MousePoint = new Vector3(0.0598f, -0.0008f, 0.5889f);//围绕旋转点
    public static Vector3 PicturePosition = new Vector3(-38.72f, 44.48f, -0.42f);//拍照位置
    public static Vector3 HomePosition = new Vector3(1.64f,31.75f,-13.1f);


    /**大拇指**/
    public static Vector3 ThumbFinger = new Vector3(-4.634f, -0.56971f, -7.51f);
    /**食指**/
    public static Vector3 IndexFinger = new Vector3(-0.509f, 1.514f, -17f);
    /**中指**/
    public static Vector3 MiddleFinger = new Vector3(2.86f, 1.845f, -17.77f);
    /**无名指**/
    public static Vector3 RingFinger = new Vector3(6.521f, 2.7f, -17);
    /**小指**/
    public static Vector3 LitterFinger = new Vector3(8.5979f, 3.1528f, -12.577f);  


    /**
     * 需修剪指甲数量
     * */
    public static int NailShellCount
    {
        get
        {
            Transform parent = GameObject.FindWithTag(SHELLPARENT).transform;
            return parent.childCount;
        }
    }

    private void Start()
    {
        switch (Setting_Sound)
        {
            case 0: Sound_Toggle.isOn = false; break;
            case 1: Sound_Toggle.isOn = true; break;
        };

        switch (Setting_Vibration)
        {
            case 0: Vibration_Toggle.isOn = false; break;
            case 1: Vibration_Toggle.isOn = true;  break;
        };
    }

    private void Awake()
    {
        LEVEL = PlayerPrefs.GetInt("Level", 1);
        SpecialLevel1 = PlayerPrefs.GetInt("SpecialLevel1", 0);
        SpecialLevel2 = PlayerPrefs.GetInt("SpecialLevel2", 0);
        SpecialLevel3 = PlayerPrefs.GetInt("SpecialLevel3", 0);

        SpecialTheme1 = PlayerPrefs.GetInt("SpecialTheme1", 0);
        SpecialTheme2 = PlayerPrefs.GetInt("SpecialTheme2", 0);
        SpecialTheme3 = PlayerPrefs.GetInt("SpecialTheme3", 0);
        


        Setting_Sound = PlayerPrefs.GetInt("Setting_Sound", 1);
        Setting_Vibration = PlayerPrefs.GetInt("Setting_Vibration", 1);
        NoAd_isBought = PlayerPrefs.GetInt("NoAd_isBought", 0);
        LikeNum = PlayerPrefs.GetInt("LikeNum", 0);
        HandSkinSelect = PlayerPrefs.GetInt("HandSkinSelect", 1);


        //------------------------------------------------------1203新增----------------------------------------------------------------------------

        DailyCount = PlayerPrefs.GetInt("DailyCount", 1); //1203新增

        if (PlayerPrefs.HasKey("LastOfflineRewardTime"))
        {
            LastOfflineRewardTime = PlayerPrefs.GetString("LastOfflineRewardTime", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //1203新增
        }
        else
        {
            PlayerPrefs.SetString("LastOfflineRewardTime", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            LastOfflineRewardTime = PlayerPrefs.GetString("LastOfflineRewardTime", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        Debug.Log("时间存档读到：" + LastOfflineRewardTime);

        //----------------------------------------------------------------------------------------------------------------------------------------


        UIControl.HandColor_Select = HandSkinSelect;

        //Debug.Log("读了"+ Setting_Sound+","+ Setting_Vibration);


       

    }


}
