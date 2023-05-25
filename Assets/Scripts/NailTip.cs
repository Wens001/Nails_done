using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailTip : MonoBehaviour
{
    private bool isTiping=true;
    public static bool isRun=false;
    public static float tipTimer;
    public static Material outNailMat;


    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

        if (isTiping&&isRun)
        {
            tipTimer += Time.deltaTime;
            outNailMat.SetColor("_Color", new Color(0.4f, 0.4f, 0.4f, Data.RATE * (tipTimer / Data.TIPTIMER)));
            if (tipTimer >= Data.TIPTIMER)
            {
                isTiping = false;
            }
        }
        if (!isTiping&&isRun)
        {
            tipTimer -= Time.deltaTime;
            outNailMat.SetColor("_Color", new Color(0.4f, 0.4f, 0.4f, Data.RATE * (tipTimer / Data.TIPTIMER)));
            if (tipTimer <= 0)
            {
                isTiping = true;

            }
        }
    }

    public static void Clear()
    {
        outNailMat.SetColor("_Color", new Color(1, 1, 1, 0));
        isRun = false;
    }
}
