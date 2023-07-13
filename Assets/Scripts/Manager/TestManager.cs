using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class TestManager : MonoBehaviour
{
    public static int Perf = 0, Score = 0;
    public static int[] Pure = {0, 0}, Near = {0, 0}, Lost = {0, 0};

    public static float s_bpm = 120.0f, s_multiple = 1.0f;

    public static void ResetTest()
    {
        Perf = 0;
        Pure = new int[2] { 0, 0 };
        Near = new int[2] { 0, 0 };
        Lost = new int[2] { 0, 0 };
        Score = 0;

        s_bpm = 120.0f;
        s_multiple = 1.0f;
    }
}
