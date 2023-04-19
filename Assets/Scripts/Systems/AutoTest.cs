using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTest : MonoBehaviour
{
    private static AutoTest s_this;

    public static int s_Index, s_Ms, s_TargetMs;
    public static NoteHolder s_TargetHolder;
    public static bool s_isTesting = false;

    private static List<NoteHolder> s_holders;
    private static int s_SpeedMs, s_SpeedPos, s_EffectMs, s_EffectPos, s_OffsetMs;
    private static int[] s_Offset = new int[2];
    private static float s_posY, s_bpm, s_bpmValue;

    [SerializeField] Transform[] MovingField;
    [SerializeField] Animator[] judgeEffects;
    [SerializeField] AudioSource[] judgeSounds;
    [SerializeField] AudioSource[] judgeLongSounds;

    private const string judgeEffectTrigger = "Play";

    private void FixedUpdate()
    {
        if (!s_isTesting) { return; }
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

        InputManager.EnableInput(false);
        s_this.StartCoroutine(s_this.MoveField());

        NoteField.SortNoteHolder();
        s_holders = new List<NoteHolder>();
        s_holders = NoteField.s_noteHolders;

        s_TargetHolder = s_holders[0];
        s_Index = 0;
        s_TargetMs = s_TargetHolder.stdMs;

        s_Ms = ValueManager.s_delay;
        s_bpm = System.Convert.ToSingle(ValueManager.s_Bpm);
        s_bpmValue = s_bpm / 150f;
        MusicBox.audioSource.Play();

        s_isTesting = true;
    }
    public static void EndTest()
    {
        InputManager.EnableInput(true);
        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EnableNote(true); }
        s_this.StopAllCoroutines();
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

        holder.EnableNote(false);
    }
    private static void JudgeEffect(GameNote.NormalNote note)
    {
        if (note == null) { return; }

        int index;

        if (note.isAir || note.line > 4) { index = note.line - 1 + 4; }
        else { index = note.line - 1; }

        s_this.judgeEffects[index].SetTrigger(judgeEffectTrigger);
    }

    private IEnumerator MoveField()
    {
        while (true)
        {
            MovingField[0].localPosition = new Vector3(0, s_posY, 0);
            MovingField[1].localPosition = new Vector3(0, s_posY * (ValueManager.s_GameSpeed / 100f), 0);
            MovingField[2].localPosition = new Vector3(0, s_Offset[0] * s_bpmValue, 0);
            yield return null;
        }
    }
}
