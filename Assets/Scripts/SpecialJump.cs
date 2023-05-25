using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpecialJump : MonoBehaviour
{
    public void JumpLevel()
    {
        //Todo图鉴退出



        int level = int.Parse(transform.name);
        if (level== 0)
            return;
        
        if (PlayerPrefs.GetInt("SpecialTheme"+ level.ToString(), 1) == 0)
        {
            ADRewardControl.RewardType = level +5 ;
            ADSDK.Intance.showRewardADs("");
        }
        else
        {
            UIControl.Collect_Select = level;
            BookScript.RefreshSwitch = true;
        }
    }
}
