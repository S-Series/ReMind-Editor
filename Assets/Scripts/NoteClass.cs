using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;

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
                note.ms = PosToMs(note.pos);
            }
            foreach (EffectNote note in s_EffectNotes)
            {
                note.ms = PosToMs(note.pos);
            }
        }

        public static int PosToMs(int pos)
        {
            float _ret;
            if (pos <= 0) { _ret = 0; }
            else if (s_SpeedNotes.Count == 0) { _ret = 150f * pos / ValueManager.s_Bpm; }
            else
            {
                int index;
                index = s_SpeedNotes.FindLastIndex(item => item.pos <= pos);

                if (index == -1) { _ret = 150f * pos / ValueManager.s_Bpm; }
                else
                {
                    SpeedNote target;
                    target = s_SpeedNotes[index];

                    _ret = target.ms + 150f * (pos - target.pos) / (Single)target.bpm;
                }
            }
            return Mathf.RoundToInt(_ret);
        }
        public static float MsToPos(float Ms)
        {
            float ret = 0.0f;
            s_SpeedNotes.FindLastIndex(item => item.ms <= Ms);
            return ret;
        }

        public static void InitSpeedMs()
        {
            if (s_SpeedNotes.Count == 0) { return; }

            int lastMs;
            float pos;
            double bpm;

            lastMs = Mathf.RoundToInt(150f * s_SpeedNotes[0].pos / ValueManager.s_Bpm);
            s_SpeedNotes[0].ms = lastMs;

            for (int i = 1; i < s_SpeedNotes.Count; i++)
            {
                bpm = s_SpeedNotes[i - 1].bpm;
                pos = s_SpeedNotes[i].pos - s_SpeedNotes[i - 1].pos;
                lastMs += Mathf.RoundToInt(150f * pos / (Single)bpm);
                s_SpeedNotes[i].ms = lastMs;
            }
        }
    }

    public class Note
    {
        public int pos, ms;
    }

    public class NormalNote : Note
    {
        public int line, length, SoundIndex;
        public bool isAir;
        public NoteHolder holder;

        public static NormalNote Generate()
        {
            NormalNote ret;
            ret = new NormalNote();
            ret.length = 1;
            ret.SoundIndex = 0;
            NoteClass.s_NormalNotes.Add(ret);
            return ret;
        }
    }

    public class SpeedNote : Note
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
        public double getBpm()
        {
            return bpm * multiple;
        }
    }

    public class EffectNote : Note
    {
        public int effectIndex, value;
        public EffectHolder holder;
        public string GetEffectName()
        {
            switch (effectIndex)
            {
                case 1: return "None";

                default: return "None";
            }
        }
        public static EffectNote Generate()
        {
            EffectNote ret;
            ret = new EffectNote();
            NoteClass.s_EffectNotes.Add(ret);
            return ret;
        }
    }

    public struct SpectrumData
    {
        public float ms { get; }
        public float posY { get; set; }
        public GameObject SpectrumObject { get; }
        private Transform[] transforms;
        public SpectrumData(float ms, float pos, GameObject @object)
        {
            this.ms = ms;
            posY = pos;
            SpectrumObject = @object;
            transforms = new Transform[2];
            transforms[0] = SpectrumObject.transform.GetChild(0);
            transforms[1] = SpectrumObject.transform.GetChild(1);
        }
        public void UpdateScale(float[] values)
        {
            transforms[0].localScale = new Vector3(values[0], .5f, 1);
            transforms[1].localScale = new Vector3(values[1], .5f, 1);
        }
        public void UpdateScaleY(Vector3 vec3)
        {
            SpectrumObject.transform.localScale = vec3;
        }
    }
}