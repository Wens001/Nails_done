using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewAdjustPos : MonoBehaviour
{
    public Transform ContentTrans;

    private IEnumerator Start()
    {
        yield return null;
        
        var pos = ContentTrans.localPosition;
        pos.y += AdjustScreenControl.Instance.OffsetHeightHalf;

        if (AdjustScreenControl.TrueHeight / AdjustScreenControl.TrueWidth > 2f)	//认定是刘海屏
        {
            pos.y -= 90;
        }
		
        ContentTrans.localPosition = pos;
    }
}