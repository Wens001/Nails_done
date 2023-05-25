using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapticPlugin;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Paint : MonoBehaviour
{

    private Transform tool;
    private Transform clippers;
    private Transform brushSmall;
    private Transform brushBig;
    private Transform brush;
    
    private Transform temp;//工具人
    private Transform mousePoint; //指定目标点
    private Transform point;//笔刷触点
    private Transform nailParent;//被简指甲父类
    public  Transform finger;
    private Transform craveBig;
    private Transform craveSmall;
    private Transform cravePaint;

    public Material brushMat;
    private Material resetMat;
    private Material nailShellMat;
    private Material handMat;

    private Vector3 m_vector;//工具人
    public Vector3 InitPosition;
    private Vector2 pixelUV;//工具人

    private string state;//工具状态
    private float m_timer;//剪手指时间判断
    private float timer;//时间
    private float bulingTimer;//闪光时间
    private float clipperTimer;//剪指甲时间
    private float isclipper;
    private float showTimer;//显示时间
    private bool isBuling=true;
    public static bool isFlash = false;
    private bool isStartShow;
    private float lastVibrateTime;//上次涂抹震动时的时间
    public static bool canBuling=false;
    public static bool canCrave=true;

    public RenderTexture renderTexture;
    public  RenderTexture renderNailShell;
    public static Paint Instance;
    public float Count;
    public Image contact;

    public static int[,] Area = new int[6, 6];

    //初始化一些相关属性
    void Start()
    {
        tool = ObjectData.GetObjectByName(ObjectData.TOOL);
        clippers = ObjectData.GetObjectByName(ObjectData.CLIPPERS);
        brushSmall = ObjectData.GetObjectByName(ObjectData.BRUSHSMALL);
        brushBig = ObjectData.GetObjectByName(ObjectData.BRUSHBIG);
        brush = ObjectData.GetObjectByName(ObjectData.BRUSH);
        mousePoint = ObjectData.GetObjectByName(ObjectData.MOUSEPOSIONT);
        point = ObjectData.GetObjectByName(ObjectData.POINT);
        craveBig = ObjectData.GetObjectByName(ObjectData.CARVEBIG);
        craveSmall = ObjectData.GetObjectByName(ObjectData.CARVESMALL);
        cravePaint = ObjectData.GetObjectByName(ObjectData.CRAVEPAINTING);

        brushMat = ObjectData.MaterialRegister(ObjectData.BRUSHMAT);
        resetMat = ObjectData.MaterialRegister(ObjectData.RESETMAT);
        nailShellMat = ObjectData.MaterialRegister(ObjectData.NAILSHELL);
        handMat = ObjectData.MaterialRegister(ObjectData.HANDMAT);

        brush.gameObject.SetActive(false);
        state = Data.NAILCLIPPERS;
        tool.gameObject.SetActive(false);
        brushSmall.gameObject.SetActive(false);
    }
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Area[i, j] = 0;
            }
        }

    }
    void Update()
    {
        if (GameObject.Find(ObjectData.TOOL)&&Input.GetMouseButton(0) &&  UIControl.UIState == "InGame")
        {
            //12.4新增——下拉状态栏bug和气泡跟随
            m_vector = Camera.main.WorldToScreenPoint(mousePoint.position);
            //限制Input输入防止手机状态栏下拉无法寻找到tool
            if (Input.mousePosition.y < Screen.height-5)
            {
                
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_vector.z);//试下限制
                if(Data.FingerId==5)
                     mousePos = new Vector3(Input.mousePosition.x*1.7f, Input.mousePosition.y*1.7f, m_vector.z);//试下限制
                mousePoint.position = Camera.main.ScreenToWorldPoint(mousePos);

            }
            temp = tool.transform;
            //旋转角度
            if (clippers)
            {
                if (GameObject.FindWithTag(Data.NAIL))
                    temp.LookAt(GameObject.FindWithTag(Data.NAIL).transform, Vector3.forward);//对准
                tool.transform.eulerAngles = new Vector3(temp.eulerAngles.x, temp.eulerAngles.y, 0);

            }
            tool.SetParent(mousePoint);
            if (Input.mousePosition.y >= Screen.height - 5)
                mousePoint.DetachChildren();
            contact.transform.position = Camera.main.WorldToScreenPoint(point.position);

            //剪指甲
            if (state == Data.NAILCLIPPERS)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, point.position - Camera.main.transform.position, out hit))
                {
                    if (hit.transform.parent && hit.transform.parent!=nailParent&& hit.transform.tag == Data.NAILSHELL)
                    {
                        hit.transform.gameObject.AddComponent<Rigidbody>();//指甲赋予刚体属性，当自由落体属性进行计算
                        hit.transform.SetParent(nailParent);
                        Destroy(hit.transform.GetComponent<MeshCollider>());
                        hit.transform.SetParent(GameObject.Find("NailParent").transform);
                        //Destroy(hit.transform.gameObject,4);
                        Data.SHELLCOUNT -= 1;

                        if (Data.Setting_Sound==1)
                        {
                            GameManger.AudioPlay = GameManger.Intance.aduio_cut;
                        }

                        if (Data.Setting_Vibration==1)
                        {
                            TapticManager.Impact(ImpactFeedback.Midium);
                        }

                        if (Data.SHELLCOUNT <= 0)
                        {
                            UIControl.InGame_BubbleDisplay = true;
                            C2B();
                        }

                    }
                    //剪指甲的时候碰到的真正的指甲和手指，以及反馈机制
                    if (hit.transform.tag == Data.HAND || hit.transform.tag == Data.NAIL)
                    {
                        m_timer += Time.deltaTime;
                        Vector3 hitPoint = (hit.transform.parent.position - hit.point);
                        hit.transform.parent.Translate(new Vector3(hitPoint.x,0, hitPoint.z) * Time.deltaTime*0.5f,Space.World);
                        {
                            clipperTimer += Time.deltaTime;
                            handMat.SetColor("_Color", new Color(1, 1 - (Data.DAMAGERATE * clipperTimer / Data.DAMGETIMER),
                                1 - (Data.DAMAGERATE * clipperTimer / Data.DAMGETIMER)));
                        }
                        //扣血机制
                        if (m_timer >= 0.3f)
                        {
                            Data.HP -= 1;
                            if (Data.Setting_Vibration==1)
                            {
                                TapticManager.Impact(ImpactFeedback.Heavy);
                                TapticManager.Impact(ImpactFeedback.Heavy);
                                TapticManager.Impact(ImpactFeedback.Heavy);
                            }
                            if (Data.HP <= 0)
                            {
                                //TODO游戏结束界面结算
                                UIControl.Instance.LevelFailed();
                                if (Data.Setting_Vibration==1)
                                {
                                    TapticManager.Impact(ImpactFeedback.Heavy);
                                    TapticManager.Impact(ImpactFeedback.Heavy);
                                    TapticManager.Impact(ImpactFeedback.Heavy);
                                    TapticManager.Impact(ImpactFeedback.Heavy);
                                    TapticManager.Impact(ImpactFeedback.Heavy);
                                    TapticManager.Impact(ImpactFeedback.Heavy);
                                }
                            }
                            m_timer = 0;
                            Debug.Log(Data.HP);
                        }
                        return;
                    }
                }
            }

            //刷子状态，开始涂抹指甲
            if (state == Data.BRUSH)
            {

                RaycastHit m_hit;
                //刷子切换
                brushSmall.gameObject.SetActive(false);
                brushBig.gameObject.SetActive(true);
                //craveSmall.gameObject.SetActive(false);
                craveBig.gameObject.SetActive(true);

                if (Physics.Raycast(Camera.main.transform.position, point.position - Camera.main.transform.position, out m_hit))
                {
                    if (m_hit.transform.tag == Data.NAIL)
                    {
                        brushSmall.gameObject.SetActive(true);
                        brushBig.gameObject.SetActive(false);
                        //craveSmall.gameObject.SetActive(true);
                        //craveBig.gameObject.SetActive(false);

                        NailTip.Clear();
                        isFlash = false;
                        pixelUV = m_hit.textureCoord;
                        brushMat.SetVector("_UV", pixelUV);
                        Graphics.Blit(null, renderTexture, brushMat);
                        Area[Mathf.RoundToInt(pixelUV.x * 5), Mathf.RoundToInt(pixelUV.y * 5)] = 1;

                        if (Data.Setting_Vibration == 1)
                        {
                            float SpeedTemp = Mathf.Sqrt(Mathf.Pow(Input.GetAxis("Mouse X"), 2) + Mathf.Pow(Input.GetAxis("Mouse Y"), 2));
                            if (Time.time - lastVibrateTime > (1 / (SpeedTemp + 0.1f)))
                            {
                                TapticManager.Impact(ImpactFeedback.Midium);
                                lastVibrateTime = Time.time;
                            }
                        }
                    }
                }
            }
        }
        //步骤非第一步则为雕花笔状态
        if (GameManger.index != 1&&canCrave)
            B2P();
        //手指材质球复位属性
        if (finger)
            finger.position = Vector3.Lerp(finger.position, InitPosition, Time.deltaTime * 2);


        timer += Time.deltaTime;    
        if (timer > 0.3f)
        {
            int num = 0;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 6; j++)
                    if (Area[i, j] == 1)
                        num++;

            Count = num;
            timer = 0;

        }

        if (clipperTimer >= 0)
        clipperTimer -= Time.deltaTime;
        handMat.SetColor("_Color", new Color(1, 1 - (Data.DAMAGERATE * clipperTimer / Data.DAMGETIMER),
                   1 - (Data.DAMAGERATE * clipperTimer / Data.DAMGETIMER)));
        

        if (Input.GetMouseButtonUp(0))
        {
            mousePoint.DetachChildren();
            isStartShow = true;
            brushSmall.gameObject.SetActive(false);
            brushBig.gameObject.SetActive(true);
            //craveSmall.gameObject.SetActive(false);
            craveBig.gameObject.SetActive(true);
            contact.transform.position = Camera.main.WorldToScreenPoint(point.position);
        }
        if (isStartShow)
        {
            showTimer += Time.deltaTime;
            if(showTimer>0.5f&& !isFlash && GameManger.index != 1)
            {
                 NailTip.isRun = true;
                 isFlash = true;
                 isStartShow = false;
                 showTimer = 0;
                 NailTip.tipTimer = 0;
                 NailTip.outNailMat.SetColor("_Color", new Color(1, 1, 1, 0));
            }
        }
        
            


        Test();
    }


    /*
     * 指甲钳转刷子 
     */
    public void C2B()
    {
        brush.gameObject.SetActive(true);
        //if(GameManger.index==0)
            clippers.gameObject.SetActive(false);
        state = Data.BRUSH;
        brushMat.SetFloat("_Size", 3.5f);
        UIControl.Instance.NextStpe(2, 4);
    }

    /*
     * 刷子转指甲钳 
     */
    public void B2C()
    {
        brush.gameObject.SetActive(false);
        clippers.gameObject.SetActive(true);
        cravePaint.gameObject.SetActive(false);
        state = Data.NAILCLIPPERS;
        contact.gameObject.SetActive(false);
    }

    /*
    * 刷子转雕花笔
    */
    public void B2P()
    {
        brush.gameObject.SetActive(false);
        cravePaint.gameObject.SetActive(true);
        clippers.gameObject.SetActive(false);
        brushMat.SetFloat("_Size", 5.5f);
        state = Data.BRUSH;
        contact.gameObject.SetActive(true);
        contact.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }

    /*
     * 待剪指甲闪光
     */
    public void Test()
    { 

        if (isBuling&&canBuling)
        {

            bulingTimer += Time.deltaTime;
             nailShellMat.SetColor("_EmissionColor", 
                new Color(Data.RATE * (bulingTimer / Data.BULLINGTIME), Data.RATE * (bulingTimer / Data.BULLINGTIME),
                Data.RATE * (bulingTimer / Data.BULLINGTIME)));
            if (bulingTimer > Data.BULLINGTIME)
                isBuling = false;
        }
      
        if (!isBuling&&canBuling)
        {
            bulingTimer -= Time.deltaTime;
            nailShellMat.SetColor("_EmissionColor",
                new Color(Data.RATE * (bulingTimer / Data.BULLINGTIME), Data.RATE * (bulingTimer / Data.BULLINGTIME),
                Data.RATE * (bulingTimer / Data.BULLINGTIME)));
            if (bulingTimer <= 0)
                isBuling = true; 
        }

    }
    /*
     * 刷子刷满底层纯色操作
     */
    public void BrushAll()
    {
        Texture texture = Resources.Load("Textrue/BrushAll") as Texture;
        brushMat.SetTexture("_BrushTex", texture);
        brushMat.SetVector("_UV", new Vector2(0.50f, 0.50f));
        Graphics.Blit(null, renderTexture, brushMat);
        //刷子复位操作
        Texture reTextrue = Resources.Load("Textrue/BrushTextrue") as Texture;
        brushMat.SetTexture("_BrushTex", reTextrue);

        for (int i = 0; i < 6; i++)
            for (int j = 0; j < 6; j++)
                Area[i, j] = 0;
    }

    public void ClearCount()
    {
        Count = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Area[i, j] = 0;
            }
        }
    }
}
