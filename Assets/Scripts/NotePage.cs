using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePage : MonoBehaviour
{
    public static bool isPageControl = false;
    public static double ZoomValue = 1.0;
    public static double ViewPage = 1.0;
    [SerializeField] GameObject[] PageObjects;

    private void Update()
    {
        if (!isPageControl) { return; }

        if (Input.GetAxis("Mouse Wheel") < 0)
        {
            MovePage(false, Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
        }

        if (Input.GetAxis("Mouse Wheel") > 0)
        {
            MovePage(true, Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
        }
    }

    private void MovePage(bool isUp, bool isCtrl)
    {
        if (isCtrl)
        {
            double value;
            if (isUp) { value = 0.1; }
            else  { value = -0.1; }
            ZoomValue += value;
            if (ZoomValue < 0.0) { ZoomValue = 0.1; }
        }
        else
        {
            
        }
    }
}
