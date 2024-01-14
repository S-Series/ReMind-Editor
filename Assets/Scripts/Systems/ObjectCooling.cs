using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCooling : MonoBehaviour
{
    public static int OverValue = 3200;

    public static void CalculateValue()
    {
        OverValue = 1600 * NoteField.s_Zoom;
    }

    public static void UpdateCooling(float pos)
    {
        //$ NoteHolder
        foreach (NoteHolder holder in NoteHolder.holders)
        {
            if (holder.stdPos < pos - 400) { holder.gameObject.SetActive(false); }
            else if (holder.stdPos > pos + 1600 * NoteField.s_Zoom + 400) { holder.EnableNote(false); }
            else { holder.EnableNote(true); }
        }

        //$ Line Holder
    }
    public static void UpdateTestCooling(float pos)
    {
        //$ NoteHolder
        foreach (NoteHolder holder in NoteHolder.holders)
        {
            
        }

        //$ LineHolder
    }
}
