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

    private static int s_HolderIndex = 0;
    private static bool s_isTesting = false, s_isPause = false;
    private static float s_Ms = 0.0f;
    private static NoteHolder s_TargetHolder;

    private static Transform[] MovingField; //# <-----------<
    [SerializeField] private Transform[] _MovingField; //#--<

    [SerializeField] private InputAction[] inputActions;

    [SerializeField] private Animator[] judgeEffects;
    [SerializeField] private AudioSource guideSound;
    [SerializeField] private AudioSource[] judgeSounds;
    [SerializeField] private AudioSource[] judgeLongSounds;

    private void Awake()
    {
        if (s_this == null) { s_this = this; }
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
    private void Update()
    {
        if (!s_isTesting) { return; }

        if (s_TargetHolder == null) { return; }

        if (s_TargetHolder.stdMs >= s_Ms)
        {

        }
    }

    public static void StartTest(int pos)
    {
        if (NoteField.s_noteHolders.Count == 0) { return; }

        NoteGenerate.Escape();
        InputManager.EnableInput(false);
        NoteField.SortNoteHolder();

        int startMs, guideMs;
        startMs = NoteClass.CalMs(pos);
        guideMs = startMs - Mathf.RoundToInt(240000 / (float)ValueManager.s_Bpm);

        s_this.StartCoroutine(ITesting(guideMs));
        s_this.StartCoroutine(ITestGuide(startMs, guideMs));
        s_this.StartCoroutine(IPlayMusic(startMs));
    }
    public static void EndTest()
    {
        s_isTesting = false;
        InputManager.EnableInput(true);
        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EnableNote(true); }
        s_this.StopAllCoroutines();

        MusicLoader.audioSource.Play();
    }

    private static void Judge(NoteHolder holder)
    {

    }

    private static IEnumerator ITesting(int value)
    {
        s_Ms = value;
        while (true)
        {
            yield return null;
            s_Ms += Time.deltaTime * 1000;
        }
    }
    private static IEnumerator ITestGuide(int value, int G)
    {
        int D;
        D = (G - value) / 4;

        int[] guideMs = new int[4] { value, value + D, value + 2 * D, value + 3 * D };
        for (int i = 0; i < 4; i++)
        {
            yield return null;
            if (s_Ms >= guideMs[i])
            {
                s_this.guideSound.Play();
            }
        }
    }
    private static IEnumerator IPlayMusic(int startMs)
    {
        int musicMs;
        musicMs = startMs + ValueManager.s_Delay;

        while(true)
        {
            yield return null;
            if (s_Ms >= musicMs)
            {
                MusicLoader.audioSource.Play();
                break;
            }
        }
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
