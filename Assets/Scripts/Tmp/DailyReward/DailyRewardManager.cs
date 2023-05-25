using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZY;

public class DailyRewardManager : Singleton<DailyRewardManager>
{
    public List<DailyRewardData> ConfigList { get; } = new List<DailyRewardData>();
    
    public int DailyCount { get; private set; }

    public void Init()
    {
        DailyCount = PlayerPrefs.GetInt("DailyCount", 1);
        
        for (var i = 0; i < 9; i++)
        {
            var data = new DailyRewardData()
            {
                Count = 1000 + i * 500,
            };
            
            ConfigList.Add(data);
        }
    }

    public void JudgeShowView()
    {
        if (DailyCount > ConfigList.Count) return;
        
        var lastGetStr = PlayerPrefs.GetString("DailyRewardGetTime", "");
        if (!string.IsNullOrEmpty(lastGetStr))
        {
            var getDate = DateTime.Parse(lastGetStr);
            var date = DateTime.Now;
            if (date.Year != getDate.Year || date.Month != getDate.Month || date.Date != getDate.Date)
            {
                DailyRewardView.Instance.Show();
            }
        }
    }

    public void GetReward(bool isDouble)
    {
        DataManager.Instance.ChangeLikeNum(ConfigList[DailyCount - 1].Count * (isDouble ? 2 : 1));
        
        DailyCount++;
        
        PlayerPrefs.SetInt("DailyCount", DailyCount);
        PlayerPrefs.SetString("DailyRewardGetTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}

public class DailyRewardData
{
    public int Count;
}