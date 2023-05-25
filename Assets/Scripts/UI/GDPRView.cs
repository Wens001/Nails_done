using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GDPRView : MonoBehaviour
{
	public GameObject View1;
	
	public GameObject View2;
	
	public GameObject View3;

	public GameObject View4;

	public GameObject BtnSdkObj;
	public GameObject BtnSdkUnObj;
	
	public GameObject BtnAdsObj;
	public GameObject BtnAdsUnObj;

	public TMP_TextEventHandler TextEventHandler;
	
	private Action<bool> _callBack;
	private bool _isCanAds;
	private bool _isCanSdk;

	private bool _isReset;
	
	public int Status { get; private set; }		//-1：没有选择过	00：都不同意；	01：同意sdk，不同意ads； 10：不同意sdk，同意ads；  11：都同意
	public bool IsAgreeAds => (Status & 2) > 0;
	public bool IsAgreeSdk => (Status & 1) > 0;
	public bool IsShowTipView => Status >= 0 && (!IsAgreeAds || !IsAgreeSdk);

	private bool _isNeedHandleAds;
	
	public static GDPRView Instance { get; private set; }

	// Use this for initialization
	public void Init()
	{
		Instance = this;

		Hide();

		Status = PlayerPrefs.GetInt("GDPRStatus", -1);

		TextEventHandler.onLinkSelection.AddListener(OnClickLink);
		
		Debug.Log("GDPRStatus：" + Status);
	}

	public void OnClickSdk(bool isCan)
	{
		_isCanSdk = isCan;
		
		BtnSdkObj.SetActive(isCan);
		BtnSdkUnObj.SetActive(!isCan);
	}
	
	public void OnClickAds(bool isCan)
	{
		_isCanAds = isCan;
		
		BtnAdsObj.SetActive(isCan);
		BtnAdsUnObj.SetActive(!isCan);
	}

	public void OnClickAgree3()
	{
		if (!_isCanSdk || !_isCanAds)
		{
			OnClickNext(4);
			return;
		}

		OnClickAgree();
	}

	public void OnClickAgree()
	{
		Status = _isCanSdk ? 1 : 0;
		Status = _isCanAds ? Status | 2 : Status & 1;
		PlayerPrefs.SetInt("GDPRStatus", Status);
		
		Debug.Log("GDPRStatus：" + Status);
		
		_callBack.Invoke(_isCanAds);
		
		Hide();
		
		ADSDK.Intance.JudgeGDPRShow();
		
		if (_isNeedHandleAds) ADSDK.Intance.ShowCrossPromo();
	}

	public void OnClickNext(int step)
	{
		if (step == 1)
		{
			View1.SetActive(true);
			View2.SetActive(false);
			View3.SetActive(false);
			View4.SetActive(false);
			
			OnClickSdk(true);
			OnClickAds(true);
		}
		else if (step == 2)
		{
			View1.SetActive(false);
			View2.SetActive(true);
			View3.SetActive(false);
			View4.SetActive(false);

			if (_isReset)
			{
				OnClickSdk(true);
				OnClickAds(true);
			}
		}
		else if (step == 3)
		{
			View1.SetActive(false);
			View2.SetActive(false);
			View3.SetActive(true);
			View4.SetActive(false);
		}
		else //if (step == 4)
		{
			View1.SetActive(false);
			View2.SetActive(false);
			View3.SetActive(false);
			View4.SetActive(true);
		}
	}

	public void OnClickLink(string idStr, string url, int id)
	{
		Application.OpenURL(url);
	}

	public void Show(Action<bool> callBack, bool isReset = true, bool isHandleAds = true)
	{
		gameObject.SetActive(true);

		_callBack = callBack;
		_isReset = isReset;
		_isNeedHandleAds = isHandleAds;

		OnClickNext(_isReset ? 1 : 3);

		if (!_isReset)
		{
			OnClickSdk(IsAgreeSdk);
			OnClickAds(IsAgreeAds);
		}
		
		if (_isNeedHandleAds) ADSDK.Intance.HideCrossPromo();
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
