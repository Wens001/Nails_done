using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerAdsAdjust : MonoBehaviour 
{
	// Use this for initialization
	private void Start () 
	{
		if (Screen.height / (float)Screen.width < 2f) return;
		
		var tmp = transform as RectTransform;
		tmp.sizeDelta = new Vector2(tmp.sizeDelta.x, 250f);
	}
}
