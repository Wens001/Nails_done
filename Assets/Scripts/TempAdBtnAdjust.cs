using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempAdBtnAdjust : MonoBehaviour
{
    private Button _btn;
    private UIBreathEffect _effect;
    
    // Start is called before the first frame update
    private void Start()
    {
        _btn = GetComponent<Button>();
        _effect = GetComponent<UIBreathEffect>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var isComplete = ADSDK.Intance.IsLoadRewardedVideoComplete;

        if (_btn) _btn.interactable = isComplete;
        if (_effect) _effect.enabled = isComplete;
    }
}
