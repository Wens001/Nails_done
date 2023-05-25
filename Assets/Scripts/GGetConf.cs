using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEngine;

public class GGetConf{

    public  static string NAILINFO = "NailInfo";


    /// <summary>
    /// 获取表信息
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static Hashtable GetConfByName(string fileName)
    {
        Hashtable Hash = new Hashtable();
        //FileStream stream = File.Open("Assets/" + student + ".xlsx", FileMode.Open, FileAccess.Read, FileShare.Read);
        //IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //DataSet result = excelReader.AsDataSet();
        //DataTable table=result.Tables[0];
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/" + fileName + ".xml");
        XmlNodeList nodeList = doc.SelectNodes("//NailInfo");

        //for (int i = 2; i < table.Rows.Count; i++)
        //{
        //    Hashtable hashtable = new Hashtable();

        //    for (int j = 0; j < table.Columns.Count; j++)
        //    {
        //        hashtable.Add(table.Rows[0][j], table.Rows[i][j]);
        //    }
        //    Hash.Add(table.Rows[i][0], hashtable);      
        //}
        return Hash;
    }

    /// <summary>
    /// 根据关键词搜索行
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Hashtable GGetConfByKey(string fileName,int id,int level)
    {
        Hashtable Hash = GetConfByName(fileName);
        foreach (Hashtable hash in Hash.Values)
        {
            if (hash["Id"].ToString() == id.ToString()&& hash["Level"].ToString() == level.ToString())
            {
                return hash;
            }
        }
        return null;
    }
}
