using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using GameNote;

public class AutoTest : MonoBehaviour
{
    private static AutoTest s_this;

    public static bool s_isTesting = false;

    private readonly static string[] Trigger = { "100", "Start", "End" };

    private static int s_HolderIndex = 0;
    private static bool s_isPause = false, s_isEffect = false;
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
    [SerializeField] public AudioSource[] judgeSounds;
    [SerializeField] public AudioSource[] judgeLongSounds;
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
            if (!s_isTesting) { return; }
            if (s_isPause)
            {
                s_isPause = false;
                MusicLoader.audioSource.Play();
            }
            else
            {
                s_isPause = true;
                MusicLoader.audioSource.Pause();
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

        _MovingField[0].localPosition
            = new Vector3(-.5f, s_PosY / -320f - 5f, 0);
        _MovingField[1].localPosition
            = new Vector3(-.5f, s_PosY / -320f - 5f, 0);
        _MovingField[2].localPosition
            = new Vector3(25, -31.3f, -19f - s_PosY / 160f);
        SpectrumManager.UpdatePosY(-s_PosY);

        if (s_TargetHolder == null) { return; }

        if (s_Ms >= s_TargetHolder.stdMs)
        {
            s_HolderIndex++;
            Judge(s_TargetHolder);
            s_TargetHolder = s_HolderIndex == NoteField.s_noteHolders.Count 
                ? null : NoteField.s_noteHolders[s_HolderIndex];
        }
    }

    public static void StartTest(int pos)
    {
        if (NoteField.s_noteHolders.Count == 0) { return; }

        NoteGenerate.Escape();
        InputManager.EnableInput(false);
        NoteField.SortNoteHolder();
        NoteField.InitAllHolder();
        NoteField.s_isFieldMovable = false;
        foreach (NoteHolder holder
            in NoteField.s_noteHolders) { holder.EnableCollider(false); }

        s_Bpm = ValueManager.s_Bpm;
        s_HolderIndex = 0;

        int startMs, guideMs;
        startMs = NoteClass.CalMs(pos);
        guideMs = startMs - Mathf.RoundToInt(240000 / (float)ValueManager.s_Bpm);

        s_isTesting = true;
        s_isPause = false;

        s_this.StartCoroutine(ITesting(guideMs));
        s_this.StartCoroutine(ITestGuide(guideMs, pos));
        s_this.StartCoroutine(IPlayMusic(startMs));

        s_HolderIndex = NoteField.s_noteHolders.FindIndex(item => item.stdMs >= startMs);
        s_TargetHolder = NoteField.s_noteHolders[s_HolderIndex];

        foreach (InputAction action in s_this.inputActions) { action.Enable(); }
    }
    public static void EndTest()
    {
        s_isTesting = false;
        s_isPause = false;
        InputManager.EnableInput(true);
        NoteField.s_isFieldMovable = true;

        foreach (NoteHolder holder in NoteField.s_noteHolders)
        {
            holder.EnableNote(true);
            holder.EnableCollider(true);
            holder.gameNoteHolder.UpdateNote();
        }
        s_this.StopAllCoroutines();

        MusicLoader.audioSource.Stop();
        foreach (InputAction action in s_this.inputActions) { action.Disable(); }
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
                    judgeSounds[i].Play();
                }
                else { StartCoroutine(ILongNote(i, holder.longMs[i])); }
            }
            if (holder.airials[i] != null)
            {
                AddCombo();
                judgeEffects[i].SetTrigger(Trigger[0]);
                gameJudgeEffects[i + 4].transform.localPosition 
                    = new Vector3(240 * i - 360, holder.airials[i].length * 3.5f, 0);
                gameJudgeEffects[i + 4].SetTrigger(Trigger[0]);
                    judgeSounds[i].Play();
            }

            if (i >= 2) { continue; }

            if (holder.bottoms[i] != null)
            {
                if (holder.bottoms[i].length == 1)
                {
                    AddCombo();
                    judgeEffects[i + 4].SetTrigger(Trigger[0]);
                    gameJudgeEffects[i + 8].SetTrigger(Trigger[0]);
                    judgeSounds[i + 4].Play();
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

        holder.gameNoteHolder.JudgeVisual();
    }
    private void AddCombo()
    {
        Combo[3]++;

        if (Combo[3] > 9) { Combo[3] -= 10; Combo[2]++; }
        if (Combo[2] > 9) { Combo[2] -= 10; Combo[1]++; }
        if (Combo[1] > 9) { Combo[1] -= 10; Combo[0]++; }

        for (int i = 0; i < 4; i++)
        {
            ComboRenderer[i].sprite = ComboSprite[Combo[i]];
        }
    }
    private void ResetCombo()
    {
        Combo = new int[4] { 0, 0, 0, 0 };
        ComboColorIndex = 0;
        ComboRenderer[0].sprite = ComboSprite[0];
        ComboRenderer[0].color = new Color32(125, 125, 125, 255);
    }

    public static AudioSource[] GetJudgeAudioSource()
    {
        AudioSource[] ret;
        ret = s_this.judgeSounds;
        ret.Concat(s_this.judgeLongSounds);
        return ret;
    }
    
    //$ Testing Coroutines
    private static IEnumerator ITesting(int value)
    {
        s_Ms = value;
        while (true)
        {
            yield return null;
            s_PosY = s_isEffect ? s_EffectPosY : (s_Ms - s_SpeedMs) * s_Bpm / 150 + s_SpeedPosY;
            
            if (s_isPause) { continue; }
            
            s_Ms += Time.deltaTime * 1000;
        }
    }
    private static IEnumerator ITestGuide(int startMs, int startPos)
    {
        int index;
        SpeedNote note;
        index = NoteClass.s_SpeedNotes.FindIndex(item => item.pos > startPos);
        note = index <= 0 ? null : NoteClass.s_SpeedNotes[index - 1];     

        int duration;
        duration = Mathf.RoundToInt(150 * 400 / (note == null ? ValueManager.s_Bpm : (float)note.bpm));

        int[] guideMs = new int[4];
        for (int i = 0; i < 4; i++) { guideMs[i] = startMs + i * duration; }
        for (int i = 0; i < 4;)
        {
            print(guideMs[i]);
            yield return null;
            if (s_Ms >= guideMs[i])
            {
                i++;
                s_this.guideSound.Play();
            }
        }
    }
    private static IEnumerator IPlayMusic(int startMs)
    {
        float delay;
        delay = ValueManager.s_Delay;
        MusicLoader.audioSource.time = (delay + startMs) / 1000f;
        while(true)
        {
            yield return null;
            if (s_Ms > startMs)
            {
                MusicLoader.audioSource.Play();
                break;
            }
        }
        yield break;
        var wait = new WaitForSeconds(1.0f);
        while (true)
        {
            yield return wait;
            MusicLoader.audioSource.time = (delay + s_Ms) / 1000f;
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
        if (SpectrumManager.isGenerating) { return; }

        if (s_isTesting) { EndTest(); }
        else { StartTest(0); }
    }
    public void Btn_MidTest()
    {
        if (SpectrumManager.isGenerating) { return; }

        if (s_isTesting) { EndTest(); }
        else { StartTest(NoteField.s_StartPos); }
    }
    public void Btn_TestPlay()
    {
        //# Not Approveds
    }

}
