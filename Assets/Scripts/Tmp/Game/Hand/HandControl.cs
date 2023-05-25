using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControl : MonoBehaviour
{
    public MeshRenderer HandMr;
    
    public FingerControl[] FingerControlArray;

    private int _curFingerIndex;
    
    public static HandControl Instance { get; private set; }
    
    public void Init()
    {
        Instance = this;
    }

    public void StartGame()
    {
        _curFingerIndex = 0;
        
        FingerControlArray[_curFingerIndex].StartPaint();
    }

    public void NailShellDrop()
    {
        FingerControlArray[_curFingerIndex].NailShellDrop();
    }
    
    public void BrushTex(Vector2 hitUv)
    {
        FingerControlArray[_curFingerIndex].BrushTex(hitUv);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
