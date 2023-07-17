using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Judge;
using GameNote;

public class TestJudge : MonoBehaviour
{
    public static List<TestJudge> s_TestJudges = new List<TestJudge>();
    private static readonly float[] noteJudgeRange = { 37.5f, 52.5f, 72.5f, 107.5f };
    private static readonly string[] noteAnimateTag = { "", "", "", "" };

    public int JudgeSystemLine;
    private int noteIndex = 0;
    private bool isPressed = false, isMultiple = false, isHeated = false;
    private float noteMs = Mathf.Infinity;
    private NormalNote normal;
    [SerializeField] Animator judgeEffect;

    private List<NormalNote> normals = new List<NormalNote>();



    private void Awake()
    {
        s_TestJudges.Add(this);
        s_TestJudges = NoteField.SortJudgeSystem(s_TestJudges);
    }
    public void LostCheck(float testMs)
    {
        if (testMs - noteMs > noteJudgeRange[2])
        {

        }
    }
    public void SetNoteFileData(List<NormalNote> noteDatas)
    {
        normals = noteDatas;
        noteMs = normals.Count == 0 ? Mathf.Infinity : normals[0].ms;
    }

    private Judgetype JudgeCheck(float dif)
    {
        return Judgetype.None;
    }
    private void JudgeApply(bool isPositive, Judgetype type)
    {
        switch (type)
        {
            //# if (type == None) return;
            case Judgetype.None:
                return;

            case Judgetype.Perfect:
                judgeEffect.SetTrigger(noteAnimateTag[0]);
                break;

            case Judgetype.Pure:
                judgeEffect.SetTrigger(noteAnimateTag[1]);
                break;

            case Judgetype.Near:
                judgeEffect.SetTrigger(noteAnimateTag[2]);
                break;

            case Judgetype.Lost:
                judgeEffect.SetTrigger(noteAnimateTag[3]);
                break;
        }
    }
    public void LoadNextNote()
    {
        noteIndex++;
        if (noteIndex >= normals.Count)
        {
            normal = null;
            noteMs = Mathf.Infinity;
        }
        else
        {
            normal = normals[noteIndex];
            noteMs = normal.ms;
        }
    }

    public void JudgeInput(bool isMain)
    {
        float dif;
        dif = TestPlay.s_TestMs - noteMs;
        JudgeApply(dif > 0 ? true : false, JudgeCheck(dif)); 

        if (isMain) { isPressed = true; }
        else { isMultiple = true; }
    }
    public void JudgeOutput(bool isMain)
    {
        if (isMain) { isPressed = false; }
        else { isMultiple = false; }
    }
    public void JudgeSideInput()
    {

    }
    public void JudgeSideOutput()
    {

    }
}
