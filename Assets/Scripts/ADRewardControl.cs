using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADRewardControl : MonoBehaviour
{

    public static int RewardType = 0; // 1 每日奖励双倍 2 离线奖励双倍 3 结算奖励双倍 4 跳关 5 解锁主题


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------激励广告兑现-------------------------------------------------------------------------------------------



        if (ADSDK.RewardADsSuccess == true)
        {
            if (RewardType == 1)
            {
                //每日奖励双倍
                UIDailyControl.Intance.UI_DailyModel.gameObject.SetActive(false);
                //Data.DailyCount++;
                UIDailyControl.Intance.getDailyReward(true);
                UIDailyControl.Intance.ShowOfflineReward();
            }
            else if (RewardType == 2)
            {
                //离线奖励双倍
                UIDailyControl.Intance.UI_OfflineModel.gameObject.SetActive(false);
           
                Data.LikeNum += UIDailyControl.OfflineRewardCount * 2;
                
                UIDailyControl.Intance.RewardTimeRefresh();
                
                ADSDK.Intance.ShowCrossPromo();
            }
            else if (RewardType == 3)
            {
                //结算奖励双倍
                UIControl.FinishGame_LikeWillGet *= 2;
                UIControl.FinishGame_ActionNum = 8;
            }
            else if (RewardType == 4)
            {
                //跳关
                GameManger.Intance.Reward_JumpLevel();
            }
            else if (RewardType == 5)
            {
                //获取赞
                Data.LikeNum += 500;
                BookScript.RefreshSwitch = true;

                PlayerPrefs.SetInt("LikeNum", Data.LikeNum);
            }

            else if (RewardType == 6)
            {
                //解锁主题1
                PlayerPrefs.SetInt("SpecialTheme1", 1);
                BookScript.RefreshSwitch = true;

            }
            else if (RewardType == 7)
            {
                //解锁主题2
                PlayerPrefs.SetInt("SpecialTheme2", 1);
                BookScript.RefreshSwitch = true;

            }
            else if (RewardType == 8)
            {
                //解锁主题3
                PlayerPrefs.SetInt("SpecialTheme3", 1);
                BookScript.RefreshSwitch = true;

            }

            RewardType = 0; //激励请求复位
            ADSDK.RewardADsSuccess = false; //完成标记复位
        }
    }
}
