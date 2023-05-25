using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Mono.Xml;
using System.Xml;
using System.Security;

public class XmlData : MonoBehaviour
{
    void Start()
    {
        string path = Application.persistentDataPath + "/" + "jewel.xml";
        WWW www = new WWW(path);
        
        string str = www.text;
        //写进去
        XmlDocument xml = new XmlDocument();
        xml.Load(path);//加载xml文件
        XmlNode root = xml.SelectSingleNode("root");//获取根节点
        var node = xml.CreateTextNode("jewel");
        
        XmlElement info = xml.CreateElement("jewel");//创建新的子节点
        info.SetAttribute("Id", "2");//创建新子节点属性名和属性值
        info.SetAttribute("Wear", "1");
        info.SetAttribute("Name", "L1");
        //info.
        root.AppendChild(info);//将子节点按照创建顺序，添加到xml
        xml.AppendChild(root);
        xml.Save(path);//保存xml到路径位置


        XmlNodeList nodeList = xml.SelectSingleNode("root").ChildNodes;
        foreach (XmlElement xe in nodeList)
        {//遍历所以子节点
            if (xe.Name == "jewel")
            {
                //Debug.Log(xe.GetAttribute("Id"));//获取Name属性值
                //Debug.Log(xe.GetAttribute("Wear"));
                //Debug.Log(xe.GetAttribute("Name"));
                
            }
        }
    }

    void Update()
    {
        
    }
}
