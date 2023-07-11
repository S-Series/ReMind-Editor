using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class TestManager : MonoBehaviour
{
    public static int Perf = 0;
    public static int[] Pure = {0, 0}, Near = {0, 0}, Lost = {0, 0};

    public static float s_bpm = 120.0f, s_multiple = 1.0f;

    public static NormalNote ApplyNote(int line, int index)
    {
        NormalNote ret = null;

        return ret;
    }
    public static SpeedNote ApplySpeed()
    {
        SpeedNote ret = null;

        return ret;
    }
    public static EffectNote ApplyEffect()
    {
        EffectNote ret = null;

        return ret;
    }
}
