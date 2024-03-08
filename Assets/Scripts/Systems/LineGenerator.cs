using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    private static LineGenerator s_this;
    [SerializeField] Transform DefaultLine;
    public static List<float> linePosX;
    private static List<NoteHolder> noteHolders;

    private void Awake()
    {
        if (s_this == null) { s_this = this; }
        else { GetComponent<LineGenerator>().enabled = false; }

        linePosX = new List<float>();
        noteHolders = new List<NoteHolder>();
    }

    public static float GetNotePosX(float posY)
    {
        float ret = 0f;
        ret = linePosX[noteHolders.FindLastIndex(item => item.stdPos < posY)];
        return ret;
    }
    public static void AddHolder(NoteHolder holder)
    {
        if (noteHolders.Contains(item: holder)) { return; }
        noteHolders.Add(item: holder);
        noteHolders.OrderBy(item => item.stdPos);
    }
    public static void AddAllHolders()
    {
        for (int i = 0; i < NoteHolder.s_holders.Count; i++)
        {
            if (noteHolders.Contains(item: NoteHolder.s_holders[i])) { return; }
            noteHolders.Add(item: NoteHolder.s_holders[i]);
        }
        noteHolders.OrderBy(item => item.stdPos);
    }
    public static void RemoveHolder(NoteHolder holder)
    {
        if (!noteHolders.Contains(item: holder)) { return; }
        noteHolders.RemoveAll(item => item == holder);
    }
    public static void RemoveAllHolders()
    {
        noteHolders = new List<NoteHolder>();
    }
}
