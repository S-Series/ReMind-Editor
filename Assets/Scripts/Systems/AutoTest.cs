using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using GameNote;

public class AutoTest : MonoBehaviour
{
    private static AutoTest s_this;

    private static readonly string[] judgeEffectTrigger = {"100", "Start", "End"};

    public static int s_Index, s_Delay, s_TargetMs;
    public static float s_Ms, s_Timer;
    public static NoteHolder s_TargetHolder;
    public static bool s_isTesting = false, s_isPause = false, s_isEffect = false;

    private static List<NoteHolder> s_holders;
    private static float s_posY, s_bpm, s_bpmValue, s_EffectPosY, s_testSpeed = 1.0f;
    private static float s_SpeedMs, s_SpeedPos, s_OffsetMs;
    private static int[] s_Offset = new int[2];
    private static bool[] isTestAlive = {false};

    private static IEnumerator TestCoroutine;

    private static Transform[] MovingField; //# <-----------<
    [SerializeField] private Transform[] _MovingField; //#--<

    [SerializeField] private InputAction[] inputActions;

    [SerializeField] private Animator[] judgeEffects;
    [SerializeField] private AudioSource guideSound;
    [SerializeField] private AudioSource[] judgeSounds;
    [SerializeField] private AudioSource[] judgeLongSounds;
    [SerializeField] private Material[] materials;
    [SerializeField] private TMP_InputField[] inputFields;

    private void Awake()
    {
        s_this = this;
        TestCoroutine = IStartTest();
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
                _time = s_Ms + ValueManager.s_Delay;
                if (_time < 0)
                {
                    s_Ms += (int)(_time);
                    _time += _time;
                }
                MusicLoader.audioSource.time = _time;
                MusicLoader.audioSource.Play();
                s_isPause = false;
            }
            else
            {
                MusicLoader.audioSource.Stop();
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

        s_Timer += Time.fixedDeltaTime * 1000;
        s_Ms = s_Timer * s_testSpeed + s_Delay;
    }
    private void Update()
    {
        if (!s_isTesting) { return; }

        s_posY = (s_SpeedPos + (s_Ms - s_Offset[1] - s_SpeedMs) * s_bpmValue) / 160f;

        //# Note Judge
        if (isTestAlive[0])
        {
            if (s_TargetMs <= s_Ms)
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
        s_this.StartCoroutine(IMoveField());

        NoteField.SortNoteHolder();
        s_holders = new List<NoteHolder>();
        s_holders = NoteField.s_noteHolders;

        if (pos == 0)
        {
            s_Ms = -5000;
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
            
        s_bpm = System.Convert.ToSingle(ValueManager.s_Bpm);
        s_Delay = ValueManager.s_Delay;
        s_bpmValue = s_bpm / 150f;

        s_posY = 0.0f;
        s_EffectPosY = 0.0f;

        NoteField.s_Zoom = 10;
        NoteField.s_this.UpdateField();
        s_this.StopCoroutine(TestCoroutine);
        TestCoroutine = IStartTest();
        s_this.StartCoroutine(TestCoroutine);
    }
    public static void EndTest()
    {
        s_isTesting = false;
        InputManager.EnableInput(true);
        for (int i = 0; i < isTestAlive.Length; i++) { isTestAlive[i] = false; }
        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EnableNote(true); }
        foreach (TMP_InputField inputField in s_this.inputFields) { inputField.interactable = true; }
        s_this.StopAllCoroutines();

        MusicLoader.audioSource.Play();
    }
    private static void JudgeApply(NoteHolder holder)
    {
        //# NormalNoteApply(normal & airial)
        for (int i = 0; i < 4; i++)
        {
            NormalNoteApply(holder.normals[i]);
            NormalNoteApply(holder.airials[i]);
        }
        NormalNoteApply(holder.bottoms[0]);
        NormalNoteApply(holder.bottoms[1]);

        SppedNoteApply(holder.speedNote);
        EffectNoteApply(holder.effectNote);

        if (s_Index == s_holders.Count)
        {
            s_TargetHolder = null;
            isTestAlive[0] = false;
        }
        else
        {
            s_TargetHolder = s_holders[s_Index];
            s_TargetMs = s_TargetHolder.stdMs;
        }
    }
    
    private static void NormalNoteApply(NormalNote note)
    {
        if (note == null) { return; }

        if (note.length != 1) { s_this.StartCoroutine(ILongJudgeEfect(note)); }

        s_this.judgeSounds[note.line - 1].Play();
        s_this.judgeEffects[note.line - 1].SetTrigger(judgeEffectTrigger[0]);
    }
    private static void SppedNoteApply(SpeedNote note)
    {
        if (note == null) { return; }
        
        s_SpeedMs = note.ms;
        s_SpeedPos = note.pos;
        s_bpm = Convert.ToSingle(note.bpm * note.multiple);
    }
    private static void EffectNoteApply(EffectNote note)
    {
        if (note == null) { return; }

    }

    private static void ChangeTestSpeed(float multiple)
    {
        s_testSpeed = multiple;
        MusicLoader.audioSource.pitch = multiple;
    }

    private static IEnumerator IStartTest()
    {
        s_isTesting = true;     //$ Start Ms = -5000;

        float[] guideCount = new float[4];
        for (int i = 0; i < 4; i++)
        {
            guideCount[i] = (60000f / s_bpm) * (4 - i);
        }

        int index = 0;
        while (true)
        {
            if (guideCount[index] <= s_Ms)
            {
                index++;
                s_this.guideSound.Play();
                if (index == 4) { break; }
            }
            yield return null;
        }

        // s_Timer = 0.0f;
        // print(60.000f / s_bpm);
        // var wait = new WaitForSeconds(60.000f / s_bpm);
        // MusicBox.audioSource.Stop();
        // MusicBox.audioSource.time = 0.0f;
        // 
        // yield return null;
        // 
        // for (int i = 0; i < 4; i++)
        // {
        //     s_this.guideSound.Play();
        //     yield return wait;
        // }
        // s_isTesting = true;
        // MusicBox.audioSource.Play();
        // yield return null;
        // s_Timer = 0.0f;
    }
    private static IEnumerator IMoveField()
    {
        while (true)
        {
            if (s_isEffect) { s_posY = s_EffectPosY; }
            else {  }

            MovingField[0].localPosition 
                = new Vector3(-.5f, -5 - (s_isEffect ? s_EffectPosY : s_posY), 0);
            MovingField[1].localPosition 
                = new Vector3(25f, -31.3f, -19 - (s_isEffect ? s_EffectPosY : s_posY));
            yield return null;
        }
    }
    private static IEnumerator ILongJudgeEfect(NormalNote note)
    {
        int ms, index;
        ms = note.ms + Mathf.RoundToInt(7500 / s_bpm);
        index = note.line - 1;
        s_this.judgeEffects[index].SetTrigger(judgeEffectTrigger[0]);
        for (int i = 1; i < note.length; i++)
        {
            while (true)
            {
                if (s_Ms >= ms)
                {
                    print(String.Format("{0} : {1}", ms, s_Ms));
                    ms += Mathf.RoundToInt(7500 / s_bpm);
                    s_this.judgeLongSounds[index].Play();
                    s_this.judgeEffects[index].SetTrigger(judgeEffectTrigger[0]);
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
        else { StartTest(NoteField.s_StartPos); }
    }
    public void Btn_TestPlay()
    {
        //# Not Approveds
    }
}
