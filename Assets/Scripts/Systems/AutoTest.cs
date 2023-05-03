using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using GameNote;

public class AutoTest : MonoBehaviour
{
    private static AutoTest s_this;

    private static readonly string[] judgeEffectTrigger = {"Play", "Start", "End"};

    public static int s_Index, s_Ms, s_TargetMs;
    public static NoteHolder s_TargetHolder;
    public static bool s_isTesting = false, s_isPause = false;

    private static List<NoteHolder> s_holders;
    private static int s_SpeedMs, s_SpeedPos, s_EffectMs, s_EffectPos, s_OffsetMs;
    private static int[] s_Offset = new int[2];
    private static bool[] isTestAlive = {false};
    private static float s_posY, s_bpm, s_bpmValue;

    private static Transform[] MovingField; //# 艾式式式式式式式式式式式式忖
    [SerializeField] private Transform[] _MovingField; //# 式式戎

    [SerializeField] private InputAction[] inputActions;

    [SerializeField] private Animator[] judgeEffects;
    [SerializeField] private AudioSource[] judgeSounds;
    [SerializeField] private AudioSource[] judgeLongSounds;
    [SerializeField] private Material[] materials;
    [SerializeField] private TMP_InputField[] inputFields;

    private void Awake()
    {
        s_this = this;
    }
    private void Start()
    {
        MovingField = _MovingField;

        //# Space
        inputActions[0].performed += item =>
        {
            if (s_isPause)
            {
                float _time;
                _time = s_Ms + ValueManager.s_delay;
                if (_time < 0)
                {
                    s_Ms += (int)(_time);
                    _time += _time;
                }
                MusicBox.audioSource.time = _time;
                MusicBox.audioSource.Play();
                s_isPause = false;
            }
            else
            {
                MusicBox.audioSource.Stop();
                s_isPause = true;
            }
        };
        //# UpArrow
        inputActions[1].performed += item =>
        {
            if (!s_isPause) { return; }
            s_Ms += 100;
        };
        //# DownArrow
        inputActions[2].performed += item =>
        {
            if (!s_isPause) { return; }
            s_Ms -= 100;
        };
    }
    private void FixedUpdate()
    {
        if (!s_isTesting) { return; }
        if (s_isPause) { return; }
        s_Ms++;
    }
    private void Update()
    {
        if (!s_isTesting) { return; }
        s_OffsetMs = s_Ms - s_Offset[1]; //$ Judge Offset Ms

        //# Note Field Movement
        s_posY = (s_SpeedPos + s_EffectPos
            + (s_OffsetMs - s_SpeedMs - s_EffectMs) * s_bpmValue) / 160f;

        //# Note Judge
        if (isTestAlive[0])
        {
            if (s_TargetMs <= s_OffsetMs)
            {
                s_Index++;
                JudgeApply(s_TargetHolder);
            }
        }
    }

    public static void StartTest(int pos)
    {
        if (NoteField.s_noteHolders.Count == 0) { return; }

        for (int i = 0; i < isTestAlive.Length; i++) { isTestAlive[i] = true; }
        foreach (TMP_InputField inputField in s_this.inputFields) { inputField.interactable = false; }

        NoteGenerate.Escape();
        InputManager.EnableInput(false);
        s_this.StartCoroutine(MoveField());

        NoteField.SortNoteHolder();
        s_holders = new List<NoteHolder>();
        s_holders = NoteField.s_noteHolders;

        if (pos == 0)
        {
            s_Index = 0;
            s_TargetHolder = s_holders[0];
        }
        else
        {
            NoteHolder calHolder;
            s_TargetHolder = null;

            for (int i = 0; i < s_holders.Count; i++)
            {
                if (s_holders[i].stdPos > pos)
                {
                    s_Index = i;
                    s_TargetHolder = s_holders[i];
                    if (i == 0) { calHolder = null; }
                    else { calHolder = s_holders[i - 1]; }
                }
            }
        }
        s_TargetMs = s_TargetHolder.stdMs;
            
        s_Ms = ValueManager.s_delay;
        s_bpm = System.Convert.ToSingle(ValueManager.s_Bpm);
        s_bpmValue = s_bpm / 150f;

        s_this.StartCoroutine(IStartTest(ValueManager.s_delay));
    }
    public static void EndTest()
    {
        InputManager.EnableInput(true);
        for (int i = 0; i < isTestAlive.Length; i++) { isTestAlive[i] = false; }
        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EnableNote(true); }
        foreach (TMP_InputField inputField in s_this.inputFields) { inputField.interactable = true; }
        s_this.StopAllCoroutines();

        MusicBox.audioSource.Play();
    }
    private static void JudgeApply(NoteHolder holder)
    {
        for (int i = 0; i < 4; i++)
        {
            JudgeEffect(holder.normals[i]);
            JudgeEffect(holder.airials[i]);
        }

        JudgeEffect(holder.bottoms[0]);
        JudgeEffect(holder.bottoms[1]);

        if (holder.speedNote != null)
        {
            s_bpm = System.Convert.ToSingle(holder.speedNote.bpm * holder.speedNote.multiple);
        }
        if (holder.effectNote != null)
        {
            print("Not Available");
        }

        if (s_Index == s_holders.Count) { s_TargetHolder = null; isTestAlive[0] = false; }
        else { s_TargetHolder = s_holders[s_Index]; }
    }
    private static void JudgeEffect(NormalNote note)
    {
        if (note == null) { return; }

        if (note.legnth != 1) { s_this.StartCoroutine(ILongJudgeEfect(note)); }
    }

    private static IEnumerator IStartTest(int delay)
    {
        print("run");
        s_isTesting = true;
        if (delay < 0) { MusicBox.audioSource.time = -delay / 1000f; }
        else { yield return new WaitForSeconds(delay / 1000f); }
        MusicBox.audioSource.Play();
    }
    private static IEnumerator MoveField()
    {
        while (true)
        {
            MovingField[0].localPosition = new Vector3(-.5f, -5 - s_posY, 0);
            MovingField[1].localPosition = new Vector3(25, -31.3f, -19 - s_posY);
            yield return null;
        }
    }
    private static IEnumerator ILongJudgeEfect(NormalNote note)
    {
        int ms, index;
        ms = note.ms + Mathf.RoundToInt(30000 / s_bpm);
        index = note.line;
        s_this.judgeEffects[index].SetTrigger(judgeEffectTrigger[1]);
        for (int i = 1; i < note.legnth; i++)
        {
            while (true)
            {
                if (s_Ms >= ms)
                {
                    ms += Mathf.RoundToInt(30000 / s_bpm);
                    s_this.judgeLongSounds[index].Play();
                    break;
                }
                yield return null;
            }
        }
        s_this.judgeEffects[index].SetTrigger(judgeEffectTrigger[2]);
    }

    public void Btn_StartTest()
    {
        if (s_isTesting) { EndTest(); }
        else { StartTest(0); }
    }
    public void Btn_MidTest()
    {
        if (s_isTesting) { EndTest(); }
        else { StartTest(0); }
    }
    public void Btn_TestPlay()
    {
        //# Not Approveds
    }
}
