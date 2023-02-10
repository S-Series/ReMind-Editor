using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameNote
{
    public class NoteClass : MonoBehaviour
    {
        public static List<NormalNote> s_normalNotes;
        public static List<SpeedNote> s_speedNotes;
        public static List<EffectNote> s_effectNotes;
    }
    
    public class Note
    {
        public int pos, ms;
    }

    public class NormalNote:Note
    {
        public int line;
        public bool isAir;
    }

    public class SpeedNote:Note
    {
        double bpm, multiple;
    }

    public class EffectNote:Note
    {
        int effectIndex, duration;
    }
}