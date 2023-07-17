using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Judge;
using GameNote;

public class TestManager : MonoBehaviour
{
    public static int Perfect = 0, Score = 0, MaxCount = 0;
    public static int[] Pure = {0, 0}, Near = {0, 0}, Lost = {0, 0};

    public static float s_bpm = 120.0f, s_multiple = 1.0f;

    private static List<SpeedNote> speedNotes;
    private static List<EffectNote> effectNotes;

    public static void ResetTest()
    {
        Perfect = 0;
        Pure = new int[2] { 0, 0 };
        Near = new int[2] { 0, 0 };
        Lost = new int[2] { 0, 0 };
        Score = 0;

        s_bpm = 120.0f;
        s_multiple = 1.0f;
    }
    public static void AddJudge(Judgetype type)
    {


        Score = Mathf.RoundToInt((Perfect + Pure[0] + Pure[1] 
            + (Near[0] + Near[1]) * 0.5f) / MaxCount * 10000000);
    }
    public static void ApplyNoteList(List<SpeedNote> speeds, List<EffectNote> effects)
    {
        speedNotes = speeds;
        effectNotes = effects;
    }
}
