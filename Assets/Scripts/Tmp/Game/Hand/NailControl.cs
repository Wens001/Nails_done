using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailControl : MonoBehaviour
{
    public int TotalShellCount = 6;
    
    public MeshRenderer NailMr;
    
    private Material _mat;

    private Texture2D _tex;

    private bool[] _paintedIndex;

    private int _size;

    private int _brushSize;
    private int _brushSizeHalf;
    private Color[] _brushColorArray;


    private bool _isCanPaint;
    private int _texIndex;
    private Texture2D _paintTex;

    private Color[] _defaultColorArray;
    private Color[] _targetColorArray;
    private Color[] _useTargetColorArray;
    private Color[] _useColorArray;
    private bool _isAdd;
    private float _changeTag;

    private int _needPaintCount;
    
    private readonly Color _defaultColor = new Color(253 / 255f, 193 / 255f, 201 / 255f, 232 / 255f);

    public void Init(int size)
    {
        _size = size;
        _tex = CreateTexture2D(_size, _defaultColor);
        
        _mat = NailMr.material;
        _mat.SetTexture("_MainTex", _tex);
        _mat.SetColor("_Color", Color.white);
        
        _defaultColorArray = _tex.GetPixels(0, 0, _tex.width, _tex.height);
        _useColorArray = new Color[_tex.width * _tex.height];
        _useTargetColorArray = new Color[_tex.width * _tex.height];
    }
    
    private Texture2D CreateTexture2D(int size, Color color)
    {
        var tt = new Texture2D(size, size);
        var colorArray = new Color[size * size];
        for (int i = 0, maxI = colorArray.Length; i < maxI; i++)
        {
            colorArray[i] = color;
        }
        tt.SetPixels(colorArray);
        tt.Apply();
        return tt;
    }

    public void StartPaint(Texture2D paintTex, Texture2D brushTex)
    {
        _isCanPaint = true;

        _paintTex = paintTex;

        _isAdd = true;
        
        _targetColorArray = _paintTex.GetPixels(0, 0, _paintTex.width, _paintTex.height);

        _needPaintCount = CalculateNeedPaintCount(_targetColorArray);
        
        Debug.Log("需要涂抹的数量：" + _needPaintCount);
        
        _brushSize = brushTex.width;
        _brushSizeHalf = _brushSize / 2;
        _brushColorArray = brushTex.GetPixels();
        
        _paintedIndex = new bool[_size * _size];
    }

    private int CalculateNeedPaintCount(Color[] colorArray)
    {
        var count = 0;
        for (int i = 0, maxI = colorArray.Length; i < maxI; i++)
        {
            var tmp = colorArray[i] * 0.4f;
            tmp.a = colorArray[i].a;
            _useTargetColorArray[i] = tmp;
            
            if (colorArray[i].a < 0.01f) continue;

            count++;
        }

        return count;
    }
    
    private void BlendTextureChange()
    {
        for (int i = 0, maxI = _useColorArray.Length; i < maxI; i++)
        {
            if (_paintedIndex[i])
                _useColorArray[i] = _defaultColorArray[i];
            else
                _useColorArray[i] = Color.Lerp(_defaultColorArray[i], _useTargetColorArray[i],
                    _useTargetColorArray[i].a * _changeTag);
        }
        
        _tex.SetPixels(_useColorArray);
        _tex.Apply();

        if (_isAdd)
        {
            _changeTag += Time.deltaTime;
            if (_changeTag >= 1)
            {
                _changeTag = 1;
                _isAdd = false;
            }
        }
        else
        {
            _changeTag -= Time.deltaTime;
            if (_changeTag <= 0)
            {
                _changeTag = 0;
                _isAdd = true;
            }
        }
    }

    public float BrushTex(Vector2 hitUv)
    {
        var hitX = (int)(hitUv.x * _size);
        var hitY = (int)(hitUv.y * _size);
        var x = Mathf.Clamp(hitX - _brushSizeHalf, 0, _size - 1);
        var y = Mathf.Clamp(hitY - _brushSizeHalf, 0, _size - 1);
        var width = Mathf.Clamp(hitX + _brushSizeHalf, 0, _size) - x;
        var height = Mathf.Clamp(hitY + _brushSizeHalf, 0, _size) - y;

        var srcColorArray = ColorGetPixels(_defaultColorArray, _size, x, y, width, height);
        var disColorArray = ColorGetPixels(_targetColorArray, _size, x, y, width, height);
                
        var offsetX = x - (hitX - _brushSizeHalf);
        var offsetY = y - (hitY - _brushSizeHalf);

        //计算绘制后的颜色
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var index = i * width + j;
                var alpha = _brushColorArray[(offsetY + i) * _brushSize + offsetX + j].a;

                var paintedIndex = (y + i) * _size + x + j;

                if (alpha > 0.5f && !_paintedIndex[paintedIndex] && disColorArray[index].a >= 0.01f)
                {
                    srcColorArray[index] = Color.Lerp(srcColorArray[index], disColorArray[index], disColorArray[index].a);
                    _paintedIndex[paintedIndex] = true;
                }
            }
        }
                
        ColorSetPixels(_defaultColorArray, _size, x, y, width, height, srcColorArray);

        return CalculatePainted() / (float)_needPaintCount;
    }

    private void Update()
    {
        if (_isCanPaint)
        {
            BlendTextureChange();
        }
    }

    private int CalculatePainted()
    {
        var count = 0;
        for (int i = 0, maxI = _paintedIndex.Length; i < maxI; i++)
        {
            if (_paintedIndex[i]) count++;
        }
        
        Debug.Log(count);

        return count;
    }

    private void ColorSetPixels(Color[] srcColorArray, int size, int x, int y, int width, int height, Color[] dstColorArray)
    {
        var startIndex = y * size + x;
        for (var i = 0; i < height; i++)
        {
            var dstStart = i * width;
            var srcStart = startIndex + i * size;
            for (var j = 0; j < width; j++)
            {
                srcColorArray[srcStart + j] = dstColorArray[dstStart + j];
            }
        }
    }

    private Color[] ColorGetPixels(Color[] srcColorArray, int size, int x, int y, int width, int height)
    {
        var dstColorArray = new Color[width * height];
        var startIndex = y * size + x;
        for (var i = 0; i < height; i++)
        {
            var dstStart = i * width;
            var srcStart = startIndex + i * size;
            for (var j = 0; j < width; j++)
            {
                dstColorArray[dstStart + j] = srcColorArray[srcStart + j];
            }
        }

        return dstColorArray;
    }
}
