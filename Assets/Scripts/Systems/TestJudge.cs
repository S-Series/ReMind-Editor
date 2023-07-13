using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class TestJudge : MonoBehaviour
{
    public static List<TestJudge> s_TestJudges = new List<TestJudge>();
    private static readonly float[] noteJudgeRange = { 37.5f, 52.5f, 72.5f, 107.5f };

    public int JudgeSystemLine;
    private int noteIndex = 0;

    private bool isPressed = false;
    private bool isMultiple = false;
    private bool isHeated = false;

    private float noteMs = Mathf.Infinity;

    private List<NormalNote> normals = new List<NormalNote>();

    private void Awake()
    {
        s_TestJudges.Add(this);
        s_TestJudges = NoteField.SortJudgeSystem(s_TestJudges);
    }
    public void LostCheck(float nowMs)
    {
        if (nowMs - noteMs > noteJudgeRange[2])
        {

        }
    }
    public void SetNoteFileData(List<NormalNote> noteDatas)
    {
        normals = noteDatas;
        noteMs = normals.Count == 0 ? Mathf.Infinity : normals[0].ms;
    }

    private static void JudgeApply(int system, float dif)
    {

    }
    public void LoadNextNote()
    {

    }

    public void JudgeInput(bool isMain)
    {

    }
    public void JudgeOutput(bool isMain)
    {

    }
    public void JudgeSideInput()
    {

    }
    public void JudgeSideOutput()
    {

    }
}
