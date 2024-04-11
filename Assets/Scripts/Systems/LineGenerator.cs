using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameNote;

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

    public static void UpdateHolder()
    {
        int linePosX = 0, endPosX = 0, holderPosX = 0;
        int[] lastPosX = { 0, 0 };
        bool keepLastPos = false;
        ScratchNote[] scratchNotes;
        for (int i = 0; i < noteHolders.Count; i++)
        {
            holderPosX = noteHolders[i].stdPos;
            keepLastPos = holderPosX - endPosX > 100 ? false : true;
            var startPosX = keepLastPos ? lastPosX[0] : 0;
            scratchNotes = new ScratchNote[2] { noteHolders[i].bottoms[0], noteHolders[i].bottoms[1] };
            for (int j = 0; j < 2; j++)
            {
                var Xvalue = 120 * scratchNotes[j].endValue;
                if (scratchNotes[j] != null)
                {
                    if (scratchNotes[j].isPowered)
                    {
                        linePosX -= Xvalue;
                    }
                    //noteHolders[i].ApplyScratchVec();
                }
            }
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
