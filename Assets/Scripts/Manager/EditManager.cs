using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameNote;
using UnityEditor;

public class EditManager : MonoBehaviour
{
    private static EditManager s_this;


    #region //% MultyEditing
    struct EditData
    {
        public int[] datas { get; }
        public NoteType noteType { get; }
        public NoteHolder noteHolder { get; }
        public EditData (int[] x, NoteType y, NoteHolder z)
        {
            datas = x;
            noteType = y;
            noteHolder = z;
        }
    }
    public static bool s_isMultyEditing = false;
    private static List<NoteData> s_MultyDatas;

    #endregion //% End MultyEditing

    public static NoteData s_LastData;
    public static NoteType s_noteType = NoteType.None;
    public static NoteHolder s_SelectNoteHolder;
    public static int s_page, s_posY, s_line, s_length;
    public static double s_bpm, s_multiply;
    public static bool s_isAirial = false;
    public static string s_effectTitle;
    /*
    [SerializeField] GameObject p_DragSelectHelper;
    private static GameObject DragSelectHelper;
    */

    private void Awake()
    {
        s_this = this;
        /*DragSelectHelper = p_DragSelectHelper;
        p_DragSelectHelper = null;*/
    }

    #region SelectNote(NoteType)
    public static void SelectNote(NormalNote note)
    {
        ResetSelectData();
        s_noteType = NoteType.Normal;
        EditBox.PopUpBox(note);

        s_posY = note.posY % 1600;
        s_page = Mathf.FloorToInt(note.posY / 1600f);
        s_line = note.line;
        s_length = note.length;
        s_isAirial = note.isAirial;
    }
    public static void SelectNote(FloorNote note)
    {
        ResetSelectData();
        s_noteType = NoteType.Floor;
        EditBox.PopUpBox(note);

        s_posY = note.posY % 1600;
        s_page = Mathf.FloorToInt(note.posY / 1600f);
        s_line = note.line;
        s_length = note.length;
        s_isAirial = false;
    }
    public static void SelectNote(SpeedNote note)
    {
        ResetSelectData();
        s_noteType = NoteType.Speed;
        EditBox.PopUpBox(note);

        s_posY = note.posY % 1600;
        s_page = Mathf.FloorToInt(note.posY / 1600f);
        s_bpm = note.bpm;
        s_multiply = note.multiple;
        s_isAirial = false;
        s_effectTitle = String.Empty;
    }
    public static void SelectNote(EffectNote note)
    {
        ResetSelectData();
        s_noteType = NoteType.Effect;
        EditBox.PopUpBox(note);

        s_posY = note.posY % 1600;
        s_page = Mathf.FloorToInt(note.posY / 1600f);
        s_isAirial = false;
        s_effectTitle = note.effectName;
    }
    public static void SelectHolder(NoteHolder holder) { s_SelectNoteHolder = holder; }
    #endregion
    private static void ResetSelectData()
    {
        s_SelectNoteHolder = null;
        s_noteType = NoteType.None;

        s_posY = -1;
        s_page = -1;
        s_line = -1;
        s_length = -1;

        s_bpm = -1d;
        s_multiply = -1d;

        s_isAirial = false;

        s_effectTitle = String.Empty;
    }
    public static void Deselect()
    {
        EditBox.Deselect();
        ResetSelectData();
    }
    public static void DeleteNote()
    {

    }
    public static void MoveNote()
    {

    }
    public static void InputRow(bool isLeft)
    {

    }
    public static void InputCol(bool isUp)
    {

    }

    public static void NoteEdit(int pos = -1, int page = -1, int line = -1, int length = -1)
    {

    }
    public static void BpmNoteEdit(float bpm = -1, float multiply = -1)
    {

    }
}