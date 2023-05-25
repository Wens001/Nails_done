using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CaptureScreen : MonoBehaviour
{
    [MenuItem("CaptureScreen/Capture")]
    public static void ClearPlayerPrefs()
    {
//        EditorUtility.DisplayDialog(
//            "Select Texture",
//            "You Must Select a Texture first!",
//            "Ok");
        
        var path = EditorUtility.SaveFilePanel(
            "Save texture as PNG",
            Application.dataPath + "/../",
            "",
            "png");
        
        if (path.Length != 0)
        {
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log(path);
        }
        
//        var tex = ScreenCapture.CaptureScreenshotAsTexture();
//        
//        var bytes = tex.EncodeToPNG();
//        System.IO.File.WriteAllBytes(Application.dataPath + "/../ScreenShot/1.png", bytes);
    }
}
