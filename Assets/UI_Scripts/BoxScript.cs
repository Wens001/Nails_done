using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{

    public int Collect_Select_Temp = 0;

    // Start is called before the first frame update

    private void Awake()
    {
        Collect_Select_Temp = UIControl.Collect_Select;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Collect_Select_Temp != UIControl.Collect_Select)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
