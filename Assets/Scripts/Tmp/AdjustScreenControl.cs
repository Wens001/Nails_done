using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 调整摄像机的orthographicSize，来达到适应屏幕的效果
/// </summary>
public class AdjustScreenControl : MonoBehaviour
{
	public bool IsWidth = true;
	public bool IsHeight = true;

	public int ResizeWidth = 1080;
	public int ResizeHeight = 1920;

	private static AdjustScreenControl _instance;

	public static AdjustScreenControl Instance => _instance;

	private static float _trueWidth;
	private static float _trueHeight;

	public static float TrueWidth => _trueWidth;

	public static float TrueHeight => _trueHeight;

	public float OffsetWidthHalf => (_trueWidth - ResizeWidth) / 2;	
	public float OffsetHeightHalf => (_trueHeight - ResizeHeight) / 2;
	public float OffsetWidth => _trueWidth - ResizeWidth;	
	public float OffsetHeight => _trueHeight - ResizeHeight;

	public void Init()
	{
		_instance = this;
		
		var scaleWidth = (float) ResizeWidth / Screen.width;
		var scaleHeight = (float) ResizeHeight / Screen.height;
		if (scaleWidth > scaleHeight || IsWidth && !IsHeight)
		{
			_trueWidth = ResizeWidth;
			_trueHeight = Screen.height * scaleWidth;
			
		}
		else
		{
			_trueWidth = Screen.width * scaleHeight;
			_trueHeight = ResizeHeight;
		}

		//GetComponent<Camera>().orthographicSize = _trueHeight / 2;
		
		Debug.Log(_trueWidth + "," + _trueHeight);
	}
}
