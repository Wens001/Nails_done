using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerControl : MonoBehaviour
{
    public Vector3 CameraPos;

    public Vector3 PaintRotate;
    
    public NailControl NailContrl;

    public Texture2D[] TextureArray;
    
    private int _nailShellDropCount;

    private int _texIndex;
    
    public void StartPaint()
    {
        _nailShellDropCount = 0;

        Camera.main.transform.position = CameraPos;
        
        transform.localEulerAngles = PaintRotate;
        
        NailContrl.Init(TextureArray[0].width);
    }

    public void NailShellDrop()
    {
        _nailShellDropCount++;

        if (_nailShellDropCount >= NailContrl.TotalShellCount)
        {
            var brushTex = PaintToolControl.Instance.NailShellEnd();

            _texIndex = 0;
            
            NailContrl.StartPaint(TextureArray[_texIndex], brushTex);
        }
    }

    public void BrushTex(Vector2 hitUv)
    {
        var progress = NailContrl.BrushTex(hitUv);
        if (progress > 0.9f)
        {
            if (_texIndex >= TextureArray.Length - 1)    //该根手指完成
            {
                
            }
            else
            {
                var brushTex = PaintToolControl.Instance.ShowNextBrush();

                _texIndex++;
                NailContrl.StartPaint(TextureArray[_texIndex], brushTex);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
