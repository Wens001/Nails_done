using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZY;

public class DataManager : Singleton<DataManager>
{
    public int LikeNum { get; private set; }

    public void Init()
    {
        LikeNum = PlayerPrefs.GetInt("LikeNum", 0);
    }

    public void ChangeLikeNum(int likeNum)
    {
        LikeNum += likeNum;
        LikeNum = LikeNum < 0 ? 0 : LikeNum;
        
        PlayerPrefs.SetInt("LikeNum", LikeNum);
    }

    public static string SwitchNumToString(int likeNum)
    {
        var likeNumTemp = Mathf.Floor(likeNum * 0.01f) * 0.1f;

        if (likeNum < 1000)
        {
            return likeNum.ToString();
        }
        else if (likeNum >= 1000 && likeNum < 100000)
        {
            return likeNumTemp.ToString("0.0") + "K";
        }
        else //if (Data.LikeNum >= 100000)
        {
            return likeNumTemp.ToString("n0") + "K";
        }
    }
}
