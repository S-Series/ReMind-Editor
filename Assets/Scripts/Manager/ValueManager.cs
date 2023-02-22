using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    public static int s_testMs = 0;
    public static int s_delay = 0;

    public static double s_Bpm = 120.0;

    public static bool s_isTest = false;
    public static bool s_isPause = false;

    private void FixedUpdate()
    {
        if (!s_isTest) { return; }
        if (s_isPause) { return; }
        s_testMs++;
    }
}
