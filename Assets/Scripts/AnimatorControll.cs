using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControll : MonoBehaviour
{
    public static Animator cameraAnimator;
    public  static Animator fingerAnimator;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SwitchState(string paraments,float value)
    {
        cameraAnimator.SetFloat(paraments,value);
        fingerAnimator.SetFloat(paraments,value);
    }

    public static void Reset(string paraments, bool value)
    {
        cameraAnimator.SetBool(paraments, value);
        fingerAnimator.SetBool(paraments, value);
    }

    public static void Quick(string paraments, bool value)
    {
        cameraAnimator.SetBool(paraments, value);
        fingerAnimator.SetBool(paraments, value);
    }

    public static void ComePlay(string paraments, int value)
    {
        cameraAnimator.SetInteger(paraments, value);
        fingerAnimator.SetInteger(paraments, value);
    }

}
