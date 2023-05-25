using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnApplicationQuit()
    {
        SaveDate();
    }
	
    private void OnApplicationPause(bool isPause)
    {
        if (isPause)
            SaveDate();
        else
            JudgeOfflineRevenue();
    }

    private void JudgeOfflineRevenue()
    {
        var lastQuitTimeStr = PlayerPrefs.GetString("LastOfflineRewardTime", "");
        if (!string.IsNullOrEmpty(lastQuitTimeStr))
        {
            var lastQuitTime = DateTime.Parse(lastQuitTimeStr);

            var timeOffset = (int) DateTime.Now.Subtract(lastQuitTime).TotalMinutes;
            if (timeOffset > 30)
            {
                OfflineView.Instance.Show(timeOffset);
            }
            else
            {
                PlayerPrefs.DeleteKey("LastOfflineRewardTime");
				
                OfflineView.Instance.Hide();
            }
        }
        else
        {
            OfflineView.Instance.Hide();
        }
    }

    private void SaveDate()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("LastOfflineRewardTime", ""))) return;

        PlayerPrefs.SetString("LastOfflineRewardTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}
