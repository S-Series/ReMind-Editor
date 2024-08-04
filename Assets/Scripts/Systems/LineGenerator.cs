using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameNote;

public class LineGenerator : MonoBehaviour
{
    private static LineGenerator s_this;
    [SerializeField] Transform[] _DefaultLine;
    private static Transform[] DefaultLine;
    public static List<float> linePosX;
    private static List<NoteHolder> noteHolders;
    private static float DefaultScaleX;
    private static float DefaultScaleY;

    private void Awake()
    {
        if (s_this == null) { s_this = this; }
        else { GetComponent<LineGenerator>().enabled = false; }

        linePosX = new List<float>();
        noteHolders = new List<NoteHolder>();

        DefaultLine = _DefaultLine;
        DefaultScaleX = DefaultLine[0].localScale.x;
        DefaultScaleY = DefaultLine[0].localScale.y;

        DefaultLine[0].localPosition = new Vector3(0, 0, 0);
        DefaultLine[1].localPosition = new Vector3(0, 0, 5);
    }

    public static void UpdateHolder()
    {
        if (NoteHolder.s_holders.Count == 0) { return; }

        Vector3 vecValue;
        NoteHolder noteHolder;
        noteHolder = NoteHolder.s_holders[0];
        vecValue = new Vector3(DefaultScaleX, GetScaleValueY(noteHolder.stdPos), 1);
        DefaultLine[0].localScale = vecValue;

        int linePosX = 0;
        var bottoms = noteHolder.floors;
        for (int i = 0; i < NoteHolder.s_holders.Count; i++)
        {
            noteHolder = NoteHolder.s_holders[i];
            noteHolder.ApplyLine(linePosX, GetScaleValueY(noteHolder.NoteMaxLength()));
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
    private static float GetScaleValueY(int length)
    {
        return DefaultScaleY * (length / 1600f);
    }
}
