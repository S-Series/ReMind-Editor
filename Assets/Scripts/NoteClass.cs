using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using GameNote;

namespace GameNote
{
    public enum NoteType { None = 0, Normal = 1, Airial = 2, Scratch = 3, Speed = 4, Effect = 5 }
    public class NormalNote
    {
        public int posY, line, length;
        public bool isAirial;
        public NormalNote(int[] values, bool isAirial)
        {
            posY = values[0];
            line = values[1];
            length = values[2];
            this.isAirial = isAirial;
        }
    }
    public class ScratchNote
    {
        public int posY, length;
        public int startX, powerX, endX;
        public bool isPower;
        public ScratchNote(int _posY, int _length, int[] valueXs, bool _isPower = false)
        {
            posY = _posY;
            length = _length;
            startX = valueXs[0];
            powerX = valueXs[1];
            endX = valueXs[2];
            isPower = _isPower;
        }
    }
    public class SpeedNote
    {
        public static List<SpeedNote> speedNotes = new List<SpeedNote>();
        public int posY;
        public float ms;
        public double bpm, multiple;
        public SpeedHolder holder;

        public SpeedNote(int value)
        {
            posY = value;
            bpm = ValueManager.s_Bpm;
            multiple = 1.0d;
        }
        public SpeedNote(int posY, double bpm, double multiple)
        {
            this.posY = posY;
            this.bpm = bpm;
            this.multiple = multiple;
        }
    }
    public class EffectNote
    {
        public int posY, effectIndex, value;
        public EffectHolder holder;
        public string GetEffectName()
        {
            switch (effectIndex)
            {
                case 1: return "None";

                default: return "None";
            }
        }
        public EffectNote(int _value)
        {
            posY = _value;
            value = 0;
            effectIndex = 0;
        }
        public EffectNote(int posY, int index, int value)
        {
            this.posY = posY;
            effectIndex = index;
            this.value = value;
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
            SpectrumObject.transform.localPosition = new Vector3(0, posY, 0);
            transforms = new Transform[2];
            transforms[0] = SpectrumObject.transform.GetChild(0);
            transforms[1] = SpectrumObject.transform.GetChild(1);
            EnableObject(false);
        }
        public void UpdatePosY(float value)
        {
            posY = value;
            SpectrumObject.transform.localPosition = new Vector3(0, posY, 0);
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
        public void EnableObject(bool isEnable)
        {
            SpectrumObject.SetActive(isEnable);
        }
    }
}

public class NoteClass : MonoBehaviour
{
    public static float PosToMs(int pos)
    {
        double ret;
        var speeds = SpeedNote.speedNotes;

        if (pos <= 0) { ret = 0; }
        else if (speeds.Count == 0) { ret = 150f * pos / ValueManager.s_Bpm; }
        else
        {
            int index;
            index = speeds.FindLastIndex(item => item.posY <= pos);

            if (index == -1) { ret = 150f * pos / ValueManager.s_Bpm; }
            else
            {
                SpeedNote target;
                target = speeds[index];
                ret = target.ms + 150f * (pos - target.posY) / target.bpm;
            }
        }
        return (Single)ret;
    }
    public static float MsToPos(float Ms)
    {
        double ret;
        var speeds = SpeedNote.speedNotes;

        if (speeds.Count == 0) { ret = ValueManager.s_Bpm * Ms / 150f; }
        else
        {
            int index;
            index = speeds.FindLastIndex(item => item.ms <= Ms);

            if (index == -1) { ret = ValueManager.s_Bpm * Ms / 150f; }
            else
            {
                SpeedNote target;
                target = speeds[index];
                ret = target.posY + target.bpm * (Ms - target.ms) / 150f;
            }
        }
        return (Single)ret;
    }
    public static void InitSpeedMs()
    {
        NoteHolder holder;
        var speeds = SpeedNote.speedNotes;
        speeds = new List<SpeedNote>();
        for (int i = 0; i < NoteHolder.s_holders.Count; i++)
        {
            holder = NoteHolder.s_holders[i];
            if (holder.speedNote != null) { speeds.Add(holder.speedNote); }
        }

        int lastMs;
        float pos;
        double bpm;

        lastMs = Mathf.RoundToInt(150f * speeds[0].posY / ValueManager.s_Bpm);
        speeds[0].ms = lastMs;

        for (int i = 1; i < speeds.Count; i++)
        {
            bpm = speeds[i - 1].bpm;
            pos = speeds[i].posY - speeds[i - 1].posY;
            lastMs += Mathf.RoundToInt(150f * pos / (Single)bpm);
            speeds[i].ms = lastMs;
        }

        for (int i = 0; i < NoteHolder.s_holders.Count; i++)
        {
            holder = NoteHolder.s_holders[i];
            if (holder.speedNote == null)
            {

            }
            else
            {

            }
        }
    }
}