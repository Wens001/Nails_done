using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelJump : MonoBehaviour
{
    public void JumpLevel()
    {
        //Todo图鉴退出

        UIControl.Instance.UI_Book.rectTransform.DOAnchorPos3D(UIControl.Instance.V3_Book_Out, 0.3f).SetEase(Ease.OutQuart);
        UIControl.Instance.Btn_StartGame();
        

        int level = int.Parse(transform.name);
        if (level== 0)
            return;

        if(UIControl.Collect_Select >= 1)
        {
            Data.isSpecialLevel = true;
        }
        else
        {
            Data.isSpecialLevel = false;
        }

        Data.LEVEL = level;
        AnimatorControll.Reset("ComeBack", false);
        GameManger.Intance.LoadNextLevel(level);
        Data.isFirstClick = 0;
    }
}
