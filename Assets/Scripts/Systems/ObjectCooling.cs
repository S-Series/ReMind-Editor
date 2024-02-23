using System;
using System.Collections;
using System.Collections.Generic;
using GameNote;
using UnityEngine;

public class ObjectCooling : MonoBehaviour
{
    public static int OverValue = 3200 + 400;
    private static int[] lastIndex = { 0, 0, 0 };
    public static List<NoteHolder> s_noteHolders = new List<NoteHolder>();
    private static List<LineHolder> s_lineHolders = new List<LineHolder>();
    private static List<SpectrumData> s_Spectrums = new List<SpectrumData>();

    public static void CalculateValue()
    {
        OverValue = 1600 * NoteField.s_Zoom + 400;
    }

    /// <param name="runOnly"> 0:Everything || 1:Note || 2:Line || 3:Spectrum </param>
    public static void UpdateCooling(float pos = -1, int runOnly = 0)
    {
        if (pos < 0) { pos = NoteField.s_StartPos; }

        //$ NoteHolder
        if (runOnly == 0 || runOnly == 1)
        {
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
        }

        //$ Line Holder
        if (runOnly == 0 || runOnly == 2)
        {
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
        }

        //$ Spectrum
        if (runOnly == 0 || runOnly == 3)
        {
            float spectrumPos;
            SpectrumData spectrum;
            foreach (SpectrumData data in s_Spectrums) { data.EnableObject(false); }
            s_Spectrums = new List<SpectrumData>();
            for (int i = 0; i < SpectrumManager.s_SpectrumDatas.Count; i++)
            {
                spectrum = SpectrumManager.s_SpectrumDatas[i];
                spectrumPos = NoteClass.MsToPos(spectrum.ms);
                if (spectrumPos > pos + OverValue) { break; }
                else if (spectrumPos >= pos - 200)
                {
                    s_Spectrums.Add(spectrum);
                    spectrum.EnableObject(true);
                }
            }
        }
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
