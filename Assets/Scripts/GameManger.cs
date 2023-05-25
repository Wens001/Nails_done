using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TapticPlugin;
using DG.Tweening;

public class GameManger : MonoBehaviour
{

    public const int MAXNormalLevel = 44; //【常规关卡总数】




    public AudioSource MusicPlayer;
    public CanvasGroup canvasGroup;
    public AudioClip aduio_cut;
    public AudioClip audio_Shot;

    private static AudioSource audioSource;

    public RawImage rawImage;//拍照图片
    private NailData nailData = new NailData();
    private Material brushMat;//刷子材质球名字
    private Material nailMat;//指甲的材质球 
    private Material outNailMat;//外层提示指甲框颜色
    private Material bristle;//笔刷材质球
    private Material craveMat;//雕花笔材质球
    private Material oilColorMat;//指甲油材质球

    public Image PreMaskImg;    //预览图的遮罩
    public RawImage preImage;//预览图
    private Transform tool;//工具包括指甲钳、刷子、雕花笔
    private Vector3 recordPosition;//

    private Vector3 savePosiotn;
    private bool isBack;
    private float m_timer;

    public Text txt_Poster;//标语
    private Image contact;//雕花笔触点
    public Transform NailOil_B;//结算界面B型手
    public Transform NailOil_C;
    public Transform NailOil_D;
    private GameObject BigHand;//手
    /**步骤**/
    public static int index=1;


    private int maxLayer;
    private int decalCount;

    private float timer;
    private float animotorTimer;
    private bool isClick;//是否第一次点击
    private bool isLastCount=true;//是否最后一根手指
    private bool isComeHome;//是否返回主界面
    private bool isNextStep;//是否可以启动下一步骤
    private bool isShot=true;//是否拍照
    private bool isNextFinger=true;//是否弹出下一根手指button
    //贴片判断
    public static bool isNeedDecal;//是否需要贴花
    public static bool isDecal;//是否已经贴花
    public static bool showStepTip=true;//贴花判断是否显示下一步
    public static GameManger Intance;

    IEnumerator Start()
    {
        LoadNextLevel(Data.LEVEL);

        brushMat = ObjectData.MaterialRegister(ObjectData.BRUSHMAT);
        bristle = ObjectData.MaterialRegister(ObjectData.BRISTLE);
        outNailMat = ObjectData.MaterialRegister(ObjectData.OUTNAIL);
        craveMat = ObjectData.MaterialRegister(ObjectData.CRAVEMat);
        oilColorMat = ObjectData.MaterialRegister(ObjectData.OILCOLORMat);
        tool = ObjectData.GetObjectByName(ObjectData.TOOL);
        Intance = this;
        contact= Paint.Instance.contact;

        NailOil_B.gameObject.SetActive(false);
        NailOil_C.gameObject.SetActive(false);
        NailOil_D.gameObject.SetActive(false);


        yield return new WaitForSeconds(5f);

        //-------------------------关卡进度统计--------------------------------

        if(Data.LEVEL == 1 && PlayerPrefs.GetInt("LogLevel1", 0) == 0)
        {
            //载入关卡时，向AppsFlyer发送关卡进度
            ADSDK.SubmitLatestLevel(Data.LEVEL);

            //载入关卡时，向FaceBook发送关卡进度
            ADSDK.Intance.LogAchievedLevelEvent("Level" + Data.LEVEL.ToString("000"));

            PlayerPrefs.SetInt("LogLevel1", 1);
        }        

        //-------------------------------------------------------------------
    }
    private void Awake()
    {
        Data.LEVEL = PlayerPrefs.GetInt("Level", 1);
        Data.Setting_Sound = PlayerPrefs.GetInt("Setting_Sound", 1);
        Data.Setting_Vibration = PlayerPrefs.GetInt("Setting_Vibration", 1);
        Data.NoAd_isBought = PlayerPrefs.GetInt("NoAd_isBought", 0);
        audioSource = GetComponent<AudioSource>();
    }

    private float _gcTime;

