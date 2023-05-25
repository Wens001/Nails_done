using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Decal : MonoBehaviour
{
    private  static Material brushMat;
    public  Texture texture;
    public static Decal Instance;
    public Image treasure;
    private Transform tool;
    private CanvasGroup canvasGroup;
    public static bool isShow;

    public GameObject EmojiHappy;

    void Start()
    {
        brushMat = ObjectData.MaterialRegister(ObjectData.BRUSHMAT);
        Texture reTextrue = Resources.Load("Textrue/BrushTextrue") as Texture;
        brushMat.SetTexture("_BrushTex", reTextrue);
        brushMat.SetFloat("_Size", 5.5f);
        Instance = this;
        treasure.gameObject.SetActive(false);
        tool = ObjectData.GetObjectByName(ObjectData.TOOL);
        canvasGroup = treasure.transform.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (isShow)
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha,1,Time.deltaTime*3);
        else
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, Time.deltaTime*2);
    }


    //12.5新增贴片功能
    //贴花操作
    public  void DecalTextrue()
    {
        //通过nail类里面的值来取值来贴图
        Nail nail = LoadAndParseXML.GetKeyByKey(Data.LEVEL,Data.FingerId);
        Texture deacalTextrue = Resources.Load("Icon/" + nail.Season + "/" + nail.DecalTextrue1) as Texture;
        RenderTexture renderTexture = ObjectData.RenderTextrueRegister(Data.FingerId);
        brushMat.SetTexture("_BGTex", deacalTextrue);
        brushMat.SetFloat("_Size", 0.5f);
        brushMat.SetVector("_UV", new Vector2(0.50f, 0.50f));
        Graphics.Blit(null, renderTexture, brushMat);
        //刷子复位操作
        Texture reTextrue = Resources.Load("Textrue/BrushTextrue") as Texture;
        brushMat.SetTexture("_BrushTex", reTextrue);
        isShow = false;

        EmojiHappy.GetComponent<ParticleSystem>().Play();
    }

    //宝藏盒子出现
    public void ShowTreasure()
    {
        //Todo--出现
        treasure.gameObject.SetActive(true);
        NailTip.isRun = false;
        NailTip.outNailMat.SetColor("_Color", new Color(1, 1, 1, 0));
        tool.gameObject.SetActive(false);
        UIControl.InGame_NextStepTips = false;
        Nail nail = LoadAndParseXML.GetKeyByKey(Data.LEVEL, Data.FingerId);

        UIControl.Instance.NextStpe(GameManger.index + 2, nail.Layer + 1);

        GameManger.showStepTip = false;
        canvasGroup.alpha = 0;
        isShow = true;
        canvasGroup.blocksRaycasts = true;
    }


    //宝藏盒子点击
    public void ClickTreasure()
    {
        DecalTextrue();
        //还需要加上宝箱变灰色的操作
        NailTip.isRun = false;
        NailTip.outNailMat.SetColor("_Color", new Color(1, 1, 1, 0));
        //treasure.gameObject.SetActive(false);
        GameManger.isNeedDecal = false;
        StartCoroutine(DecalEnd());
        Paint.canCrave = false;
        canvasGroup.blocksRaycasts = false;


    }

    //协程，在1秒之后弹出nextFigner或者是拍照按钮
    IEnumerator DecalEnd()
    {
        yield return new WaitForSeconds(0.1f);
        GameManger.isDecal = true;
    }
}
