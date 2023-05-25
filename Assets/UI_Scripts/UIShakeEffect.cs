using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShakeEffect : MonoBehaviour
{
    public bool EffectSwitch = true;

    public int Speed = 3;
    public float Intensity = 5;

    private Vector3 MyScale = Vector3.zero;
    private float timer = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        MyScale = gameObject.GetComponent<Transform>().localScale;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EffectSwitch)
        {
            timer += Time.deltaTime;
            gameObject.GetComponent<Transform>().eulerAngles = MyScale + new Vector3(0, 0, 1f * Intensity) * Mathf.Sin(timer * Speed);
            if (timer >= 2 * Mathf.PI)
            {
                timer = 0;
            }
        }
        else
        {
            gameObject.GetComponent<Transform>().localScale = MyScale;
            timer = 0;
        }
    }
}
