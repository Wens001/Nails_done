using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData
{
    /**切换指甲钳状态**/
    public static string STATE_NAILCLIPPERS = "state_nailclippers";

    /**切换指甲刷状态**/
    public static string STATE_BRUSH = "state_brush";

    /**切换工具闲置状态**/
    public static string STATE_IDLE = "state_idle";



    /**指甲钳**/
    public static string CLIPPERS = "Clippers";

    /**雕花笔弯**/
    public static string CARVEBIG = "carveBig";

    /**雕花笔弯**/
    public static string CARVESMALL = "carveSmall";

    /**雕花笔**/
    public static string CRAVEPAINTING = "carvingPaint";

    /**工具**/
    public static string TOOL = "Tool";

    /**指甲刷**/
    public static string BRUSH = "Brush";

    /**软刷**/
    public static string BRUSHSMALL = "BrushSmall";

    /**硬刷**/
    public static string BRUSHBIG = "BrushBig";

    /**目标旋转点**/
    public static string MOUSEPOSIONT = "mousePosiont";

    /**指甲油**/
    public static string NAILOIL= "NailOil";

    /**目标旋转点**/
    public static string POINT = "point";

    /**手**/
    public static string BIGHAND = "BigHand";

    /**摄像机**/
    public static string MAINCAMERA= "MainCamera";

    /**刷子材质球**/
    public static string BRUSHMAT = "Brush";

    /**重置材质球**/
    public static string RESETMAT = "Reset";

    /**指甲碎材质球**/
    public static string NAILSHELL = "NailShell";

    /**外层指甲材质球**/
    public static string OUTNAIL = "OutNail";

    /**指甲碎材质球**/
    public static string BRISTLE = "Bristle";

    /**雕花笔材质球**/
    public static string CRAVEMat = "cravePaint";

    /**指甲油材质球**/
   public static string OILCOLORMat = "NialOil";

    /**手材质球**/
    public static string HANDMAT= "Hand";

    /**渲染图1**/
    public static string RENDERTEXTURE1 = "renderTexture1";

    /**渲染图2**/
    public static string RENDERTEXTURE2 = "renderTexture2";

    /**渲染图3**/
    public static string RENDERTEXTURE3 = "renderTexture3";

    /**渲染图4**/
    public static string RENDERTEXTURE4 = "renderTexture4";

    /**渲染图5**/
    public static string RENDERTEXTURE5 = "renderTexture5";

    //材质球注册
    public static Material MaterialRegister(string name)
    {   
        Material material = Resources.Load("Materials/" + name) as Material;
        return material;
    }

    /**根据名字获取对象**/
    public static Transform GetObjectByName(string name)
    {
        
        Transform ObjectTransform = GameObject.Find(name).transform;
        return ObjectTransform;
    }

    //按手指获取渲染图
    public static RenderTexture RenderTextrueRegister(int fingerId)
    {
        RenderTexture renderTexture = Resources.Load("Textrue/renderTexture" + fingerId.ToString()) as RenderTexture;
        return renderTexture;
    }

    //按手指获取材质球
    public static Material GetNailMat(int fingerId)
    {
        Material material = Resources.Load("Materials/NailMat" + fingerId.ToString()) as Material;
        return material;
    }

    //加载手
    public static GameObject GetHand(string  level)
    {
        GameObject BigHand = Resources.Load("Model/" + level) as GameObject;
        return BigHand;
    }


    
}
