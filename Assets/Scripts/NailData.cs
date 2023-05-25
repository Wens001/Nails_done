using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NailData {

    public  Dictionary<int, Nail> m_dict = new Dictionary<int, Nail>();

    private void Start()
    {
        
    }
    public void InitData()
    {
        Type type = typeof(Nail);
        for (int i = 1; i <=5; i++)
        {
            Nail nail = new Nail();
            NailChange(nail,i,1);
            m_dict[nail.FingerId] = nail;
        }
    }


    public Nail GetItemById(int id)
    {
        return m_dict[id];
    }


    //反射
    public void NailChange(Nail nail,int id,int level)
    {
        Type type = typeof(Nail);
        Hashtable hashtable = GGetConf.GGetConfByKey(GGetConf.NAILINFO, id, level);
        FieldInfo[] fieldInfos = type.GetFields();
        foreach (FieldInfo field in fieldInfos)
        {
            if (field.FieldType == typeof(int))
                field.SetValue(nail, int.Parse(hashtable[field.Name].ToString()));
            if (field.FieldType == typeof(string))
                field.SetValue(nail, hashtable[field.Name]);
        }
    }

}
