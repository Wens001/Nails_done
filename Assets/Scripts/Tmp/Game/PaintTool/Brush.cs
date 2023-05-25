using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public Transform TriggerPosTrans;
    
    public Texture2D BrushTex;
    
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, TriggerPosTrans.position - Camera.main.transform.position, out hit))
        {
            if (hit.transform.CompareTag("Nail"))
            {
                HandControl.Instance.BrushTex(hit.textureCoord);
            }
        }
    }
}