    void Update()
    {

        //Debug.Log("CAO"+Data.isMenu);

        if (Time.time - _gcTime > 120f)
        {
            Resources.UnloadUnusedAssets();    //卸载未占用的asset资源
            System.GC.Collect();    //回收内存
            _gcTime = Time.time;
        }


        if (Data.Setting_Sound == 1)
        {
            MusicPlayer.volume = 0.1f;
        }
        else
        {
            MusicPlayer.volume = 0f;
        }

        if (Data.isSpecialLevel)
        {
            UIControl.Instance.UI_LevelNum.text = "SPECIAL";
        }
        else
        {
            UIControl.Instance.UI_LevelNum.text = "LEVEL " + Data.LEVEL.ToString();
        }



        //涂满底层操作
        if (maxLayer == 1)
        {
            //最后一层满层只有一层的情况下
            if (Data.FingerId == 5)
            {
                timer += Time.deltaTime;
                if(Paint.Instance.Count >= Data.CHECKNUM&&timer>=Data.ONELAYERTIMER&&isShot)
                {
                    if (!isDecal)
                    {
                        JudgeDecal();
                        return;
                    }
                    isShot = false;
                    UIControl.Instance.CanBeShot();
                    isLastCount = false;
                    isNextStep = true;

                    return;
                }
            }
            //非最后一根手指为底层操作
            else
            {
                if (Paint.Instance.Count >= Data.CHECKNUM&&isNextFinger)
                {
                    //提示贴花操作
                    if (!isDecal)
                    {
                        JudgeDecal();
                        return;
                    }
                    UIControl.Instance.CanBeNextFinger();
                    isNextFinger = false;
                }
            }
            
        }

        //非一层就涂满操作
        else
        {
            if (index == 1)
            {
                if (Paint.Instance.Count >= Data.CHECKNUM)
                {
                    UIControl.InGame_NextStepTips = true;
                    isNextStep = true;
                }
            }
            else if (index > 1)
            {
                timer += Time.deltaTime;
                //下一步右上角Tip操作
                if (timer >= Data.NEXTSTEPTIMER && index < maxLayer)
                {
                    UIControl.InGame_NextStepTips = true;
                    isNextStep = true;
                }
                if (timer >= Data.NEXTSTEPTIMER && index == maxLayer)
                {
                    //涂到最顶层 涂满加上时间到了才会显示出来
                    if (Paint.Instance.Count >=2 && Data.FingerId != 5 && isNextFinger)
                    {
                        if (!isDecal)
                        {
                            JudgeDecal();
                            return;
                        }
                        UIControl.Instance.CanBeNextFinger();
                        isNextFinger = false;
                    }

                    //最后一关涂到最顶层
                    if (Paint.Instance.Count >= 2&&Data.FingerId == 5 && isLastCount)
                    {
                        if (!isDecal)
                        {
                            JudgeDecal();
                            return;
                        }
                        UIControl.Instance.CanBeShot();
                        isLastCount = false;
                    }
                }
            }      
        }
        //工具移动限制，不可移出屏幕外侧
        MoveLimit.Instance.Limit(Data.FingerId);
        if (Input.GetKeyDown(KeyCode.Z))
            StartCoroutine(JumpLevel());

    }



