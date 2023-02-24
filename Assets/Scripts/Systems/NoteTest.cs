using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class NoteTest : MonoBehaviour
{
    private static NoteTest s_this;
    private static List<MultyNoteHolder> multyHolders = new List<MultyNoteHolder>();

    public static bool isTest = false;
    public static bool isReady = false;

    [SerializeField] Transform GenerateField;

    private static class DeleteNotes
    {
        public static List<NormalNote> deleteNormals = new List<NormalNote>();
        public static List<SpeedNote> deleteSpeeds = new List<SpeedNote>();
        public static List<EffectNote> deleteEffects = new List<EffectNote>();

        public static void ResetAll()
        {
            deleteNormals = new List<NormalNote>();
            deleteSpeeds = new List<SpeedNote>();
            deleteEffects = new List<EffectNote>();
        }

        public static void DeleteAll()
        {
            foreach (NormalNote note in deleteNormals)
            {
                ToolManager.EraseManager.DeleteObject(NoteHolder.holders
                    .Find(item => item.noteClass == note).gameObject, ToolManager.NoteType.Normal);
            }
            foreach (SpeedNote note in deleteSpeeds)
            {
                ToolManager.EraseManager.DeleteObject(SpeedHolder.holders
                    .Find(item => item.noteClass == note).gameObject, ToolManager.NoteType.Speed);
            }
            foreach (EffectNote note in deleteEffects)
            {
                ToolManager.EraseManager.DeleteObject(EffectHolder.holders
                    .Find(item => item.noteClass == note).gameObject, ToolManager.NoteType.Effect);
            }

            ResetAll();
        }
    }

    private void Awake()
    {
        s_this = this;
    }

    public static void InitTest(bool isAuto, bool isPlayScene)
    {
        multyHolders = new List<MultyNoteHolder>();
        NormalNote targetNormal;
        SpeedNote targetSpeed;
        EffectNote targetEffect;
        MultyNoteHolder targetHolder;

        for (int i = 0; i < s_this.GenerateField.childCount; i++)
        {
            Destroy(s_this.GenerateField.GetChild(i).gameObject);
        }

        NoteClass.SortAll();
        NoteClass.InitAll();

        DeleteNotes.ResetAll();

        for (int i = 0; i < NoteClass.s_SpeedNotes.Count; i++)
        {
            targetSpeed = NoteClass.s_SpeedNotes[i];

            targetHolder = multyHolders.Find(item => item.stdPos == targetSpeed.pos);

            if (targetHolder != null) { DeleteNotes.deleteSpeeds.Add(targetSpeed); }
            else
            {
                targetHolder = new MultyNoteHolder();
                targetHolder.InitBySpeed(NoteClass.s_SpeedNotes[i]);
                multyHolders.Add(targetHolder);
            }
        }
        
        targetHolder = null;

        for (int i = 0; i < NoteClass.s_NormalNotes.Count; i++)
        {
            targetNormal = NoteClass.s_NormalNotes[i];

            targetHolder = multyHolders.Find(item => item.stdPos == targetNormal.pos);
            if (targetHolder == null)
            {
                targetHolder = new MultyNoteHolder();
                targetHolder.stdMs = NoteClass.CalMs(targetNormal.pos);
                targetHolder.stdPos = targetNormal.pos;
            }

            if (targetNormal.isAir)
            {
                if (targetHolder.airials[targetNormal.line - 1] != null)
                { DeleteNotes.deleteNormals.Add(targetNormal); }
                else { targetHolder.airials[targetNormal.line - 1] = targetNormal; }
            }
            else if (targetNormal.line > 4)
            {
                if (targetHolder.bottoms[targetNormal.line - 4 - 1] != null)
                { DeleteNotes.deleteNormals.Add(targetNormal); }
                else { targetHolder.bottoms[targetNormal.line - 4 - 1] = targetNormal; }
            }
            else
            {
                if (targetHolder.normals[targetNormal.line - 1] != null)
                { DeleteNotes.deleteNormals.Add(targetNormal); }
                else { targetHolder.normals[targetNormal.line - 1] = targetNormal; }
            }
        }

        targetHolder = null;

        for (int i = 0; i < NoteClass.s_EffectNotes.Count; i++)
        {
            targetEffect = NoteClass.s_EffectNotes[i];

            targetHolder = multyHolders.Find(item => item.stdPos == targetEffect.pos);
            if (targetHolder == null)
            {
                targetHolder = new MultyNoteHolder();
                targetHolder.stdMs = NoteClass.CalMs(targetEffect.pos);
                targetHolder.stdPos = targetEffect.pos;
            }

            if (targetHolder.effect != null) { DeleteNotes.deleteEffects.Add(targetEffect); }
            else { targetHolder.effect = targetEffect; }
        }

        DeleteNotes.DeleteAll();

        multyHolders.OrderBy(item => item.stdPos);

        if (isPlayScene) { ; }

        if (isAuto)
        {

        }
        else
        {

        }
    }
}
