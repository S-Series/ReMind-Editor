using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameNote;

public class LineGenerator : MonoBehaviour
{
    private static LineGenerator s_this;
    [SerializeField] Transform[] DefaultLine;
    public static List<float> linePosX;
    private static List<NoteHolder> noteHolders;

    private void Awake()
    {
        if (s_this == null) { s_this = this; }
        else { GetComponent<LineGenerator>().enabled = false; }

        linePosX = new List<float>();
        noteHolders = new List<NoteHolder>();
    }

    public static void UpdateHolder()
    {
        int linePosX = 0;
        for (int i = 0; i < NoteHolder.s_holders.Count; i++)
        {
            
        }
    }
    public static void UpdateHolder(NoteHolder holder)
    {
        int index;
        index = noteHolders.FindIndex(item => item == holder);
    }
    public static float GetNotePosX(float posY)
    {
        float ret = 0f;
        ret = linePosX[noteHolders.FindLastIndex(item => item.stdPos < posY)];
        return ret;
    }
}
