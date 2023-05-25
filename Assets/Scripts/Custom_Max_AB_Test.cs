#define iOS_Enabled
#define Android_Enabled

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom_Max_AB_Test : MonoBehaviour
{
    public static Custom_Max_AB_Test instance;
    [HideInInspector]
    public string debugLogString;
    public UnityEngine.UI.Text Unity_debug_label;
    // public TMPro.TextMeshProUGUI TM_debug_label;
    
    void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        #if (UNITY_IOS && iOS_Enabled) || (UNITY_ANDROID && Android_Enabled)
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            OnMaxSdkInitizalized ();
        };
        #endif
    }

    void OnMaxSdkInitizalized ()
    {
        #if (UNITY_IOS && iOS_Enabled) || (UNITY_ANDROID && Android_Enabled)
        ApplyValues ();
        #endif
    }

    void Mapvalue (string sourceString, ref float targetVariable)
    {
#if (UNITY_IOS && iOS_Enabled) || (UNITY_ANDROID && Android_Enabled)
        string str = MaxSdk.VariableService.GetString (sourceString, "");
        if (!string.IsNullOrEmpty (str))
        {
            float.TryParse (str, out targetVariable);
            PlayerPrefs.SetFloat ("max" + sourceString, targetVariable);
            debugLogString += "\n";
            debugLogString += ("fromServer :  " + sourceString + " : " + targetVariable);
            
        }
        else
        {
            targetVariable = PlayerPrefs.GetFloat ("max" + sourceString, targetVariable);
            debugLogString += "\n";
            debugLogString += ("fromCache :  " + sourceString + " : " + targetVariable);
        }

#endif
    }
    void Mapvalue (string sourceString, ref int targetVariable)
    {
#if (UNITY_IOS && iOS_Enabled) || (UNITY_ANDROID && Android_Enabled)
        string str = MaxSdk.VariableService.GetString (sourceString, "");
        if (!string.IsNullOrEmpty (str))
        {
            int.TryParse (str, out targetVariable);
            PlayerPrefs.SetInt ("max" + sourceString, targetVariable);
            debugLogString += "\n";
            debugLogString += ("fromServer :  " + sourceString + " : " + targetVariable);
        }
        else
        {
            targetVariable = PlayerPrefs.GetInt ("max" + sourceString, targetVariable);
            debugLogString += "\n";
            debugLogString += ("fromCache :  " + sourceString + " : " + targetVariable);
        }
#endif
    }

    public void ApplyValues ()
    {
#if (UNITY_IOS && iOS_Enabled) || (UNITY_ANDROID && Android_Enabled)

        MaxSdk.VariableService.LoadVariables ();

        debugLogString += ("DebugLog:");

        //Add variables here.
        //Mapvalue ("interstitial_delay", ref ADSDK.DURATION);  //指向出现插屏广告的等待时长
        //Mapvalue ("min_level_to_show_ad", ref ADSDK.MIN_LEVEL_SHOW_AD);   //指向出现插屏广告的最低关卡数

        Mapvalue ("ad_frequency", ref ADSDK.ad_frequency);   //指向出现插屏广告的最低关卡数
        //Mapvalue ("experiment_group", ref ADSDK.variableValue);   //指向出现插屏广告的最低关卡数

        if (Unity_debug_label != null)
            Unity_debug_label.text = debugLogString;

        // if (TM_debug_label != null)
        //     TM_debug_label.text = debugLogString;
#endif
    }

}