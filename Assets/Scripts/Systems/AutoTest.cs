using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using GameNote;
using GameData;

public class AutoTest : MonoBehaviour
{
    private static AutoTest s_this;

    public static float s_Ms = 0.0f;
    public static bool s_isTesting = false;

    //private readonly static string[] Trigger = { "100", "Start", "End" };
    private readonly static string[] s_Trigger = { "100", "100", "100" };

    private static int s_HolderIndex = 0;
    private static bool s_isPause = false, s_isEffect = false;
    private static float s_Bpm = 120.0f, s_GameSpeed;
    private static float s_SpeedMs = 0.0f;
    private static float s_PosY = 0.0f, s_SpeedPosY = 0.0f, s_EffectPosY = 0.0f;
    private static NoteHolder s_TargetHolder;
    
    //# --------------------------------------------------
    private int ComboColorIndex;
    private int[] Combo = new int[4] { 0, 0, 0, 0 };

    private static Transform[] MovingField; //# <-----------<
    [SerializeField] private Transform[] _MovingField; //#--<

    [SerializeField] private InputAction[] inputActions;

    //# --------------------------------------------------
    private static Animator[][] judgeEffects;
    [SerializeField] Animator[] judgeEffects_A;
    [SerializeField] Animator[] judgeEffects_B;
    [SerializeField] Animator[] judgeEffects_C;
    private static Animator[][] gameJudgeEffects;
    [SerializeField] Animator[] gameJudgeEffects_A;
    [SerializeField] Animator[] gameJudgeEffects_B;
    [SerializeField] Animator[] gameJudgeEffects_C;

    //# --------------------------------------------------
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

        judgeEffects = new Animator[][]
        {
            judgeEffects_A,
            judgeEffects_B,
            judgeEffects_C
        };
        gameJudgeEffects = new Animator[][]
        {
            gameJudgeEffects_A,
            gameJudgeEffects_B,
            gameJudgeEffects_C
        };

        //# Space
        inputActions[0].performed += item =>
        {
            if (!s_isTesting) { return; }
            if (s_isPause)
            {
                s_isPause = false;
                MusicLoader.audioSource.time = (ValueManager.s_Delay + s_Ms) / 1000f;
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
            s_TargetHolder = s_HolderIndex == NoteHolder.s_holders.Count 
                ? null : NoteHolder.s_holders[s_HolderIndex];
        }
    }

    public static void StartTest(int pos)
    {
        NoteGenerate.Escape();
        NoteField.SortNoteHolder();
        NoteField.s_isFieldMovable = false;

        foreach (NoteHolder holder
            in NoteHolder.s_holders) { holder.EnableCollider(false); }

        ObjectCooling.ResetLastIndex();

        s_Bpm = ValueManager.s_Bpm;
        s_HolderIndex = 0;

        float startMs, guideMs;
        startMs = NoteClass.PosToMs(pos);
        guideMs = startMs - Mathf.RoundToInt(240000 / (float)ValueManager.s_Bpm);

        s_isTesting = true;
        s_isPause = false;

        s_this.StartCoroutine(ITesting(guideMs));
        s_this.StartCoroutine(ITestGuide(guideMs, pos));
        s_this.StartCoroutine(IPlayMusic(startMs));

        s_HolderIndex = NoteHolder.s_holders.FindIndex(item => item.stdMs >= startMs);
        s_TargetHolder = s_HolderIndex == -1 ? null : NoteHolder.s_holders[s_HolderIndex];

        foreach (InputAction action in s_this.inputActions) { action.Enable(); }
    }
    public static void EndTest()
    {
        s_isTesting = false;
        s_isPause = false;
        NoteField.s_isFieldMovable = true;

        foreach (NoteHolder holder in NoteHolder.s_holders)
        {
            holder.EnableNote(false);
            holder.EnableCollider(true);
            holder.gameNoteHolder.UpdateNote();
        }
        s_this.StopAllCoroutines();

        ObjectCooling.UpdateCooling();
        MusicLoader.audioSource.Stop();
        foreach (InputAction action in s_this.inputActions) { action.Disable(); }
    }

