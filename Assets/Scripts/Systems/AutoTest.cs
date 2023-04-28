using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class AutoTest : MonoBehaviour
{
    private static AutoTest s_this;

    public static int s_Index, s_Ms, s_TargetMs;
    public static NoteHolder s_TargetHolder;
    public static bool s_isTesting = false, s_isPause = false;

    private static List<NoteHolder> s_holders;
    private static int s_SpeedMs, s_SpeedPos, s_EffectMs, s_EffectPos, s_OffsetMs;
    private static int[] s_Offset = new int[2];
    private static float s_posY, s_bpm, s_bpmValue;

    [SerializeField] private InputAction[] inputActions;

    [SerializeField] Transform[] _MovingField;
    private static Transform[] MovingField;

    [SerializeField] Animator[] judgeEffects;
    [SerializeField] AudioSource[] judgeSounds;
    [SerializeField] AudioSource[] judgeLongSounds;
    [SerializeField] Material[] materials;
    [SerializeField] TMP_InputField[] inputFields;

    private const string judgeEffectTrigger = "Play";

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
        if (!s_isPause) { return; }
        s_Ms++;
    }
    private void Update()
    {
        if (!s_isTesting) { return; }
        s_OffsetMs = s_Ms - s_Offset[1]; //$ Judge Offset Ms

        //# Note Field Movement
        s_posY = s_SpeedPos + s_EffectPos
            + (s_OffsetMs - s_SpeedMs - s_EffectMs) * s_bpmValue;

        //# Note Judge
        if (s_TargetHolder.stdMs <= s_OffsetMs)
        {
            JudgeApply(s_TargetHolder);
        }
    }

    public static void StartTest()
    {
        if (NoteField.s_noteHolders.Count == 0) { return; }

        foreach (TMP_InputField inputField in s_this.inputFields) { inputField.interactable = false; }

        InputManager.EnableInput(false);
        s_this.StartCoroutine(MoveField());

        NoteField.SortNoteHolder();
        s_holders = new List<NoteHolder>();
        s_holders = NoteField.s_noteHolders;

        s_TargetHolder = s_holders[0];
        s_Index = 0;
        s_TargetMs = s_TargetHolder.stdMs;

        s_Ms = ValueManager.s_delay;
        s_bpm = System.Convert.ToSingle(ValueManager.s_Bpm);
        s_bpmValue = s_bpm / 150f;

        s_this.StartCoroutine(IStartTest(ValueManager.s_delay));
    }
    public static void EndTest()
    {
        InputManager.EnableInput(true);
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
    }
    private static void JudgeEffect(GameNote.NormalNote note)
    {
        if (note == null) { return; }

        int index;

        if (note.isAir || note.line > 4) { index = note.line - 1 + 4; }
        else { index = note.line - 1; }

        s_this.judgeEffects[index].SetTrigger(judgeEffectTrigger);
    }

    private static IEnumerator IStartTest(int delay)
    {
        s_isTesting = true;
        if (delay < 0) { MusicBox.audioSource.time = -delay / 1000f; }
        else { yield return new WaitForSeconds(delay / 1000f); }
        MusicBox.audioSource.Play();
    }
    private static IEnumerator MoveField()
    {
        while (true)
        {
            MovingField[0].localPosition = new Vector3(0, s_posY, 0);
            MovingField[1].localPosition = new Vector3(0, s_posY * (ValueManager.s_GameSpeed / 100f), 0);
            yield return null;
        }
    }

    public void Btn_StartTest()
    {
        StartTest();
    }
    public void Btn_EndTest()
    {
        EndTest();
    }
}
