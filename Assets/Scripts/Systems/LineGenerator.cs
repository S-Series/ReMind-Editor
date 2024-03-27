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

    public static void UpdateHolders()
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
            if (scratchNotes[0] != null)
            {
                if (scratchNotes[0].isPowered)
                {
                    linePosX -= 120 * scratchNotes[0].endValue;
                }
                noteHolders[i].ApplyScratchVec(
                    true, scratchNotes[0].isSlide ?
                    new Vector3[2] {
                        new Vector3(startPosX, 0, 0),
                        new Vector3(startPosX, 0, 0)
                    } :
                    new Vector3[4] {
                        new Vector3(keepLastPos ? lastPosX[0] : 0, -100, 0),
                        new Vector3(keepLastPos ? lastPosX[0] : 0, 0, 0),
                        new Vector3(),
                        new Vector3()
                    }
                );
            }
        }
    }

    public static float GetNotePosX(float posY)
    {
        float ret = 0f;
        ret = linePosX[noteHolders.FindLastIndex(item => item.stdPos < posY)];
        return ret;
    }
}
