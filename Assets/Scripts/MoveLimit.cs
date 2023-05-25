using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLimit : MonoBehaviour
{
    public static Vector3 LittleScale = new Vector3(0.7f, 0.7f, 0.7f);
    public static Vector3 RingScale = new Vector3(0.8f,0.8f,0.8f);
    public static Vector3 MiddleScale = new Vector3(0.88f,0.88f,0.88f);
    public static Vector3 IndexScale = new Vector3(0.85f, 0.88f, 0.88f);
    public static Vector3 ThumbScale = new Vector3(0.8f, 0.8f, 0.8f);

    public static Vector3 LittlePosition = new Vector3(8.601f, 3.26f, -12.44f);
    public static Vector3 RingPosition = new Vector3(6.21f, 2.08f, -14.21f);
    public static Vector3 MiddlePositon = new Vector3(2.92f, 2.5f, -18.11f);
    public static Vector3 IndexPosition = new Vector3(-1.59f, 2.0285f, -16.28f);
    public static Vector3 ThumbPosition = new Vector3(-5.262f, -0.8f, -4.982f);


    private float Little_MaxX= 9.57f;
    private float Little_MinX= 5.82f;
    private float Little_MaxZ= -9.16f;
    private float Little_MinZ= -13.8f;

    
    private float Ring_MaxX= 7.00257f;
    private float Ring_Minx= 3.294f;
    private float Ring_MaxZ= -10.28f;
    private float Ring_Minz= -16.0529f;

    private float Middle_MaxX= 3.749123f;
    private float Middle_MinX= -0.6109815f;
    private float Middle_MaxZ= -13.98f;
    private float Middle_MinZ = -20.50578f;

    private float Index_MaxX= -0.293f;
    private float Index_MinX= -4.85825f;
    private float Index_MaxZ= -11.94f;
    private float Index_MinZ = -18.588f;

    private float Thumb_MaxX= -4.584f;
    private float Thumb_MinX= -8.135f;
    private float Thumb_MaxZ= -1.170f;
    private float Thumb_MinZ= -6.20f;

    private float LittleMax=-15.2f;
    private float RingMax=-16.7f;
    private float MiddleMax=-20.8f;
    private float IndexMax=-18.5f;
    private float ThumbMax=-5.94f;




    private Transform tool;
    private Transform mousePosition;
    public static MoveLimit Instance;
    void Start()
    {
        tool = ObjectData.GetObjectByName(ObjectData.TOOL);
        tool.localScale = MiddleScale;
        mousePosition=GameObject.Find("mousePosiont").transform;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LittleLimit()
    {

        if (tool.position.x > Little_MaxX)
            tool.position = new Vector3(Little_MaxX, tool.position.y, tool.position.z);
        if (tool.position.x < Little_MinX)
            tool.position = new Vector3(Little_MinX, tool.position.y, tool.position.z);
        if (tool.position.z > Little_MaxZ)
            tool.position = new Vector3(tool.position.x, tool.position.y, Little_MaxZ);
        if (tool.position.z < Little_MinZ)
            tool.position = new Vector3(tool.position.x, tool.position.y, Little_MinZ);
    }

    private void RingLimit()
    {
        if (tool.position.x > Ring_MaxX)
            tool.position = new Vector3(Ring_MaxX, tool.position.y, tool.position.z);
        if (tool.position.x < Ring_Minx)
            tool.position = new Vector3(Ring_Minx, tool.position.y, tool.position.z);
        if (tool.position.z > Ring_MaxZ)
            tool.position = new Vector3(tool.position.x, tool.position.y, Ring_MaxZ);
        if (tool.position.z < Ring_Minz)
            tool.position = new Vector3(tool.position.x, tool.position.y, Ring_Minz);
    }

    private void MiddleLimit()
    {
        if (tool.position.x > Middle_MaxX)
            tool.position = new Vector3(Middle_MaxX, tool.position.y, tool.position.z);
        if (tool.position.x < Middle_MinX)
            tool.position = new Vector3(Middle_MinX, tool.position.y, tool.position.z);
        if (tool.position.z > Middle_MaxZ)
            tool.position = new Vector3(tool.position.x, tool.position.y, Middle_MaxZ);
        if (tool.position.z < Middle_MinZ)
            tool.position = new Vector3(tool.position.x, tool.position.y, Middle_MinZ);
    }

    private void IndexLimit()
    {
        if (tool.position.x > Index_MaxX)
            tool.position = new Vector3(Index_MaxX, tool.position.y, tool.position.z);
        if (tool.position.x < Index_MinX)
            tool.position = new Vector3(Index_MinX, tool.position.y, tool.position.z);
        if (tool.position.z > Index_MaxZ)
            tool.position = new Vector3(tool.position.x, tool.position.y, Index_MaxZ);
        if (tool.position.z < Index_MinZ)
            tool.position = new Vector3(tool.position.x, tool.position.y, Index_MinZ);
    }

    private void ThumbLimit()
    {
        if (tool.position.x > Thumb_MaxX)
            tool.position = new Vector3(Thumb_MaxX, tool.position.y, tool.position.z);
        if (tool.position.x < Thumb_MinX)
            tool.position = new Vector3(Thumb_MinX, tool.position.y, tool.position.z);
        if (tool.position.z > Thumb_MaxZ)
            tool.position = new Vector3(tool.position.x, tool.position.y, Thumb_MaxZ);
        if (tool.position.z < Thumb_MinZ)
            tool.position = new Vector3(tool.position.x, tool.position.y, Thumb_MinZ);
    }

    public  void Limit(int fingerId)
    {
        switch (fingerId)
        {
            case 1:
                LittleLimit();
                break;
            case 2:
                RingLimit();
                break;
            case 3:
                MiddleLimit();
                break;
            case 4:
                IndexLimit();
                break;
            case 5:
                ThumbLimit();
                break;
        }
    }
    public void MousePostionLimit()
    {
        switch (Data.FingerId)
        {
            case 1:
                if (mousePosition.position.z < LittleMax)
                    mousePosition.position = new Vector3(mousePosition.position.x, mousePosition.position.y, LittleMax);
                break;
            case 2:
                if (mousePosition.position.z < RingMax)
                    mousePosition.position = new Vector3(mousePosition.position.x, mousePosition.position.y, RingMax);
                break;
            case 3:
                if (mousePosition.position.z < MiddleMax)
                    mousePosition.position = new Vector3(mousePosition.position.x, mousePosition.position.y, MiddleMax);
                break;
            case 4:
                if (mousePosition.position.z < IndexMax)
                    mousePosition.position = new Vector3(mousePosition.position.x, mousePosition.position.y, IndexMax);
                break;
            case 5:
                if (mousePosition.position.z < ThumbMax)
                    mousePosition.position = new Vector3(mousePosition.position.x, mousePosition.position.y, ThumbMax);
                break;
        }


    }
}