    /// <summary>
    /// 自定义拍照截图
    /// </summary>
    /// <returns></returns
    IEnumerator ScreenShotPNG()
    {
        
        //相关关卡包括特殊关卡的存储
        if(Data.LEVEL>0 && Data.LEVEL <= 5)
        {
            UIControl.FinishGame_LikeWillGet = Random.Range(150, 200);
        }
        else if (Data.LEVEL > 5 && Data.LEVEL <= 10)
        {
            UIControl.FinishGame_LikeWillGet = Random.Range(200, 299);
        }
        else if (Data.LEVEL > 10)
        {
            UIControl.FinishGame_LikeWillGet = Random.Range(300, 799);
        }
        

        
        if (Data.Setting_Sound==1)
        {
            AudioPlay = audio_Shot;
        }

        if (Data.Setting_Vibration==1)
        {
            TapticManager.Impact(ImpactFeedback.Heavy);
            TapticManager.Impact(ImpactFeedback.Heavy);
            TapticManager.Impact(ImpactFeedback.Heavy);
        }

        GameObject.FindWithTag(Data.BIGHAND).SetActive(false);
        Nail nail = LoadAndParseXML.GetKeyByKey(Data.LEVEL,1);
        //根据读表当前关卡手的形状，得知场景上应该显示的是哪一个结算关卡的手
        switch (nail.Shape)
        {
            case "B":
                NailOil_B.gameObject.SetActive(true);
                break;
            case "C":
                NailOil_C.gameObject.SetActive(true);
                break;
            case "D":
                NailOil_D.gameObject.SetActive(true);
                break;
        }
        tool.gameObject.SetActive(false);
        contact.gameObject.SetActive(false);
        UIControl.Instance.BubbleReady();
        UIControl.Instance.InGameReady();

        UIControl.InGame_BubbleDisplay = false;
        yield return new WaitForEndOfFrame();
      

        //对截图的照片进行处理
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D((int)(height * 0.3896f), (int)(height * 0.3896f), TextureFormat.RGBA32, false);
        Color col = new Color(0, 0, 0, 0);
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                tex.SetPixel(w, h,col);
            }
        }
        tex.ReadPixels(new Rect((int)(width/2 - height * 0.3896f/2), (int)(height * (1-0.5971f)), 
                        (int)(height * 0.3896f), (int)(height * 0.3896f)), 0, 0 , false);
        
        tex.Apply();
        rawImage.gameObject.SetActive(true);
        NailOil_B.gameObject.SetActive(false);
        NailOil_C.gameObject.SetActive(false);
        NailOil_D.gameObject.SetActive(false);
        rawImage.texture = tex;
        txt_Poster.text = ""; //1204新增
        yield return new WaitForSeconds(1f); //1204新增
        Nail m_nail = LoadAndParseXML.GetKeyByKey(Data.LEVEL, 5); //1204新增
        txt_Poster.DOText(m_nail.Poster, 2f); //1204新增
    }

    /// <summary>
    /// 读取表中的数据，根据对应的关卡、手指来加载指定关卡的数据
    /// </summary>
    /// <param name="level">关卡</param>
    /// <param name="partId">手指数</param>
    public void LoadResouces(int level,int partId)
    {
        Nail nail = LoadAndParseXML.GetKeyByKey(level, partId);
        maxLayer = nail.Layer;
        UIControl.InGame_SumStep = nail.Layer+1;
        if (nail.DecalCount != 0)
        {
            BubbleControl.isDea = true;
            UIControl.InGame_SumStep = nail.Layer + 2;
        }
        else
        {
            BubbleControl.isDea = false;
        }
        decalCount = nail.DecalCount;
        LoadTextrue(nail); 
        Texture icon = Resources.Load("icon/" + nail.Season + "/" + nail.Icon) as Texture;//预览图
        Texture oilTextrue = Resources.Load("icon/" + nail.Season + "/" + nail.OilColor) as Texture;
        oilColorMat.SetTexture("_MainTex",oilTextrue);
        Data.SHELLCOUNT = nail.ShellCount;
        preImage.texture = icon;
        RenderTexture textrue = ObjectData.RenderTextrueRegister(nail.FingerId);
        Paint.Instance.renderTexture = textrue;
       

    }

    // 加载下一步操作
    public void LoadNextStype()
    {
        if (!isNextStep)
            return;
        isNextStep = false;

        timer = 0;
        m_timer = 0;
        animotorTimer = 0;
        if (UIControl.InGame_NextStepTips)
        {
            //由此判断是否需要贴花
            if (isNeedDecal)
            {
                Decal.Instance.ShowTreasure();
                isNeedDecal = false;
                Paint.canCrave = false;
                return;
            }
            ResetTool(Data.FingerId);
            index += 1;
            Paint.canCrave = true;
            Paint.Instance.ClearCount();
            UIControl.InGame_NextStepTips = false;
            Nail nail = LoadAndParseXML.GetKeyByKey(Data.LEVEL, Data.FingerId);
            if (index > nail.Layer)
                return;
            UIControl.Instance.NextStpe(index+1, nail.Layer);
    
            Paint.Instance.ClearCount();
            Paint.Instance.BrushAll();
            if (index != 1)
                NailTip.isRun = true;
            LoadTextrue(nail);
        }
       
    }
    //加载下一关关卡
    public void LoadNextLevel(int level)
    {

        //存储当前玩的数据
        if (!Data.isSpecialLevel)
        {
            if (level >= PlayerPrefs.GetInt("Level", 1))
            {
                PlayerPrefs.SetInt("Level", Data.LEVEL);
            }

            if (Data.LEVEL > MAXNormalLevel)
            {
                Data.LEVEL = 1;
                level = 1;
            }
        }

              

        Data.HP = 3;
        isLastCount = true;
        isShot = true;
        Nail nail = LoadAndParseXML.GetKeyByKey(level, 1);
        Data.isMenu = 0;

        //非第一次打开的一系列操作
        if (Data.isFirstOpen==0)
        {
            BigHand.SetActive(true);
            DestroyImmediate(GameObject.FindWithTag(Data.BIGHAND));
            GameRoot.ClearRenderTxtrue();
            Paint.canBuling = true;
        }
        //清除指甲碎片
        ClearObj.Clear();

        //非第一次进去，则删除之前的手，加载新的手进来，重新加载动画控制器
        BigHand = Instantiate(ObjectData.GetHand(nail.Shape)) as GameObject;
        BigHand.tag = Data.BIGHAND;
        AnimatorControll.cameraAnimator = ObjectData.GetObjectByName(ObjectData.MAINCAMERA).GetComponent<Animator>();
        AnimatorControll.fingerAnimator = BigHand.GetComponent<Animator>();
        NailTip.outNailMat = ObjectData.MaterialRegister(ObjectData.OUTNAIL);

        //指甲涂刷提示层清空停止
        NailTip.Clear();
        index = 1;
        Data.FingerId = 0;
        if (Data.isFirstOpen==0)
        {
            AnimatorControll.Quick("Quick", true);
            AnimatorControll.SwitchState("Finger", 1);
            LoadResouces(level, Data.FingerId + 1);
            NextFinger();
        }

        Data.isFirstOpen = 0;

        PreMaskImg.sprite = Resources.Load<Sprite>("Sprites/PreMaskImg/PreMaskImg" + nail.Shape);
    }


    //加载刷子颜色和该刷的图案
    public void LoadTextrue(Nail nail)
    {
        Texture2D textrue = new Texture2D(1024, 1024, TextureFormat.ARGB32, true, true);
        textrue.filterMode = FilterMode.Trilinear;
        Texture bgColor;

        //根据当前步骤，加载刷子的纹理贴图，以及当前应该刷子应该刷的图案
        switch (index)
        {
            case 1:
                textrue = Resources.Load("icon/" + nail.Season + "/" + nail.Texture1) as Texture2D;//要涂的图
                bgColor = Resources.Load("icon/" + nail.Season + "/" + nail.BgColor1) as Texture;//画笔颜色
                brushMat.SetTexture("_BGTex", textrue);
                outNailMat.SetTexture("_MainTex", textrue);
                bristle.SetTexture("_MainTex",bgColor);

                break;
            case 2:
                textrue = Resources.Load("icon/" + nail.Season + "/" + nail.Texture2) as Texture2D;//要涂的图
                bgColor = Resources.Load("icon/" + nail.Season + "/" + nail.BgColor2) as Texture;//画笔颜色
                brushMat.SetTexture("_BGTex", textrue);
                outNailMat.SetTexture("_MainTex", textrue);
                craveMat.SetTexture("_MainTex", bgColor);

                break;
            case 3:
                textrue = Resources.Load("icon/" + nail.Season + "/" + nail.Texture3) as Texture2D;//要涂的图
                bgColor = Resources.Load("icon/" + nail.Season + "/" + nail.BgColor3) as Texture;//画笔颜色
                brushMat.SetTexture("_BGTex", textrue);
                outNailMat.SetTexture("_MainTex", textrue);
                craveMat.SetTexture("_MainTex", bgColor);
                break;
        }

    }

    //下一根手指的加载
     IEnumerator  LoadNextFinger(int fingerId)
     {
        NailTip.Clear();
        contact.gameObject.SetActive(false);
        isNextFinger = true;
        isNeedDecal = false;
        showStepTip = true;
        Paint.Instance.ClearCount();
        Data.HP = 3;
        ClearObj.Clear();
        //禁用按钮
        UIControl.Instance.UI_HomeBtn.GetComponent<Button>().interactable = false;
        UIControl.Instance.UI_ResetBtn.GetComponent<Button>().interactable = false;

        BigHand.GetComponent<Animator>().enabled = true;
        tool.localScale = new Vector3(0.58f, 0.58f, 0.58f);
        AnimatorControll.SwitchState("Finger", fingerId);
        index = 1;
        //动画片段反向操作
        if (fingerId != 1)
        {
            AnimatorControll.Quick("Quick", false);
        }
       
        tool.gameObject.SetActive(false);
        UIControl.InGame_NextStepTips = false;
        UIControl.Instance.SmallBubbleReset();

        yield return new WaitForSeconds(2f);
        //启用按钮
        UIControl.Instance.UI_HomeBtn.GetComponent<Button>().interactable = true;
        UIControl.Instance.UI_ResetBtn.GetComponent<Button>().interactable = true;

  

        RenderTexture renderTexture = ObjectData.RenderTextrueRegister(fingerId);
        

        Data.FingerId++;
        timer = 0;
        LoadResouces(Data.LEVEL, Data.FingerId);
        ResetTool(Data.FingerId);
        isDecal = decalCount != 0 ? false : true;

        //取消之前手指上的标签，暂且设定为某个值
        GameObject.FindWithTag(Data.HAND).transform.tag = "Player";
        GameObject.FindWithTag(Data.NAIL).transform.tag = "Player";


        //标签移到其他手指
        GameObject.Find("Nail" + fingerId).transform.tag = Data.NAIL;
        GameObject.Find("Hand" + fingerId).transform.tag = Data.HAND;
        Paint.Instance.InitPosition = GameObject.FindWithTag(Data.HAND).transform.parent.position;
        Paint.Instance.finger = GameObject.FindWithTag(Data.HAND).transform.parent;
        Transform m_shellParent = GameObject.Find("NailShell" + fingerId).transform;
        for (int i = 0; i < m_shellParent.childCount; i++)
            m_shellParent.GetChild(i).tag = Data.NAILSHELL;

        if (UIControl.UIState == "InGame")
        {
            UIControl.InGame_BubbleDisplay = true;
            
        }

    }

    public  void  NextFinger()
    {
        StartCoroutine(LoadNextFinger(Data.FingerId + 1));
        NailTip.Clear();
    }

    public void TakePitrue()
    {
        StartCoroutine(ScreenShotPNG());
    }

    public void NextLevel()
    {
        if (Data.isSpecialLevel)
        {

            UIControl.Instance.BackToMenuInEnd();
            Data.isSpecialLevel = false;
            Data.LEVEL = PlayerPrefs.GetInt("Level", 1);
            LoadNextLevel(Data.LEVEL);
            GameObject.FindWithTag(Data.BIGHAND).SetActive(true);

            BackToMenu();
            tool.gameObject.SetActive(false);
            Data.isFirstClick = 1;
            return;
        }

        Data.LEVEL++;
        LoadNextLevel(Data.LEVEL);
        ClearObj.Clear();
        
        //-------------------------关卡进度统计--------------------------------

        //载入关卡时，向AppsFlyer发送关卡进度
        ADSDK.SubmitLatestLevel(Data.LEVEL);


        //载入关卡时，向FaceBook发送关卡进度
        ADSDK.Intance.LogAchievedLevelEvent("Level" + Data.LEVEL.ToString("000"));

        //-------------------------------------------------------------------
    }

    //重玩本关关卡
    public void RePlay()
    {
        ResetTool(Data.FingerId);
        Paint.Instance.ClearCount();
        NailTip.isRun = false;
        index = 1;
        isShot = true;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        Decal.isShow = false;

        isDecal = decalCount != 0 ? false : true;
        isNeedDecal = false;
        isNextFinger = false;
        showStepTip = true; 
        Paint.isFlash = true;
        NailTip.outNailMat.SetColor("_Color", new Color(1,1,1,0));

        UIControl.InGame_NextStepTips = false;
        UIControl.Instance.SmallBubbleReset();

        UIControl.InGame_BubbleDisplay = false;

        UIControl.Instance.OffNextFinger();

        UIControl.Instance.OffShot();

        UIControl.Instance.UI_ShotBtn.rectTransform.position = UIControl.Instance.v3_ShotBtn_Out;

        LoadResouces(Data.LEVEL, Data.FingerId);
        Material  resetMat = ObjectData.MaterialRegister(ObjectData.RESETMAT);
        RenderTexture renderTexture = ObjectData.RenderTextrueRegister(Data.FingerId);
        Graphics.Blit(null, renderTexture, resetMat);//刷子贴
        ClearObj.Clear();
        isNextFinger = true;
        Data.HP = 3;
        string fingerName ="";
        switch (Data.FingerId)
        {
            case 1:
                fingerName = "LittleFinger";
                break;
            case 2:
                fingerName = "RingFinger";
                break;
            case 3:
                fingerName = "MiddleFinger";
                break;
            case 4:
                fingerName = "IndexFinger";
                break;
            case 5:
                fingerName = "ThumbFinger";
                break;
        }
        GameObject finger = GameObject.Find(fingerName);
        Nail nail = LoadAndParseXML.GetKeyByKey(Data.LEVEL,Data.FingerId);
        GameObject fingerClone= Instantiate(Resources.Load("HandModel/"+nail.Shape+"/"+ fingerName), GameObject.FindWithTag(Data.BIGHAND).transform) as GameObject;

        for (int i = 0; i < 4; i++)
            DestroyImmediate(finger.transform.GetChild(0).gameObject);

        for (int j = 0; j < 4; j++)
            fingerClone.transform.GetChild(0).SetParent(finger.transform);

        GameRoot.ClearTextrueByKey(Data.FingerId);
        Destroy(fingerClone);


    }
    /// <summary>
    /// 工具重置
    /// </summary>
    /// <param name="fingerId"></param>
    public void ResetTool(int fingerId)
    {

        if (UIControl.UIWanna != "MainMenu")
        {
            tool.gameObject.SetActive(true);
        }
        BigHand.GetComponent<Animator>().enabled = false;
        Paint.Instance.B2C();
        tool.rotation = Quaternion.Euler(new Vector3(0,-20,0));
        switch (fingerId)
        {
            case 1:
                tool.position = MoveLimit.LittlePosition;
                tool.localScale = MoveLimit.LittleScale;
                break;
            case 2:
                tool.position = MoveLimit.RingPosition;
                tool.localScale = MoveLimit.RingScale;
                break;
            case 3:
                tool.position = MoveLimit.MiddlePositon;
                tool.localScale = MoveLimit.MiddleScale;
                break;
            case 4:
                tool.position = MoveLimit.IndexPosition;
                tool.localScale = MoveLimit.IndexScale;
                break;
            case 5:
                tool.position = MoveLimit.ThumbPosition;
                tool.localScale = MoveLimit.ThumbScale;
                break;
        }
    }

    public static AudioClip AudioPlay
    {
        set
        {
            audioSource.PlayOneShot(value); 
        }
    }

    //游戏中回到主菜单，并且保存当前手指之前手指的涂抹数据
    public void BackToMenu()
    {
        index = 1;
        ClearObj.Clear();
        AnimatorControll.ComePlay("ComePlay", 0);
        Data.HP = 3;
        isNextFinger = true;
        NailTip.isRun = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;

        Decal.isShow = false;
        NailTip.outNailMat.SetColor("_Color", new Color(1, 1, 1, 0));
        Paint.Instance.ClearCount();
        UIControl.Instance.UI_ShotBtn.rectTransform.position= UIControl.Instance.v3_ShotBtn_Out;

        Material nailShellMat = ObjectData.MaterialRegister(ObjectData.NAILSHELL);
        nailShellMat.SetColor("_EmissionColor",
                new Color(0, 0, 0, 1));
        Paint.canBuling = false;
        Paint.isFlash = true;
        AnimatorControll.Reset("ComeBack",true);
        BigHand.GetComponent<Animator>().enabled = true;
        AnimatorControll.fingerAnimator.SetFloat("Finger", 0);
        AnimatorControll.cameraAnimator.SetFloat("Finger", 0);
        tool.gameObject.SetActive(false);
        UIControl.Instance.Btn_NextFinger();
        UIControl.InGame_NextStepTips = false;
        contact.gameObject.SetActive(false);


    }

    //从主菜单进来，进入游戏，可能为第一次进入，可能为返回游戏再次进入游戏
    public void BackToPlay()
    {
        
        AnimatorControll.Reset("ComeBack", false);
        isDecal = decalCount != 0 ? false : true;
        isNeedDecal = false;
        showStepTip = true;
        if (Data.isMenu==1)
        {
            Data.isMenu = 0;
            if (Data.isFirstClick == 1)
            {
                LoadNextLevel(Data.LEVEL);
                Data.isFirstClick = 0;
            }
            else
            {
                StartCoroutine(Back());
            }
        }
        Paint.canBuling = true;

    }

    //游戏中，返回主菜单之后，再次进入游戏
    IEnumerator Back()
    {
        //重置基本属性
        index = 1;
        
        Nail nail = LoadAndParseXML.GetKeyByKey(Data.LEVEL, Data.FingerId);
        GameRoot.ClearTextrueByKey(Data.FingerId);
       
        LoadResouces(Data.LEVEL,Data.FingerId);
        
        //瞬间替换该手指的指甲
        DestroyImmediate(GameObject.Find("NailShell" + Data.FingerId));
        GameObject nailShell = Instantiate(Resources.Load<GameObject>("HandModel/" + nail.Shape + "/NailShell" + Data.FingerId),
            GameObject.FindWithTag(Data.HAND).transform.parent);
        nailShell.name = "NailShell" + Data.FingerId;
        AnimatorControll.Reset("ComeBack", false);
        AnimatorControll.ComePlay("ComePlay", Data.FingerId);
        for (int i = 0; i < nailShell.transform.childCount; i++)
        {
            nailShell.transform.GetChild(i).tag = Data.NAILSHELL;
        }

        PreMaskImg.sprite = Resources.Load<Sprite>("Sprites/PreMaskImg/PreMaskImg" + nail.Shape);

        yield return new WaitForSeconds(1.2f);
        ResetTool(Data.FingerId);
        Paint.Instance.ClearCount();
        tool.gameObject.SetActive(true);
        UIControl.Instance.UI_ShotBtn.rectTransform.position = UIControl.Instance.v3_ShotBtn_Out;
        ClearObj.Clear();
        AnimatorControll.ComePlay("ComePlay", 0);
    }

    //跳关
    IEnumerator JumpLevel()
    {
        tool.gameObject.SetActive(false);
        AnimatorControll.Reset("ComeBack", true);

        index = 1;
        Data.HP = 3;
        isNextFinger = true;
        NailTip.isRun = false;
        Paint.canBuling = false;
        Paint.isFlash = true;
        NailTip.outNailMat.SetColor("_Color", new Color(1, 1, 1, 0));
        Paint.Instance.ClearCount();
        UIControl.Instance.UI_ShotBtn.rectTransform.position = UIControl.Instance.v3_ShotBtn_Out;
        UIControl.InGame_NextStepTips = false;

        UIControl.Instance.SmallBubbleReset();
        UIControl.InGame_BubbleDisplay = false;
        UIControl.Instance.OffNextFinger();

        BigHand.GetComponent<Animator>().enabled = true;
        AnimatorControll.fingerAnimator.SetFloat("Finger", 0);
        AnimatorControll.cameraAnimator.SetFloat("Finger", 0);

        UIControl.Instance.OffShot();
        UIControl.Instance.UI_ShotBtn.rectTransform.position = UIControl.Instance.v3_ShotBtn_Out;

        yield return new WaitForSeconds(1f);
        Data.FingerId = 1;
        Data.LEVEL++;//需处理，对于存储
        AnimatorControll.Reset("ComeBack", false);
        LoadResouces(Data.LEVEL, Data.FingerId);
        tool.gameObject.SetActive(true);
        ResetTool(Data.FingerId);
        LoadNextLevel(Data.LEVEL);
    }

    public void Reward_JumpLevel()
    {
        //激励广告
        StartCoroutine(JumpLevel());
    }

    //贴花判断
    public void JudgeDecal()
    {
        isNeedDecal = true;
        isNextStep = true;
        if (showStepTip)
            UIControl.InGame_NextStepTips = true;
    }
}
