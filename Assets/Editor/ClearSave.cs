using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClearSave : MonoBehaviour 
{
	[MenuItem("ClearSave/Clear")]
	public static void ClearPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
}
