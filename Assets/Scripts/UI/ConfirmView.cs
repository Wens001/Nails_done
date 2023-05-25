using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmView : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void OnClick()
    {
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
