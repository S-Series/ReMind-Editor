using System;
using System.Collections;
using System.Collections.Generic;
using GameNote;
using UnityEngine;

public class ObjectCooling : MonoBehaviour
{
    public static int OverValue = 3200 + 400;
    private static int[] lastIndex = { 0, 0, 0 };
    private static List<NoteHolder> s_noteHolders = new List<NoteHolder>();
    private static List<LineHolder> s_lineHolders = new List<LineHolder>();

    public static void CalculateValue()
    {
        OverValue = 1600 * NoteField.s_Zoom + 400;
    }

    public static void UpdateCooling(float pos = -1)
    {
        if (pos == -1) { pos = NoteField.s_StartPos; }
        //$ NoteHolder
        NoteHolder noteHolder;
        foreach (NoteHolder holder in s_noteHolders) { holder.EnableNote(false); }
        s_noteHolders = new List<NoteHolder>();
        for (int i = 0; i < NoteHolder.s_holders.Count; i++)
        {
            noteHolder = NoteHolder.s_holders[i];
            if (noteHolder.stdPos > pos + OverValue) { break; }
            if (isHolderVisible(pos, noteHolder))
            {
                s_noteHolders.Add(noteHolder);
                noteHolder.EnableNote(true);
            }
        }

        //$ Line Holder
        LineHolder lineHolder;
        foreach (LineHolder holder in s_lineHolders) { holder.EnableHolder(false); }
        s_lineHolders = new List<LineHolder>();
        for (int i = 0; i < LineHolder.s_holders.Count; i++)
        {
            lineHolder = LineHolder.s_holders[i];
            if (lineHolder.GetPosValue() > pos + OverValue) { break; }
            else if (lineHolder.GetPosValue() >= pos - 200)
            {
                s_lineHolders.Add(lineHolder);
                lineHolder.EnableHolder(true);
            }
        }
    
        //$ Spectrum
        SpectrumData spectrum;
    }
    public static void UpdateTestCooling(float pos)
    {
        //$ NoteHolder
        if (lastIndex[0] == NoteHolder.s_holders.Count) {; }
        else if (isHolderVisible(pos, NoteHolder.s_holders[lastIndex[0]]))
        {
            NoteHolder.s_holders[lastIndex[0]].EnableNote(true);
            lastIndex[0]++;
        }

        //$ LineHolder
    }
    public static void ResetLastIndex(int[] ints = null)
    {
        if (ints == null) { ints = new int[] { 0, 0, 0 }; }
        lastIndex = ints;
    }

    private static bool isHolderVisible(float pos, NoteHolder holder)
    {
        int stdPos;
        stdPos = holder.stdPos;
        if (stdPos > pos + OverValue) { return false; }
        else if (stdPos + holder.NoteMaxLength(true) < pos - 200) { return false; }
        else { return true; }
    }
}
