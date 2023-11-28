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

    private readonly static string[] Trigger = { "Play", "Start", "End" };

    private static int s_HolderIndex = 0;
    private static bool s_isTesting = false, s_isPause = false, s_isEffect = false;
    private static float s_Bpm = 120.0f, s_GameSpeed;
    private static float s_Ms = 0.0f, s_SpeedMs = 0.0f;
    private static float s_PosY = 0.0f, s_SpeedPosY = 0.0f, s_EffectPosY = 0.0f;
    private static NoteHolder s_TargetHolder;
    
    //# --------------------------------------------------
    private int ComboColorIndex;
    private int[] Combo = new int[4] { 0, 0, 0, 0 };

    private static Transform[] MovingField; //# <-----------<
    [SerializeField] private Transform[] _MovingField; //#--<

    [SerializeField] private InputAction[] inputActions;

    [SerializeField] private Animator[] judgeEffects;
    [SerializeField] private Animator[] gameJudgeEffects;
    [SerializeField] private AudioSource guideSound;
    [SerializeField] private AudioSource[] judgeSounds;
    [SerializeField] private AudioSource[] judgeLongSounds;
    [SerializeField] private Sprite[] ComboSprite;
    [SerializeField] private SpriteRenderer[] ComboRenderer;

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
            s_HolderIndex++;
            Judge(s_TargetHolder);
            s_TargetHolder = NoteField.s_noteHolders[s_HolderIndex];
        }

        _MovingField[0].localPosition 
            = new Vector3(0, s_isEffect ? s_EffectPosY : s_PosY, 0);
        _MovingField[1].localPosition 
            = new Vector3(0, (s_isEffect ? s_EffectPosY : s_PosY) * s_GameSpeed, 0);
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

        s_TargetHolder = NoteField.s_noteHolders.Find(item => item.stdMs > startMs);
    }
    public static void EndTest()
    {
        s_isTesting = false;
        InputManager.EnableInput(true);
        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EnableNote(true); }
        s_this.StopAllCoroutines();

        MusicLoader.audioSource.Play();
    }

    private void Judge(NoteHolder holder)
    {
        for (int i = 0; i < 4; i++)
        {
            if (holder.normals[i] != null)
            {
                if (holder.normals[i].length == 1)
                {
                    AddCombo();
                    judgeEffects[i].SetTrigger(Trigger[0]);
                    gameJudgeEffects[i].SetTrigger(Trigger[0]);
                }
                else { StartCoroutine(ILongNote(i, holder.longMs[i])); }
            }
            if (holder.airials[i] != null)
            {
                AddCombo();
                judgeEffects[i].SetTrigger(Trigger[0]);
                gameJudgeEffects[i + 4].transform.localPosition 
                    = new Vector3(0, holder.airials[i].length, 0);
                gameJudgeEffects[i + 4].SetTrigger(Trigger[0]);
            }

            if (i >= 2) { continue; }

            if (holder.bottoms[i] != null)
            {
                if (holder.bottoms[i].length == 1)
                {
                    AddCombo();
                    judgeEffects[i + 8].SetTrigger(Trigger[0]);
                    gameJudgeEffects[i + 8].SetTrigger(Trigger[0]);
                }
                else
                {

                }
            }
        }

        if (holder.speedNote != null)
        {
            s_Bpm = (float)holder.speedNote.getBpm();
            s_SpeedMs = holder.stdMs;
            s_SpeedPosY = holder.stdPos;
        }
        if (holder.effectNote != null)
        {
            s_isEffect = true;
        }
    }
    private void AddCombo()
    {
        Combo[3]++;

        if (Combo[3] > 9) { Combo[3]--; Combo[2]++; }
        if (Combo[2] > 9) { Combo[2]--; Combo[1]++; }
        if (Combo[1] > 9) { Combo[1]--; Combo[0]++; }

        for (int i = 0; i < 4; i++)
        {
            
        }
    }
    private void ResetCombo()
    {
        Combo = new int[4] { 0, 0, 0, 0 };
        ComboColorIndex = 0;
        ComboRenderer[0].sprite = ComboSprite[0];
        ComboRenderer[0].color = new Color32(125, 125, 125, 255);
    }

    //$ Testing Coroutines
    private static IEnumerator ITesting(int value)
    {
        s_Ms = value;
        while (true)
        {
            yield return null;
            s_Ms += Time.deltaTime * 1000;
            s_PosY = (s_Ms - s_SpeedMs) * s_Bpm / 150 + s_SpeedPosY;
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

    //$ LongNote, EffectNote, etc... Coroutines
    private IEnumerator ILongNote(int line, int[] datas)
    {
        judgeEffects[line].SetTrigger(Trigger[1]);
        gameJudgeEffects[line].SetTrigger(Trigger[1]);

        for (int i = 0; i < datas.Length;)
        {
            if (s_Ms > datas[i])
            {
                AddCombo();
                i++;
            }
            yield return null;
        }

        judgeEffects[line].SetTrigger(Trigger[2]);
        gameJudgeEffects[line].SetTrigger(Trigger[2]);
    }
    private IEnumerator IEffect(NoteHolder holder)
    {
        int EndMs;
        EndMs = holder.stdMs + holder.effectNote.value;

        while (s_Ms > EndMs)
        {
            yield return null;
        }
    }

    //$ Button Actions
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
