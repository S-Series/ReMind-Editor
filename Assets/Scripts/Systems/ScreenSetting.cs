using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;

public class ScreenSetting : MonoBehaviour
{
    private static List<Camera> s_cameras = new List<Camera>();
    private static int[] s_Values = new int[5] { 0, 0, 0, 0, 0 };
    [SerializeField] TMP_Dropdown[] drops;

    void Start()
    {
        print(gameObject.name);
        s_cameras = GameManager.FindAllObjects<Camera>();
        s_Values[0] = PlayerPrefs.GetInt("Screen01");
        s_Values[1] = PlayerPrefs.GetInt("Screen02");
        s_Values[2] = PlayerPrefs.GetInt("Screen03");
        s_Values[3] = PlayerPrefs.GetInt("Screen04");
        s_Values[4] = PlayerPrefs.GetInt("Screen05");
        for (int i = 0; i < 5; i++)
        {
            drops[i].value = s_Values[i];
        }
        UpdateScreen();
    }

    public void ScreenResolutionValue(TMP_Dropdown dropdown)
    {
        s_Values[0] = dropdown.value;
        PlayerPrefs.SetInt("Screen01", dropdown.value);
        UpdateScreen();
    }
    public void ScreenModeValue(TMP_Dropdown dropdown)
    {
        s_Values[1] = dropdown.value;
        PlayerPrefs.SetInt("Screen02", dropdown.value);
        UpdateScreen();
    }
    private void UpdateScreen()
    {
        FullScreenMode mode;
        if (s_Values[1] == 1) { mode = FullScreenMode.ExclusiveFullScreen; }
        else if (s_Values[1] == 2) { mode = FullScreenMode.FullScreenWindow; }
        else { mode = FullScreenMode.Windowed; }

        int width, height;
        width = s_Values[0] < 5 ? 320 * (s_Values[0] + 2) : 640 * (s_Values[0] - 2);
        height = s_Values[0] < 5 ? 180 * (s_Values[0] + 2) : 360 * (s_Values[0] - 2);

        Screen.SetResolution(width, height, mode);
    }

    public void FrameRate(TMP_Dropdown dropdown)
    {
        SetFrameRate(dropdown.value);
        PlayerPrefs.SetInt("Screen03", dropdown.value);
    }
    private void SetFrameRate(int value)
    {
        if (value == 0) { Application.targetFrameRate = 30; }
        else if (value == 1) { Application.targetFrameRate = 45; }
        else if (value == 3) { Application.targetFrameRate = 120; }
        else if (value == 4) { Application.targetFrameRate = 180; }
        else if (value == 5) { Application.targetFrameRate = 240; }
        else if (value == 6) { Application.targetFrameRate = -1;}
        else { Application.targetFrameRate = 60; } //# value == 2
    }
    
    public void AntiAliasingValue(TMP_Dropdown dropdown)
    {
        SetAntiAliasing(dropdown.value);
        PlayerPrefs.SetInt("Screen04", dropdown.value);
    }
    private void SetAntiAliasing(int value)
    {
        AntialiasingMode mode;
        if (value == 1) { mode = AntialiasingMode.FastApproximateAntialiasing; }
        else if (value == 2) {mode = AntialiasingMode.SubpixelMorphologicalAntiAliasing;}
        else { mode = AntialiasingMode.None; }

        foreach(Camera camera in s_cameras)
        {
            camera.GetComponent<UniversalAdditionalCameraData>().antialiasing = mode;
        }
    }

    public void QualityValue(TMP_Dropdown dropdown)
    {
        SetQuality(dropdown.value);
        PlayerPrefs.SetInt("Screen05", dropdown.value);
    }
    private void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value, true);
    }

#if UNITY_EDITOR
    [ContextMenu("Reset PlayerPrefs")]
    private void ResetPrefs()
    {
        PlayerPrefs.SetInt("Screen01", 3);
        PlayerPrefs.SetInt("Screen02", 0);
        PlayerPrefs.SetInt("Screen03", 2);
        PlayerPrefs.SetInt("Screen04", 0);
        PlayerPrefs.SetInt("Screen05", 2);
    }
#endif
}
