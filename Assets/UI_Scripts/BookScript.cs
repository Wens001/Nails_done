using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookScript : MonoBehaviour
{

    public GameObject UI_SpecialBtnGroup;

    public Image UI_NormalBtn_Light;
    public Image UI_SpecialBtn_Light;

    public Text UIText_NormalBtnText;
    public Text UIText_SpecialBtnText;

    private Color LightColor = new Color(0.988f, 0.988f, 0.988f, 1);
    private Color DarkColor = new Color(0.4235f, 0.1686f, 0.29f, 1);

    public static bool RefreshSwitch = false;

    public RawImage BookBox;
    public RectTransform FatherPanelTransform;
    public RectTransform PanelTransform;
    public RectTransform content;

    private GameObject BookBoxFab;
    private GameObject SpecialBoxFab;

    private int BookBoxCreateCount = 0;
    private int Collect_Select_Temp = -2;
    
    private int LevelCountTemp = 0;

    private int BigPage = 1;

    private readonly List<GameObject> _normalCellList = new List<GameObject>();
    private readonly List<GameObject> _specialTitleList = new List<GameObject>();
    private readonly Dictionary<int, List<GameObject>> _specialCellDic = new Dictionary<int, List<GameObject>>();

    public void Btn_Normal()
    {
        UI_NormalBtn_Light.enabled = true;
        UI_SpecialBtn_Light.enabled = false;
        UIText_NormalBtnText.color = DarkColor;
        UIText_SpecialBtnText.color = LightColor;

        UIControl.Collect_Select = 0;

        //RefreshSwitch = true;
    }

    public void Btn_Special()
    {
        UI_NormalBtn_Light.enabled = false;
        UI_SpecialBtn_Light.enabled = true;
        UIText_NormalBtnText.color = LightColor;
        UIText_SpecialBtnText.color = DarkColor;

        UIControl.Collect_Select = -1;

        //RefreshSwitch = true;
    }


    public void Btn_UnlockSpecial()
    {
        if(Data.LikeNum >= 3000)
        {
            Data.LikeNum -= 3000;
            string str = "SpecialLevel" + Collect_Select_Temp.ToString();
            int temp = PlayerPrefs.GetInt(str, 0);
            temp++;
            PlayerPrefs.SetInt(str, temp);
            RefreshSwitch = true;
            
            PlayerPrefs.SetInt("LikeNum", Data.LikeNum);
        }
    }

    public void Btn_ADGetLike()
    {
        //激励广告请求
        ADRewardControl.RewardType = 5;
        ADSDK.Intance.showRewardADs("special_get500");
    }




    //---------------------图鉴页面层次机---------------------------------

    private int SpecialPageNum = 3;

    private string[] SpecialThemename = new string[6] 
    {"Halloween","Christmas","Cat","Rabbit","",""}; 

    //-------------------------------------------------------------------



    // Start is called before the first frame update
    void Start()
    {
        BookBoxFab = (GameObject)Resources.Load("LevelView");
        SpecialBoxFab = (GameObject)Resources.Load("SpecialView");

        UI_NormalBtn_Light.enabled = true;
        UI_SpecialBtn_Light.enabled = false;

        UIText_NormalBtnText.color = DarkColor;
        UIText_SpecialBtnText.color = LightColor;

        UI_SpecialBtnGroup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if((Collect_Select_Temp != UIControl.Collect_Select && UIControl.UIState == "MainMenu")|| RefreshSwitch == true)
        {

            #region 常规关卡

            if (UIControl.Collect_Select == 0)
            {

                UI_SpecialBtnGroup.SetActive(false);

                Collect_Select_Temp = UIControl.Collect_Select;
                RefreshSwitch = false;
                Debug.Log(UIControl.Collect_Select);
                BookBoxCreateCount = 1;
                LevelCountTemp = 0;
                for (int k = 1; k < Collect_Select_Temp; k++)
                {
                    LevelCountTemp += LoadAndParseXML.count[k];
                }
                for (int i = 0; i < LoadAndParseXML.count[Collect_Select_Temp] / 2f; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (BookBoxCreateCount < LoadAndParseXML.count[Collect_Select_Temp] + 1)
                        {
                            GameObject BookBoxClone = null;
                            if (BookBoxCreateCount > _normalCellList.Count)
                            {
                                BookBoxClone = Instantiate(BookBoxFab, PanelTransform);
                                _normalCellList.Add(BookBoxClone);
                            }
                            else
                            {
                                BookBoxClone = _normalCellList[BookBoxCreateCount - 1];
                                BookBoxClone.SetActive(true);
                            }
                            BookBoxClone.GetComponent<RectTransform>().anchoredPosition = new Vector2(BookBox.rectTransform.anchoredPosition.x + (j * 485), BookBox.rectTransform.anchoredPosition.y - (i * 485));
                            Nail nail = LoadAndParseXML.GetKeyByKey(LevelCountTemp + BookBoxCreateCount, 1);
                            BookBoxClone.transform.name = (LevelCountTemp + BookBoxCreateCount).ToString();
                            

                            Texture texture = Resources.Load("Icon/" + nail.Season + "/" + nail.handbook) as Texture;

                            if ((LevelCountTemp + BookBoxCreateCount) > PlayerPrefs.GetInt("Level", 1))
                            {
                                BookBoxClone.GetComponent<Button>().interactable = false;
                                BookBoxClone.transform.Find("Image").GetComponent<Image>().enabled = false;
                                BookBoxClone.transform.Find("Txt_Level").GetComponent<Text>().text = "level " + (LevelCountTemp + BookBoxCreateCount).ToString();
                            }
                            else
                            {
                                BookBoxClone.GetComponent<Button>().interactable = true;
                                BookBoxClone.transform.Find("RawImage").GetComponent<RawImage>().texture = texture;
                                BookBoxClone.transform.Find("Image").GetComponent<Image>().enabled = true;
                                BookBoxClone.transform.Find("Txt_Level").GetComponent<Text>().text = "";
                            }
                            BookBoxCreateCount++;
                        }
                    }
                }
                PanelTransform.anchoredPosition = new Vector2(0, -372.24f);
                content.offsetMax = new Vector2(0, 0);
                content.offsetMin = new Vector2(content.offsetMin.x, -((int)(((LoadAndParseXML.count[Collect_Select_Temp] + 1) / 2f)) * 485 + 65 - FatherPanelTransform.rect.height));
            }

            #endregion

            #region 特殊主题

            else if (UIControl.Collect_Select == -1)
            {

                UI_SpecialBtnGroup.SetActive(false);

                Collect_Select_Temp = UIControl.Collect_Select;
                RefreshSwitch = false;
                Debug.Log(UIControl.Collect_Select);
                BookBoxCreateCount = 1;
                LevelCountTemp = 0;

                for (int i = 0; i < SpecialPageNum / 2f; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (BookBoxCreateCount < SpecialPageNum + 1)
                        {
                            GameObject BookBoxClone = null;
                            if (BookBoxCreateCount > _specialTitleList.Count)
                            {
                                BookBoxClone = Instantiate(SpecialBoxFab, PanelTransform);
                                _specialTitleList.Add(BookBoxClone);
                            }
                            else
                            {
                                BookBoxClone = _specialTitleList[BookBoxCreateCount - 1];
                                BookBoxClone.SetActive(true);
                            }
                            
                            BookBoxClone.GetComponent<RectTransform>().anchoredPosition = new Vector2(BookBox.rectTransform.anchoredPosition.x + (j * 485), BookBox.rectTransform.anchoredPosition.y - (i * 485));
                            
                            BookBoxClone.transform.name = BookBoxCreateCount.ToString();

                            BookBoxClone.transform.Find("Txt_Level").GetComponent<Text>().text = SpecialThemename[BookBoxCreateCount - 1];

                            string str = "SpecialTheme" + BookBoxCreateCount.ToString();
                            //Debug.Log(str);

                            if (PlayerPrefs.GetInt(str, 1) == 0 )    //直接解锁
                            {
                                Texture texture = Resources.Load("SpecialPage/Special_0" + BookBoxCreateCount) as Texture; //未解锁状态
                                BookBoxClone.transform.Find("RawImage").GetComponent<RawImage>().texture = texture;
                                BookBoxClone.transform.Find("LockIcon").GetComponent<Image>().enabled = true;
                            }
                            else
                            {
                                Texture texture = Resources.Load("SpecialPage/Special_0" + BookBoxCreateCount+"_Light") as Texture; //解锁状态
                                BookBoxClone.transform.Find("RawImage").GetComponent<RawImage>().texture = texture;
                                BookBoxClone.transform.Find("LockIcon").GetComponent<Image>().enabled = false;
                            }
                            

                            //BookBoxClone.transform.Find("RawImage").GetComponent<RawImage>().texture = texture;

                            BookBoxCreateCount++;
                        }
                    }
                }
                PanelTransform.anchoredPosition = new Vector2(0, -372.24f);
                content.offsetMax = new Vector2(0, 0);

                if (SpecialPageNum < 6)
                {
                    content.offsetMin = new Vector2(content.offsetMin.x, 0);
                }
                else
                {
                    content.offsetMin = new Vector2(content.offsetMin.x, -((int)(((SpecialPageNum + 1) / 2f)) * 485 + 65 - FatherPanelTransform.rect.height));
                }

            }

            #endregion

            else if (UIControl.Collect_Select >= 1)
            {
                if (!_specialCellDic.ContainsKey(UIControl.Collect_Select))
                    _specialCellDic.Add(UIControl.Collect_Select, new List<GameObject>());
                
                var cellList = _specialCellDic[UIControl.Collect_Select];

                Collect_Select_Temp = UIControl.Collect_Select;
                RefreshSwitch = false;
                Debug.Log(UIControl.Collect_Select);
                BookBoxCreateCount = 1;
                LevelCountTemp = 0;
                for (int k = 0; k < Collect_Select_Temp; k++)
                {
                    LevelCountTemp += LoadAndParseXML.count[k];
                }
                for (int i = 0; i < LoadAndParseXML.count[Collect_Select_Temp] / 2f; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (BookBoxCreateCount < LoadAndParseXML.count[Collect_Select_Temp] + 1)
                        {
                            GameObject BookBoxClone = null;
                            if (BookBoxCreateCount > cellList.Count)
                            {
                                BookBoxClone = Instantiate(BookBoxFab, PanelTransform);
                                cellList.Add(BookBoxClone);
                            }
                            else
                            {
                                BookBoxClone = cellList[BookBoxCreateCount - 1];
                                BookBoxClone.SetActive(true);
                            }
                            
                            BookBoxClone.GetComponent<RectTransform>().anchoredPosition = new Vector2(BookBox.rectTransform.anchoredPosition.x + (j * 485), BookBox.rectTransform.anchoredPosition.y - (i * 485));
                            Nail nail = LoadAndParseXML.GetKeyByKey(LevelCountTemp + BookBoxCreateCount, 1);
                            BookBoxClone.transform.name = (LevelCountTemp + BookBoxCreateCount).ToString();

                            Texture texture = Resources.Load("Icon/" + nail.Season + "/" + nail.handbook) as Texture;
                            string str = "SpecialLevel" + Collect_Select_Temp.ToString();

                            if (LoadAndParseXML.count[Collect_Select_Temp] > PlayerPrefs.GetInt(str, 0))
                            {
                                UI_SpecialBtnGroup.SetActive(true);
                            }
                            else
                            {
                                UI_SpecialBtnGroup.SetActive(false);
                            }

                            if ((BookBoxCreateCount) > PlayerPrefs.GetInt(str, 0))
                            {
                                BookBoxClone.GetComponent<Button>().interactable = false;
                                BookBoxClone.transform.Find("Image").GetComponent<Image>().enabled = false;
                                BookBoxClone.transform.Find("Txt_Level").GetComponent<Text>().text = SpecialThemename[UIControl.Collect_Select - 1] +  BookBoxCreateCount.ToString();
                            }
                            else
                            {
                                BookBoxClone.GetComponent<Button>().interactable = true;
                                BookBoxClone.transform.Find("RawImage").GetComponent<RawImage>().texture = texture;
                                BookBoxClone.transform.Find("Image").GetComponent<Image>().enabled = true;
                                BookBoxClone.transform.Find("Txt_Level").GetComponent<Text>().text = "";
                            }
                            BookBoxCreateCount++;
                        }
                    }
                }
                PanelTransform.anchoredPosition = new Vector2(0, -372.24f);
                content.offsetMax = new Vector2(0, 0);
                content.offsetMin = new Vector2(content.offsetMin.x, -((int)(((LoadAndParseXML.count[Collect_Select_Temp] + 1) / 2f)) * 485 + 65 - FatherPanelTransform.rect.height));
                
//                if (SpecialPageNum < 6)
//                {
//                    content.offsetMin = new Vector2(content.offsetMin.x, -((int)(((8 + 1) / 2f)) * 485 + 65 - FatherPanelTransform.rect.height));
//                }
//                else
//                {
//                    content.offsetMin = new Vector2(content.offsetMin.x, -((int)(((SpecialPageNum + 1) / 2f)) * 485 + 65 - FatherPanelTransform.rect.height));
//                }
            }


        }
    }
}
