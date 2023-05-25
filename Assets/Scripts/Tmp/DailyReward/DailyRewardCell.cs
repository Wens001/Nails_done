using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardCell : MonoBehaviour
{
    public Text TitleTxt;
    public Text ValueTxt;

    public GameObject SelectedObj;

    private int _index;
    private DailyRewardData _data;

    public void Init(int index, DailyRewardData data)
    {
        _index = index;
        _data = data;

        TitleTxt.text = "DAY" + _index;
        ValueTxt.text = _data.Count.ToString();
    }

    public void RefreshView()
    {
        SelectedObj.SetActive(_index == DailyRewardManager.Instance.DailyCount);
    }
}
