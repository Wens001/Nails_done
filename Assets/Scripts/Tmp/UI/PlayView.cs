using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayView : MonoBehaviour
{
    public Text TitleTxt;

    public GameObject[] ProgressObjArray;

    private int _progressIndex;
    
    public static PlayView Instance { get; private set; }

    public void Init()
    {
        Instance = this;
        
        
    }

    public void ChangeProgress()
    {
        _progressIndex++;

        for (int i = 0, maxI = ProgressObjArray.Length; i < maxI; i++)
        {
            ProgressObjArray[i].SetActive(_progressIndex == i + 1);
        }
    }

    public void Show()
    {
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
