using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameNote
{
    public class NoteClass : MonoBehaviour
    {
        public static List<NormalNote> s_NormalNotes = new List<NormalNote>();
        public static List<SpeedNote> s_SpeedNotes = new List<SpeedNote>();
        public static List<EffectNote> s_EffectNotes = new List<EffectNote>();

        public static int CalMs(int pos)
        {
            int _ret = 0;
            s_SpeedNotes.OrderBy(item => item.pos);

            SpeedNote _speedNote = null;
            for (int i = 0; i < s_SpeedNotes.Count; i++)
            {
                if (pos < s_SpeedNotes[i].pos) 
                {
                    if (i == 0) { _speedNote = null; }
                    else { _speedNote = s_SpeedNotes[i - 1]; }
                    break;
                }
            }

            if (_speedNote == null) { _ret = Mathf.RoundToInt(pos); }
            else
            {
                InitSpeedMs();

            }

            return _ret;
        }

        private static void InitSpeedMs()
        {
            float _pos;
            s_SpeedNotes[0].ms = 0;

            for (int i = 1; i < s_SpeedNotes.Count; i++)
            {
                _pos = s_SpeedNotes[i].pos - s_SpeedNotes[i - 1].pos;
                s_SpeedNotes[i].ms = s_SpeedNotes[i - 1].ms + Mathf.RoundToInt(_pos);
            }
        }
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