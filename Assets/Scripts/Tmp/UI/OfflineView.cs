using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OfflineView : MonoBehaviour
{
    public Text Txt;
    
    public GameObject DoubleBtnObj;
    
    private int _revenue;
    
    public static OfflineView Instance { get; private set; }

    public void Init()
    {
        Instance = this;

        Hide();
    }

    public void Show(int offlineTime)
    {
        gameObject.SetActive(true);
        
        _revenue = 100 + (offlineTime - 30 ) * 4;
        _revenue = _revenue > 2000 ? 2000 : _revenue;
        Txt.text = _revenue.ToString();
        
        DOTween.To(f => { Txt.text = "x" + ((int)f).ToString(); }, 0, _revenue, 1f);
    }
    
    public void OnClickCollect()
    {
        DataManager.Instance.ChangeLikeNum(_revenue);
        
        PlayerPrefs.DeleteKey("LastOfflineRewardTime");

        Hide();
    }

    public void OnClickCollectDouble()
    {
        DoubleBtnObj.SetActive(false);
        
        //AdsManager.Instance.ShowRewardedInterstitial(DelayCollectDouble);
    }

    private void DelayCollectDouble(bool isReward)
    {
        if (!isReward) return;
        
        DataManager.Instance.ChangeLikeNum(_revenue * 2);
        
        PlayerPrefs.DeleteKey("LastOfflineRewardTime");

        Hide();
        
        //StatsManager.Instance.LogEvent("AdsShowRewardedOffline");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}