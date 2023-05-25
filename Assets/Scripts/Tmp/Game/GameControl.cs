using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public HandControl HandControl;
    public PaintToolControl PaintToolControl;

    public static GameControl Instance { get; private set; }

    private IEnumerator Start()
    {
        HandControl.Init();
        PaintToolControl.Init();
        
        yield return null;

        Init();
        
        yield return new WaitForSeconds(1f);

        Camera.main.transform.GetComponent<Animator>().enabled = false;

        StartGame();
        
        PaintToolControl.StartPaint(HandControl.HandMr);
    }

    public void Init()
    {
        Instance = this;
    }

    public void StartGame()
    {
        HandControl.StartGame();
    }
    
    // Update is called once per frame
    public void FrameUpdate()
    {
        
    }
}
