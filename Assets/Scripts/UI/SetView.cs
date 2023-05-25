using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;
using UnityEngine.UI;

public class SetView : MonoBehaviour
{
    public Image SettingLang;

    public ConfirmView ConfirmView;
    
    public static SetView Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        
        ConfirmView.Hide();
    }

    private void Start()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.Chinese:
                SettingLang.sprite = Resources.Load<Sprite>("Sprites/SettingLang/Connect_CNS");
                break;
            case SystemLanguage.ChineseTraditional:
                SettingLang.sprite = Resources.Load<Sprite>("Sprites/SettingLang/Connect_CNT");
                break;
            case SystemLanguage.German:
                SettingLang.sprite = Resources.Load<Sprite>("Sprites/SettingLang/Connect_DE");
                break;
            case SystemLanguage.Spanish:
                SettingLang.sprite = Resources.Load<Sprite>("Sprites/SettingLang/Connect_ES");
                break;
            case SystemLanguage.Japanese:
                SettingLang.sprite = Resources.Load<Sprite>("Sprites/SettingLang/Connect_JP");
                break;
            case SystemLanguage.Korean:
                SettingLang.sprite = Resources.Load<Sprite>("Sprites/SettingLang/Connect_KR");
                break;
            case SystemLanguage.Russian:
                SettingLang.sprite = Resources.Load<Sprite>("Sprites/SettingLang/Connect_RU");
                break;
            default:
                SettingLang.sprite = Resources.Load<Sprite>("Sprites/SettingLang/Connect_EN");
                break;
        }
        
        SettingLang.SetNativeSize();
    }

    public void OnClickFacebook()
    {
        Application.OpenURL("https://www.facebook.com/lionstudios.cc/");
    }
    
    public void OnClickYoutube()
    {
        Application.OpenURL("https://www.youtube.com/lionstudioscc");
    }
    
    public void OnClickInstagram()
    {
        Application.OpenURL("https://www.instagram.com/lionstudioscc/");
    }
    
    public void OnClickTwitter()
    {
        Application.OpenURL("https://twitter.com/LionStudiosCC");
    }
    
    public void OnClickMainWebsite()
    {
        Application.OpenURL("https://lionstudios.cc");
    }

    public void OnClickGDPRBtn()
    {
        GDPRView.Instance.Show(ADSDK.Intance.CallBackConsent, false, false);
    }
    
    public void OnClickRestoreBtn()
    {
        IOSPurchaseManager.Instance.RestorePurchases();
    }

    public void ShowConfirmView()
    {
        ConfirmView.Show();
    }
}
