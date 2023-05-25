using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintToolControl : MonoBehaviour
{
    public Transform TargetLook;

    public Clipper Clipper;
    public Brush[] BrushArray;
    
    private Camera _mainCamera;
    private Vector3 _lastMousePos;
    private bool _isMouseDown;

    private float _minPosX;
    private float _maxPosX;
    private float _minPosZ;
    private float _maxPosZ;

    private int _brushIndex;
    
    public static PaintToolControl Instance { get; private set; }
    
    public void Init()
    {
        Instance = this;
        
        _mainCamera = Camera.main;
    }

    public void StartPaint(MeshRenderer mr)
    {
        Clipper.Init(mr);
        
        var pos = TargetLook.position;
        _minPosX = pos.x - 1.8f;
        _maxPosX = pos.x + 1.6f;
        _minPosZ = pos.z - 5f;
        _maxPosZ = pos.z + 1f;
        
        transform.position = new Vector3(pos.x + 1f, pos.y + 0.2f, pos.z - 3.8f);
        
        Clipper.gameObject.SetActive(true);
        _brushIndex = -1;
        SetBrushVisible(-1);
    }

    private void SetBrushVisible(int index)
    {
        for (int i = 0, maxI = BrushArray.Length; i < maxI; i++)
        {
            BrushArray[i].gameObject.SetActive(i == index);
        }
    }

    public Texture2D NailShellEnd()
    {
        Clipper.gameObject.SetActive(false);
        _brushIndex = 0;
        SetBrushVisible(0);

        return BrushArray[_brushIndex].BrushTex;
    }

    public Texture2D ShowNextBrush()
    {
        _brushIndex++;
        SetBrushVisible(_brushIndex);
        
        return BrushArray[_brushIndex].BrushTex;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if (JudgeClickUI.Instance.IsClickUI()) return;    //点到了UI

            _lastMousePos = Input.mousePosition;

            _isMouseDown = true;
        }

        if (Input.GetMouseButton(0))
        {
            //if (JudgeClickUI.Instance.IsClickUI()) return;    //点到了UI
            
            var z = _mainCamera.WorldToScreenPoint(transform.position).z;
            _lastMousePos.z = z;
            var lastPos = _mainCamera.ScreenToWorldPoint(_lastMousePos);
            var tmpPos = _lastMousePos = Input.mousePosition;
            tmpPos.z = z;
            var curPos = _mainCamera.ScreenToWorldPoint(tmpPos);

            var offsetPos = curPos - lastPos;
        
            //Debug.Log(offsetPos + "," + lastPos + "," + curPos + "," + Input.mousePosition + "," + z);
            
            transform.LookAt(TargetLook, Vector3.forward);//对准
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

            var pos = transform.localPosition;
            pos.x += offsetPos.x;
            pos.z += offsetPos.z;
            curPos.y = 0;
            pos = JudgePos(pos);

            transform.localPosition = pos;
            
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!_isMouseDown) return;
            
            _isMouseDown = false;
            
            //if (JudgeClickUI.Instance.IsClickUI()) return;    //点到了UI
        }
    }
    
    private Vector3 JudgePos(Vector3 pos)
    {
        pos.x = pos.x > _maxPosX ? _maxPosX : pos.x;
        pos.x = pos.x < _minPosX ? _minPosX : pos.x;
        pos.z = pos.z > _maxPosZ ? _maxPosZ : pos.z;
        pos.z = pos.z < _minPosZ ? _minPosZ : pos.z;
        return pos;
    }
}
