using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoActiveFalse : MonoBehaviour
{
    public float AutoTime;

    private void OnEnable()
    {
        StartCoroutine(AutoFalse());
    }

    private IEnumerator AutoFalse()
    {
        yield return new WaitForSeconds(AutoTime);
        
        gameObject.SetActive(false);
    }
}
