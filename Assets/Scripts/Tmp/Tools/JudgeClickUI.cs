using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ZY;

public class JudgeClickUI : Singleton<JudgeClickUI>
{

	public bool IsClickUI()
	{
#if !UNITY_EDITOR
		if (Input.touchCount > 0)       //检测手值的触碰 && Input.GetTouch(0).phase == TouchPhase.Began
		{
			//Debug.Log("检测手指的触碰");
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))   //监视是否触碰UI
			{
				//Debug.Log("监视触碰UI");
				return true;
			}

			var screenPosition = Input.GetTouch(0).position;
			//实例化点击事件
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			//将点击位置的屏幕坐标赋值给点击事件
			eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

			List<RaycastResult> results = new List<RaycastResult>();
			//向点击处发射射线
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

			//Debug.Log("检测结果.." + results.Count);
			if (results.Count > 0)
				return true;
		}
		
#endif
		
#if UNITY_EDITOR
		if (EventSystem.current.IsPointerOverGameObject())
			return true;
#endif

		return false;
	}
}
