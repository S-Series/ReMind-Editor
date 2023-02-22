using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class MultyNoteHolder
{
    public int stdMs;
    public int stdPos;

    public NormalNote[] normals = new NormalNote[4];
    public NormalNote[] airials = new NormalNote[4];
    public NormalNote[] bottoms = new NormalNote[2];

    public SpeedNote speed = null;
    public EffectNote effect = null;

    public void InitBySpeed(SpeedNote note)
    {
        stdMs = note.ms;
        stdPos = note.pos;
        speed = note;
    }
}
