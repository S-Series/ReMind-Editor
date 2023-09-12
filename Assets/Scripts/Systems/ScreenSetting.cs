using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;

public class ScreenSetting : MonoBehaviour
{
    private static List<Camera> s_cameras = new List<Camera>();

    void Start()
    {
        s_cameras = GameManager.FindAllObjects<Camera>();
    }

    public void ScreenResolution(TMP_Dropdown dropdown)
    {
        int value;
        value = dropdown.value;
        if (value == 0) { Application.targetFrameRate = 30; }
        else if (value == 1) { Application.targetFrameRate = 45; }
        else if (value == 3) { Application.targetFrameRate = 120; }
        else if (value == 4) { Application.targetFrameRate = 180; }
        else if (value == 5) { Application.targetFrameRate = 240; }
        else { Application.targetFrameRate = 60; } //# value == 2
    }
    public void AntiAliasing(TMP_Dropdown dropdown)
    {
        AntialiasingMode mode;
        if (dropdown.value == 1) { mode = AntialiasingMode.FastApproximateAntialiasing; }
        else if (dropdown.value == 2) {mode = AntialiasingMode.SubpixelMorphologicalAntiAliasing;}
        else { mode = AntialiasingMode.None; }

        foreach(Camera camera in s_cameras)
        {
            camera.GetComponent<UniversalAdditionalCameraData>().antialiasing = mode;
        }
    }
}
