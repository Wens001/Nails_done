using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : MonoBehaviour
{
    public Text LikeNumTxt;
    
    public static StartView Instance { get; private set; }

    public void Init()
    {
        Instance = this;
        
        
    }

    public void RefreshView()
    {
        LikeNumTxt.text = DataManager.SwitchNumToString(DataManager.Instance.LikeNum);
    }
}
