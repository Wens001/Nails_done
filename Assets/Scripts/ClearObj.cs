using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 指甲碎片清除计划
    /// </summary>
    public static void Clear()
    {
        //if (!GameObject.Find(Data.NAILSHELL))
        ////    return;
        //Transform nailShellParent = GameObject.Find("NailParent").transform;
        //for(int i = 0; i< nailShellParent.childCount; i++)
        //{
        //    //if(!nailShells[i].transform.parent)
        //    DestroyImmediate(nailShellParent.GetChild(i).gameObject);
        //}
        GameObject[] shells= GameObject.FindGameObjectsWithTag(Data.NAILSHELL);
        for(int i = 0; i < shells.Length; i++)
        {
            if (shells[i].transform.parent == GameObject.Find("NailParent").transform)
                Destroy(shells[i].gameObject);
        }
    }
}
