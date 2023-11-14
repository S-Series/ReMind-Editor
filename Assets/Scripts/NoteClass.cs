using System;
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

        public static void SortAll()
        {
            s_NormalNotes.OrderBy(item => item.pos).ThenBy(item => item.line);
            s_SpeedNotes.OrderBy(item => item.pos);
            s_EffectNotes.OrderBy(item => item.pos);
        }
        public static void InitAll()
        {
            InitSpeedMs();
            foreach (NormalNote note in s_NormalNotes)
            {
                note.ms = CalMs(note.pos);
            }
            foreach (EffectNote note in s_EffectNotes)
            {
                note.ms = CalMs(note.pos);
            }    
        }    

        public static int CalMs(int pos)
        {
            int _ret = 0;
            if (pos <= 0) { return _ret; }

            int _calMs;
            float _calPos;
            double _calBpm;
            s_SpeedNotes.OrderBy(item => item.pos);

            SpeedNote _speedNote = null;

            int _index = 0;
            for (int i = 0; i < s_SpeedNotes.Count; i++)
            {
                if (pos >= s_SpeedNotes[i].pos)
                {
                    _index = i;
                }
                else { break; }
            }

            if (_index == 0) { _speedNote = null; }
            else { _speedNote = s_SpeedNotes[_index - 1]; }

            if (_speedNote == null)
            {
                _calMs = 0;
                _calPos = pos;
                _calBpm = ValueManager.s_Bpm;
            }
            else
            {
                InitSpeedMs();
                _calMs = _speedNote.ms;
                _calPos = pos - _speedNote.pos;
                _calBpm = _speedNote.bpm * _speedNote.multiple;
            }

            _ret = Mathf.RoundToInt(Convert.ToSingle(150 * _calPos / _calBpm));

            return _ret;
        }

        private static void InitSpeedMs()
        {
            if (s_SpeedNotes.Count == 0) { return; }

            float _pos;
            double _bpm;
            s_SpeedNotes[0].ms = Mathf.RoundToInt(Convert.ToSingle(150 * s_SpeedNotes[0].pos / ValueManager.s_Bpm));

            for (int i = 1; i < s_SpeedNotes.Count; i++)
            {
                _bpm = s_SpeedNotes[i].bpm * s_SpeedNotes[i].multiple;
                _pos = s_SpeedNotes[i].pos - s_SpeedNotes[i - 1].pos;
                s_SpeedNotes[i].ms = s_SpeedNotes[i - 1].ms 
                    + Mathf.RoundToInt(Convert.ToSingle(150 * _pos / _bpm));
            }
        }
    }
    
    public class Note
    {
        public int pos, ms;
    }

    public class NormalNote:Note
    {
        public int line, length = 1, SoundIndex = 0;
        public bool isAir;
        public NoteHolder holder;

        public static NormalNote Generate()
        {
            NormalNote ret;
            ret = new NormalNote();
            NoteClass.s_NormalNotes.Add(ret);
            return ret;
        }
    }

    public class SpeedNote:Note
    {
        public double bpm, multiple;
        public SpeedHolder holder;

        public static SpeedNote Generate()
        {
            SpeedNote ret;
            ret = new SpeedNote();
            NoteClass.s_SpeedNotes.Add(ret);
            return ret;
        }
    }

    public class EffectNote:Note
    {
        public int effectIndex, value;
        public EffectHolder holder;

        public static EffectNote Generate()
        {
            EffectNote ret;
            ret = new EffectNote();
            NoteClass.s_EffectNotes.Add(ret);
            return ret;
        }
    }
}