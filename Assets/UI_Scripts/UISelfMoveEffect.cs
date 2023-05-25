using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TapticPlugin;

public class UISelfMoveEffect : MonoBehaviour
{
    public int Speed = 3;
    public float Intensity = 5;

    public Image Myself;

    private float MyPos = 0;
    private float timer = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        MyPos = Myself.rectTransform.anchoredPosition.y;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        timer += 2f*Time.deltaTime;
        Myself.rectTransform.anchoredPosition = new Vector3(Myself.rectTransform.anchoredPosition.x,MyPos + 4f * Intensity * Mathf.Sin(timer * Speed));
        if (timer >= 0.67f * Mathf.PI)
        {
            timer = 0;
            if (UIControl.InGame_NextStepTips == true)
            {
                if (Data.Setting_Vibration == 1)
                {
                    TapticManager.Impact(ImpactFeedback.Heavy);
                  
                }
            }
        }
        */

        if (UIControl.InGame_NextStepTips == true)
        {
            gameObject.GetComponent<Button>().interactable = true;
            if (Myself.color.a < 1)
            {
                Myself.color += new Color(0, 0, 0, 5 * Time.deltaTime);
            }
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
            if (Myself.color.a > 0)
            {
                Myself.color -= new Color(0, 0, 0, 5 * Time.deltaTime);
            }
        }

    }
}