    private void Judge(NoteHolder holder)
    {
        int[][] data;
        data = holder.ApplyJudge();
        for (int i = 0; i < 6; i++)
        {
            if (data[0][i] > 0)
            {
                judgeEffects[0][i].SetTrigger(s_Trigger[0]);
                StartCoroutine
                (ILongNote(
                    NoteType.Normal,
                    new int[3]{holder.stdPos, i, data[0][i]},
                    holder.longMs[i]
                ));
            }
            if (data[1][i] > 0)
            {
                judgeEffects[1][i].SetTrigger(s_Trigger[0]);
            }
            if (i > 1) { continue; }
            if (data[2][i] > 0)
            {
                judgeEffects[2][i].SetTrigger(s_Trigger[0]);
                StartCoroutine
                (ILongNote(
                    NoteType.Floor,
                    new int[3]{holder.stdPos, i, data[0][i]},
                    holder.longMs[i + 6]
                ));
            }
        }
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

    public static void ApplyGameMode(GameMode mode)
    {
        
    }
    public static AudioSource[] GetJudgeAudioSource()
    {
        AudioSource[] ret;
        ret = s_this.judgeSounds;
        ret.Concat(s_this.judgeLongSounds);
        return ret;
    }
    
    
    //$ Testing Coroutines
    private static IEnumerator ITesting(float value)
    {
        float delay;
        delay = ValueManager.s_Delay;

        AudioSource audio;
        audio = MusicLoader.audioSource;

        s_Ms = value;
        while (true)
        {
            yield return null;
            s_Ms += Time.deltaTime * 1000f;
            s_PosY = s_Ms * s_Bpm / 150f;
            ObjectCooling.UpdateTestCooling(s_PosY);
        }
    }
    private static IEnumerator ITestGuide(float startMs, float startPos)
    {
        int index;
        SpeedNote note;
        index = SpeedNote.speedNotes.FindIndex(item => item.posY > startPos);
        note = index <= 0 ? null : SpeedNote.speedNotes[index - 1];     

        int duration;
        duration = Mathf.RoundToInt(150 * 400 / (note == null ? ValueManager.s_Bpm : (float)note.bpm));

        float[] guideMs = new float[4];
        for (int i = 0; i < 4; i++) { guideMs[i] = startMs + i * duration; }
        for (int i = 0; i < 4;)
        {
            yield return null;
            if (s_Ms >= guideMs[i])
            {
                i++;
                s_this.guideSound.Play();
            }
        }
    }
    private static IEnumerator IPlayMusic(float startMs)
    {
        MusicLoader.audioSource.time = (ValueManager.s_Delay + startMs) / 1000f;
        while(true)
        {
            yield return null;
            if (s_Ms > startMs)
            {
                MusicLoader.audioSource.Play();
                break;
            }
        }
    }

    //$ LongNote, EffectNote, etc... Coroutines
    /// <param name="datas"> datas = int[3] {pos, line, length} </param>
    private IEnumerator ILongNote(NoteType type, int[] datas, int[] judges)
    {
        if (datas[1] > 5) { yield break; }
        var @struct = new float[2]
        {
            NoteClass.PosToMs(datas[0]),
            NoteClass.PosToMs(datas[0] + datas[2] * 100)
        };
        LongJudgeVisualize.s_LJV[0][datas[1] - 1].StartLongVisualize(datas[2], @struct);
        LongJudgeVisualize.s_LJV[1][datas[1] - 1].StartLongVisualize(datas[2], @struct);

        for (int i = 0; i < datas[2];)
        {
            if (s_Ms > judges[i])
            {
                //! Debug Code
                //! end
                AddCombo();
                i++;
            }
            yield return null;
        }
    }
    private IEnumerator IEffect(NoteHolder holder)
    {
        float EndMs;
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
