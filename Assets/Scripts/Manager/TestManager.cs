using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

namespace GameSystem
{
    public enum Judgetype { Perfect = 0, Pure = 1, Near = 2, Lost = 3, None = 4 };
    public enum LineType { Line_1 = 1, Line_2 = 2, Line_3 = 3, Line_4 = 4, Side_L = 5, Side_R = 6 };
    
    public struct HolderPackage
    {
        public NoteHolder noteHolder { get; }
        public GameNoteHolder gameNoteHolder { get; }
        public HolderPackage(NoteHolder x, GameNoteHolder y)
        {
            noteHolder = x;
            gameNoteHolder = y;
        }
    }
    public struct JudgePackage
    {
        public LineType line { get; }
        public int length { get; }
        public bool isAirial { get; }
        public float JudgeMs { get; }
        public HolderPackage holders { get; }
        public JudgePackage(float ms, HolderPackage _holder, int _line, int _length, bool isAir)
        {
            length = _length;
            isAirial = isAir;
            JudgeMs = ms;
            holders = _holder;
            line = (LineType)_line;
        }
    }
    
    public static class JudgeSystem
    {
        private static readonly float[] JudgeRange = { 37.5f, 52.5f, 72.5f, 107.5f };

        public static Judgetype CheckJudge(float value)
        {
            Judgetype ret;
            bool isPositive;

            isPositive = value < 0 ? false : true;
            if (!isPositive) { value = Mathf.Abs(value); }

            if (value < JudgeRange[0]) { ret = Judgetype.Perfect; }     //$ 100 + 1
            else if (value < JudgeRange[1]) { ret = Judgetype.Pure; }   //$ 100
            else if (value < JudgeRange[2]) { ret = Judgetype.Near; }   //$ 50
            else if (value < JudgeRange[3] && isPositive) { ret = Judgetype.Lost; }
            else { ret = Judgetype.None; }

            return ret;
        }
    }
}

public class TestManager : MonoBehaviour
{
    private static TestManager s_this;

    private void Awake()
    {
        if (s_this == null) { s_this = this; }
    }

    
}
