
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenShot : MonoBehaviour
{

    public string FileName = "生成图片";

    [ContextMenu("Take Screen Shot")]
    public string TakeScreenShot() {
        //创建桌面路径
        string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        //保存图片名字
        string imageName = FileName + ".png";
        ScreenCapture.CaptureScreenshot(path + "/" + imageName);
        return path;
    }
}
