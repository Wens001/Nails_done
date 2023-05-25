using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{

    public static RenderTexture renderTexture;
    private static Material nailMat;
    private static Material resetMat;
    public static RenderTexture renderNailShell;
    //初始化
    void Start()
    {

        ClearRenderTxtrue();
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
    }

    void Update()
    {

    }

    public static void ClearRenderTxtrue()
    {
        resetMat = ObjectData.MaterialRegister(ObjectData.RESETMAT);
        for (int i = 1; i <= 5; i++)
        {
            renderTexture = ObjectData.RenderTextrueRegister(i);
            Graphics.Blit(null, renderTexture, resetMat);
            Graphics.Blit(null, renderTexture, resetMat);
            Graphics.Blit(null, renderTexture, resetMat);
            Graphics.Blit(null, renderTexture, resetMat);
            Graphics.Blit(null, renderTexture, resetMat);
            Graphics.Blit(null, renderTexture, resetMat);
            Graphics.Blit(null, renderTexture, resetMat);
            Graphics.Blit(null, renderTexture, resetMat);
            Graphics.Blit(null, renderTexture, resetMat);
            Graphics.Blit(null, renderTexture, resetMat);
            nailMat = ObjectData.GetNailMat(i);

        }
        Graphics.Blit(null, renderNailShell, resetMat);//刷子贴
    }

    public static void ClearTextrueByKey(int fingerId)
    {
        resetMat = ObjectData.MaterialRegister(ObjectData.RESETMAT);
        renderTexture = ObjectData.RenderTextrueRegister(fingerId);
        Graphics.Blit(null, renderTexture, resetMat);
        nailMat = ObjectData.GetNailMat(fingerId);

        for (int i = 1; i <= 10; i++)
        {
            Graphics.Blit(null, renderTexture, resetMat);//刷子贴
        }
       


    }
}
