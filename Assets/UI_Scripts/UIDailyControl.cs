using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDailyControl : MonoBehaviour
{
    public static UIDailyControl Intance;

    public static int OfflineRewardCount = 0; //计算离线奖励


    public Image UI_DailyModel;
    public Image UI_OfflineModel;

    public GameObject UI_DRGetBtn;
    public GameObject UI_ORGetBtn;

    public Image UI_D1_L;
    public Image UI_D1_D;
    public Image UI_D2_L;
    public Image UI_D2_D;
    public Image UI_D3_L;
    public Image UI_D3_D;
    public Image UI_D4_L;
    public Image UI_D4_D;
    public Image UI_D5_L;
    public Image UI_D5_D;
    public Image UI_D6_L;
    public Image UI_D6_D;
    public Image UI_D7_L;
    public Image UI_D7_D;
    public Image UI_D8_L;
    public Image UI_D8_D;
    public Image UI_D9_L;
    public Image UI_D9_D;

    public Text UIText_ORCanGet;




    public int GetOfflineDays()
    {
        System.DateTime Temp = System.DateTime.Parse(Data.LastOfflineRewardTime);


        int DayTemp = System.DateTime.Now.Day - Temp.Day;
        Debug.Log("获取到离线天数：" + DayTemp);

        return DayTemp;
    }

    public int GetOfflineMinutes()
    {
        System.DateTime Temp = System.DateTime.Parse(Data.LastOfflineRewardTime);
        System.TimeSpan Span = System.DateTime.Now - Temp;

        int Reward = (int)Span.TotalMinutes;

        Debug.Log("获取到离线分钟:" + Reward);
        Debug.Log("获取到离线秒" + (int)Span.TotalSeconds);

        return Reward;
    }

    public void RewardTimeRefresh()
    {
        //用于存盘
        Data.LastOfflineRewardTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        PlayerPrefs.SetString("LastOfflineRewardTime", Data.LastOfflineRewardTime);
        PlayerPrefs.SetInt("DailyCount", Data.DailyCount);
        PlayerPrefs.SetInt("LikeNum", Data.LikeNum);
        Debug.Log("时间存档更新");
    }


    void UI_DRGetBtn_DisPlay()
    {
        UI_DRGetBtn.gameObject.SetActive(true);
        //Debug.Log("蛋蛋蛋");
    }


    void UI_ORGetBtn_DisPlay()
    {
        UI_ORGetBtn.gameObject.SetActive(true);
    }




    public void Btn_GetDailyReward()
    {
        //每日奖励事件

        UI_DailyModel.gameObject.SetActive(false);
        //Data.DailyCount++;
        getDailyReward(false);
        ShowOfflineReward();
    }

    public void Btn_GetDailyRewardDouble()
    {
        //每日奖励事件 加倍
        //激励广告
        ADRewardControl.RewardType = 1;
        ADSDK.Intance.showRewardADs("double_dailyreward");

    }

    public void Btn_GetOfflineRewark()
    {
        //离线奖励事件

        UI_OfflineModel.gameObject.SetActive(false);
        Data.LikeNum += OfflineRewardCount;

        
        RewardTimeRefresh();
        
        ADSDK.Intance.ShowCrossPromo();
    }

    public void Btn_GetOfflineRewarkDouble()
    {
        //离线奖励事件 加倍
        //激励广告
        ADRewardControl.RewardType = 2;
        ADSDK.Intance.showRewardADs("double_offline");

    }




    public void ShowDailyReward()
    {
        if(GetOfflineDays() > 0 || Data.DailyCount ==1)
        {
            
            UI_DailyModel.gameObject.SetActive(true);
            DailyBoxControl();
            Invoke("UI_DRGetBtn_DisPlay", 6);
            
            ADSDK.Intance.HideCrossPromo();
        }
        else
        {
            ShowOfflineReward();
        }
    }

    public void ShowOfflineReward()
    {
        //30分钟开始有离线奖励
        if (GetOfflineMinutes() > 30)
        {
            OfflineRewardCount = 100 + ( GetOfflineMinutes() - 30 ) * 4;
            if (OfflineRewardCount > 2000)
            {
                OfflineRewardCount = 2000;
            }
            Debug.Log("离线奖励:" + OfflineRewardCount);

            UIText_ORCanGet.text = "× " + OfflineRewardCount;

            UI_OfflineModel.gameObject.SetActive(true);
            Invoke("UI_ORGetBtn_DisPlay", 3);
            
            ADSDK.Intance.HideCrossPromo();
        }
        else
        {
            RewardTimeRefresh();
        }
    }


    void DailyBoxReset()
    {
        UI_D1_L.enabled = false;
        UI_D1_D.enabled = false;
        UI_D2_L.enabled = false;
        UI_D2_D.enabled = false;
        UI_D3_L.enabled = false;
        UI_D3_D.enabled = false;
        UI_D4_L.enabled = false;
        UI_D4_D.enabled = false;
        UI_D5_L.enabled = false;
        UI_D5_D.enabled = false;
        UI_D6_L.enabled = false;
        UI_D6_D.enabled = false;
        UI_D7_L.enabled = false;
        UI_D7_D.enabled = false;
        UI_D8_L.enabled = false;
        UI_D8_D.enabled = false;
        UI_D9_L.enabled = false;
        UI_D9_D.enabled = false;
    }

    public void DailyBoxControl()
    {
        DailyBoxReset();

        if (Data.DailyCount == 1)
        {
            UI_D1_L.enabled = true;
        }
        else if(Data.DailyCount == 2)
        {
            UI_D1_D.enabled = true;
            UI_D2_L.enabled = true;
        }
        else if (Data.DailyCount == 3)
        {
            UI_D1_D.enabled = true;
            UI_D2_D.enabled = true;
            UI_D3_L.enabled = true;
        }
        else if (Data.DailyCount == 4)
        {
            UI_D1_D.enabled = true;
            UI_D2_D.enabled = true;
            UI_D3_D.enabled = true;
            UI_D4_L.enabled = true;
        }
        else if (Data.DailyCount == 5)
        {
            UI_D1_D.enabled = true;
            UI_D2_D.enabled = true;
            UI_D3_D.enabled = true;
            UI_D4_D.enabled = true;
            UI_D5_L.enabled = true;
        }
        else if (Data.DailyCount == 6)
        {
            UI_D1_D.enabled = true;
            UI_D2_D.enabled = true;
            UI_D3_D.enabled = true;
            UI_D4_D.enabled = true;
            UI_D5_D.enabled = true;
            UI_D6_L.enabled = true;
        }
        else if (Data.DailyCount == 7)
        {
            UI_D1_D.enabled = true;
            UI_D2_D.enabled = true;
            UI_D3_D.enabled = true;
            UI_D4_D.enabled = true;
            UI_D5_D.enabled = true;
            UI_D6_D.enabled = true;
            UI_D7_L.enabled = true;
        }
        else if (Data.DailyCount == 8)
        {
            UI_D1_D.enabled = true;
            UI_D2_D.enabled = true;
            UI_D3_D.enabled = true;
            UI_D4_D.enabled = true;
            UI_D5_D.enabled = true;
            UI_D6_D.enabled = true;
            UI_D7_D.enabled = true;
            UI_D8_L.enabled = true;
        }
        else if (Data.DailyCount == 9)
        {
            UI_D1_D.enabled = true;
            UI_D2_D.enabled = true;
            UI_D3_D.enabled = true;
            UI_D4_D.enabled = true;
            UI_D5_D.enabled = true;
            UI_D6_D.enabled = true;
            UI_D7_D.enabled = true;
            UI_D8_D.enabled = true;
            UI_D9_L.enabled = true;
        }

    }

    public void getDailyReward(bool isDouble)
    {
        if (Data.DailyCount == 1)
        {
            Data.LikeNum += 1000 * (isDouble ? 2 : 1);
        }
        else if (Data.DailyCount == 2)
        {
            Data.LikeNum += 1500 * (isDouble ? 2 : 1);
        }
        else if (Data.DailyCount == 3)
        {
            Data.LikeNum += 2000 * (isDouble ? 2 : 1);
        }
        else if (Data.DailyCount == 4)
        {
            Data.LikeNum += 2500 * (isDouble ? 2 : 1);
        }
        else if (Data.DailyCount == 5)
        {
            Data.LikeNum += 3000 * (isDouble ? 2 : 1);
        }
        else if (Data.DailyCount == 6)
        {
            Data.LikeNum += 3500 * (isDouble ? 2 : 1);
        }
        else if (Data.DailyCount == 7)
        {
            Data.LikeNum += 4000 * (isDouble ? 2 : 1);
        }
        else if (Data.DailyCount == 8)
        {
            Data.LikeNum += 4500 * (isDouble ? 2 : 1);
        }
        else if (Data.DailyCount == 9)
        {
            Data.LikeNum += 5000 * (isDouble ? 2 : 1);
        }

        Data.DailyCount++;
        
        ADSDK.Intance.ShowCrossPromo();
    }





    // Start is called before the first frame update
    void Start()
    {
        //DailyBoxReset();

        UI_DailyModel.gameObject.SetActive(false);
        UI_OfflineModel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Intance = this;
    }
}
