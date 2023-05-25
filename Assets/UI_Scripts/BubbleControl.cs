using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleControl : MonoBehaviour
{
    public static bool isDea = false;


    public Image Bubble1;
    public Image Bubble2;
    public Image Bubble3;
    public Image Bubble4;

    private bool B1 = false;
    private bool B2 = false;
    private bool B3 = false;
    private bool B4 = false;

    private Color col1;
    private Color col2;
    private Color col3;
    private Color col4;

    private Sprite _brush;
    private Sprite _diamond;


    // Start is called before the first frame update
    void Start()
    {
        _brush = Resources.Load<Sprite>("game_bubble_3");
        _diamond = Resources.Load<Sprite>("game_bubble_4");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDea)
        {
            if (UIControl.InGame_SumStep >= 4)
            {
                Bubble3.sprite = _brush;
                Bubble4.sprite = _diamond;
            }
            else
            {
                Bubble3.sprite = _diamond;
            }
        }
        else
        {
            Bubble4.sprite = _brush;
            Bubble3.sprite = _brush;
        }

        Bubble1.rectTransform.localScale = new Vector3(0.38f* (1 - Mathf.Abs((180-gameObject.transform.eulerAngles.z) * 0.007f)),
            0.38f *  (1 - Mathf.Abs((180-gameObject.transform.eulerAngles.z) * 0.007f)), 1);

        Bubble2.rectTransform.localScale = new Vector3(0.38f * (1 - Mathf.Abs((146 - gameObject.transform.eulerAngles.z) * 0.007f)),
            0.38f * (1 - Mathf.Abs((146 - gameObject.transform.eulerAngles.z) * 0.007f)), 1);

        Bubble3.rectTransform.localScale = new Vector3(0.38f * (1 - Mathf.Abs((112 - gameObject.transform.eulerAngles.z) * 0.007f)),
            0.38f * (1 - Mathf.Abs((112 - gameObject.transform.eulerAngles.z) * 0.007f)), 1);

        Bubble4.rectTransform.localScale = new Vector3(0.38f * (1 - Mathf.Abs((78 - gameObject.transform.eulerAngles.z) * 0.007f)),
            0.38f * (1 - Mathf.Abs((78 - gameObject.transform.eulerAngles.z) * 0.007f)), 1);


        col1 = new Color(1, 1, 1, (1 - Mathf.Abs((180 - gameObject.transform.eulerAngles.z) * 0.009f)));
        col2 = new Color(1, 1, 1, (1 - Mathf.Abs((146 - gameObject.transform.eulerAngles.z) * 0.009f)));
        col3 = new Color(1, 1, 1, (1 - Mathf.Abs((112 - gameObject.transform.eulerAngles.z) * 0.009f)));
        col4 = new Color(1, 1, 1, (1 - Mathf.Abs((78  - gameObject.transform.eulerAngles.z) * 0.009f)));


        if (UIControl.InGame_BubbleDisplay == true)
        {
            if(B1 == false)
            {
                if (Bubble1.color.a < col1.a)
                {
                    Bubble1.color += new Color(0, 0, 0, Time.deltaTime);
                }
                else
                {
                    B1 = true;
                }
            }
            else
            {
                Bubble1.color = col1;
            }

            if (UIControl.InGame_SumStep >= 2)
            {
                if (B2 == false)
                {
                    if (Bubble2.color.a < col2.a)
                    {
                        Bubble2.color += new Color(0, 0, 0, Time.deltaTime);
                    }
                    else
                    {
                        B2 = true;
                    }
                }
                else
                {
                    Bubble2.color = col2;
                }
            }


            if (UIControl.InGame_SumStep >= 3)
            {
                if (B3 == false)
                {
                    if (Bubble3.color.a < col3.a)
                    {
                        Bubble3.color += new Color(0, 0, 0, Time.deltaTime);
                    }
                    else
                    {
                        B3 = true;
                    }
                }
                else
                {
                    Bubble3.color = col3;
                }
            }

            if (UIControl.InGame_SumStep >= 4)
            {
                if (B4 == false)
                {
                    if (Bubble4.color.a < col4.a)
                    {
                        Bubble4.color += new Color(0, 0, 0, Time.deltaTime);
                    }
                    else
                    {
                        B4 = true;
                    }
                }
                else
                {
                    Bubble4.color = col4;
                }
            }
        }
        else
        {
            if (Bubble1.color.a > 0)
            {
                Bubble1.color -= new Color(0, 0, 0, 2f * Time.deltaTime);
            }
            if (Bubble2.color.a > 0)
            {
                Bubble2.color -= new Color(0, 0, 0, 2f * Time.deltaTime);
            }
            if (Bubble3.color.a > 0)
            {
                Bubble3.color -= new Color(0, 0, 0, 2f * Time.deltaTime);
            }
            if (Bubble4.color.a > 0)
            {
                Bubble4.color -= new Color(0, 0, 0, 2f * Time.deltaTime);
            }
        }

    }
}
