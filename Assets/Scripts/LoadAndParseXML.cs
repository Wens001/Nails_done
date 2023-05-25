using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Xml;
using Mono.Xml;
using UnityEngine;

public class LoadAndParseXML : MonoBehaviour
{
    public static Dictionary<int, Dictionary<int, Nail>> nailDataDic = new Dictionary<int, Dictionary<int, Nail>>();

    private void Awake()
    {
        LoadConfigData();
        SeasonCount();
    }

    public Dictionary<int, Dictionary<int, Nail>> LoadConfigData()
    {
        var txtAsset = Resources.Load<TextAsset>("Config/Nail");
        var fileText = txtAsset.text;

        if (string.IsNullOrEmpty(fileText))
        {
            Debug.LogError("关卡数据为空");
            return null;
        }

        var sp = new SecurityParser();
        sp.LoadXml(fileText);
        var se = sp.ToXml();
        var type = typeof(Nail);
        foreach (SecurityElement sel in se.Children)
        {
            if (sel.Tag.Equals("Nail"))
            {
                var data = new Nail();
                foreach (SecurityElement se2 in sel.Children)
                {
                    var fileInfo = type.GetField(se2.Tag);
                    if (fileInfo == null)
                    {
                        //Debug.LogError("关卡数据字段：" + se2.Tag + " 无对应");
                        continue;
                    }
                    if (fileInfo.FieldType.Name == "Int32")
                        fileInfo.SetValue(data, int.Parse(se2.Text));
                    else if (fileInfo.FieldType.Name == "String")
                        fileInfo.SetValue(data, se2.Text);
                }

                if (!nailDataDic.ContainsKey(data.Level))
                    nailDataDic.Add(data.Level, new Dictionary<int, Nail>());

                nailDataDic[data.Level].Add(data.FingerId, data);
            }
        }

        return nailDataDic;
    }

    /// <summary>
    /// 根据两个key值返回
    /// </summary>
    /// <param name="level"></param>
    /// <param name="fingerId"></param>
    /// <returns></returns>
    public static Nail GetKeyByKey(int level,int fingerId)
    {
        foreach(Dictionary<int, Nail> dict in nailDataDic.Values)
        {
            foreach(Nail nail in dict.Values)
            {
                if (nail.FingerId == fingerId && nail.Level == level)
                    return nail;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据关卡和季节返回
    /// </summary>
    /// <param name="level"></param>
    /// <param name="season"></param>
    /// <returns></returns>
    public static Nail GetSpriteByKey(int level)
    {
        foreach (Dictionary<int, Nail> dict in nailDataDic.Values)
        {
            foreach (Nail nail in dict.Values)
            {
                if (nail.Level == level)
                    return nail;
            }
        }
        return null;
    }


    public static int[] count=new int[5] { 0,0, 0, 0,0 };
    public static void SeasonCount()
    {
        
        foreach (Dictionary<int, Nail> dict in nailDataDic.Values)
        {
            foreach (Nail nail in dict.Values)
            {
                int season = nail.Season;
                count[season]+=1;
            }
        }


        for(int i = 0; i < count.Length; i++)
        {
            int num = count[i] / 5;
            count[i] = num;
        }
    }
}
