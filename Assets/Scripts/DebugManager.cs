using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static bool IsDebug;

    private void Awake()
    {
        LoadConfig();
    }
    
    public void LoadConfig()
    {
        var path = "";
#if UNITY_EDITOR || UNITY_IPHONE
        path = "file://" + Application.streamingAssetsPath + "/tmp.txt";
#else
        path =  Application.streamingAssetsPath + "/tmp.txt";
#endif

        StartCoroutine(DoLoad(path));
    }

        
    private IEnumerator DoLoad(string path)
    {
        var www = new WWW(path);
        yield return www;

        IsDebug = int.Parse(www.text.Trim()) == 1;

        if (IsDebug)
        {
            PlayerPrefs.SetInt("Level", GameManger.MAXNormalLevel);
            
            PlayerPrefs.SetInt("SpecialLevel1", 10000);
            PlayerPrefs.SetInt("SpecialLevel2", 10000);
            PlayerPrefs.SetInt("SpecialLevel3", 10000);
        }
    }
}
