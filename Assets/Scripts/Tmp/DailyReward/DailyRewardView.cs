using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardView : MonoBehaviour
{
    public GameObject CellObj;

    public GameObject DoubleBtnObj;
    
    private List<DailyRewardCell> _cellList = new List<DailyRewardCell>();
    
    public static DailyRewardView Instance { get; private set; }

    public void Init()
    {
        Instance = this;

        var configList = DailyRewardManager.Instance.ConfigList;
        var parentTrans = CellObj.transform.parent;
        for (int i = 1, maxI = configList.Count; i <= maxI; i++)
        {
            var obj = Instantiate(CellObj, parentTrans);
            obj.name = "Cell" + i;

            var cell = obj.GetComponent<DailyRewardCell>();
            cell.Init(i, configList[i - 1]);
            
            _cellList.Add(cell);
        }

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        
        for (int i = 0, maxI = _cellList.Count; i < maxI; i++)
        {
            _cellList[i].RefreshView();
        }
    }

    public void OnClickOne()
    {
        DailyRewardManager.Instance.GetReward(false);
        
        Hide();
    }

    public void OnClickDouble()
    {
        DoubleBtnObj.SetActive(false);
        
        //AdsManager.Instance.ShowRewardedInterstitial(DelayCollectDouble);
    }

    private void DelayCollectDouble(bool isReward)
    {
        if (!isReward) return;

        DailyRewardManager.Instance.GetReward(true);

        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
