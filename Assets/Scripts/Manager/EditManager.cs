using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameNote;

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
    public static int s_page, s_posY, s_line, s_length, s_soundIndex;
    public static bool s_isAirial = false, s_isGuideLeft = true;
    private static bool s_shift = false, s_ctrl = false;
    [SerializeField] InputAction[] actions;
    [SerializeField] GameObject p_DragSelectHelper;
    private static GameObject DragSelectHelper;

    private void Awake()
    {
        s_this = this;
        actions[0].performed += item => { s_shift = true; };
        actions[1].performed += item => { s_shift = false; };
        actions[2].performed += item => { s_ctrl = true; };
        actions[3].performed += item => { s_ctrl = false; };
        InputEnable(true);
        DragSelectHelper = p_DragSelectHelper;
        p_DragSelectHelper = null;
    }

    public static void InputEnable(bool isEnable)
    {
        foreach (InputAction action in s_this.actions)
        {
            if (isEnable) { action.Enable(); }
            else { action.Disable(); }
        }
    }
    public static void Select(NoteData data)
    {
        NoteHolder holder;
        s_LastData = data;
        holder = data.GetNoteHolder();
        data.Selected(true);

        s_posY = holder.stdPos % 1600;
        s_page = Mathf.FloorToInt(holder.stdPos / 1600f);

        s_SelectNoteHolder = holder;

        NormalNote note;
        s_noteType = data.noteType;
        s_line = data.NoteLine;
        s_isAirial = data.noteType == NoteType.Airial ? true : false;
        switch (s_noteType)
        {
            case NoteType.Normal:
                note = holder.normals[s_line - 1];
                s_length = note.length;
                s_soundIndex = 0;
                break;

            case NoteType.Bottom:
                note = holder.bottoms[s_line - 1];
                s_length = note.length;
                s_soundIndex = note.SoundIndex;
                break;

            case NoteType.Airial:
                note = holder.airials[s_line - 1];
                s_length = note.length;
                s_soundIndex = 0;
                break;
                
            case NoteType.Speed:
                
                break;

            case NoteType.Effect:
                
                break;

            case NoteType.None:
            default:
                new System.Exception("");
                break;
        }

        InputManager.Editing(true);
        EditBox.PopUpBox(s_noteType);
        // HelperUpdate(true);

        int pagePos, startPos, endPos;
        startPos = NoteField.s_StartPos;
        endPos = NoteField.s_StartPos + Mathf.FloorToInt(1600 * NoteField.s_Zoom);
        pagePos = s_SelectNoteHolder.stdPos;

        while (pagePos < startPos)
        {
            NoteField.s_Scroll--;
            startPos = NoteField.s_Page * 1600
                + Mathf.RoundToInt(1600f / GuideGenerate.s_guideCount * NoteField.s_Scroll);
        }
        while (pagePos > endPos)
        {
            NoteField.s_Scroll++;
            endPos = NoteField.s_Page * 1600
               + Mathf.RoundToInt(1600f / GuideGenerate.s_guideCount * NoteField.s_Scroll)
               + Mathf.FloorToInt(1600f * (10f / NoteField.s_Zoom));
        }
        NoteField.s_this.UpdateField();
    }
    public static void Escape()
    {
        s_LastData.Selected(false);

        foreach (NoteHolder holder in NoteHolder.s_holders) { holder.EnableCollider(true); }
        InputManager.Editing(false);
        EditBox.Deselect();
        // HelperUpdate(false);
    }
    public static void Delete()
    {
        if (s_SelectNoteHolder == null) { return; }

        if (s_isMultyEditing) { ; return; }

        if (s_noteType == NoteType.Normal)
        {
            NormalNote note;
            note = s_SelectNoteHolder.normals[s_line - 1];
            NoteClass.s_NormalNotes.RemoveAll(item => item == note);
            s_SelectNoteHolder.normals[s_line - 1] = null;
        }
        else if (s_noteType == NoteType.Bottom)
        {
            NormalNote note;
            note = s_SelectNoteHolder.bottoms[s_line - 1];
            NoteClass.s_NormalNotes.RemoveAll(item => item == note);
            s_SelectNoteHolder.bottoms[s_line - 1] = null;
        }
        else if (s_noteType == NoteType.Airial)
        {
            NormalNote note;
            note = s_SelectNoteHolder.airials[s_line - 1];
            NoteClass.s_NormalNotes.RemoveAll(item => item == note);
            s_SelectNoteHolder.airials[s_line - 1] = null;
        }
        else
        {
            if (s_noteType == NoteType.Speed)
            {
                SpeedNote noteData;
                noteData = s_SelectNoteHolder.speedNote;
                NoteClass.s_SpeedNotes.RemoveAll(item => item == noteData);
                s_SelectNoteHolder.speedNote = null;
            }
            else if (s_noteType == NoteType.Effect)
            {
                EffectNote noteData;
                noteData = s_SelectNoteHolder.effectNote;
                NoteClass.s_EffectNotes.RemoveAll(item => item == noteData);
                s_SelectNoteHolder.effectNote = null;
            }
            else { throw new Exception("Note Delete System Error!"); }
        }
        NoteClass.SortAll();
        s_SelectNoteHolder.UpdateNote();
        Escape();
    }
    public static void EditNote(int page = -1, int pos = -1, int line = -1)
    {
        if (s_noteType == NoteType.None) { return; }


        if (page > -1) { s_page = page; }
        if (pos > -1) { s_posY = pos; }
        if (line > -1) { s_line = line; }

        int[] noteData = new int[3] { s_page, s_posY, s_line };

        NoteHolder targetHolder;

        if (page != -1 || pos != -1) //@ (page == -1 && pos == -1) => UnChanged
        {
            int finalValue;
            finalValue = noteData[0] * 1600 + noteData[1];

            targetHolder = NoteHolder.s_holders.Find(item => item.stdPos == finalValue);

            if (targetHolder == null) { targetHolder = NoteGenerate.GenerateNoteManual(finalValue);}
        }
        else { return; }

        NormalNote[] values = new NormalNote[2];
        switch (s_noteType)
        {
            case NoteType.Normal:
                values[0] = s_SelectNoteHolder.normals[s_line - 1]; //@ A
                values[1] = targetHolder.normals[noteData[2] - 1];  //@ B
                s_SelectNoteHolder.normals[s_line - 1] = values[1]; //$ B
                targetHolder.normals[noteData[2] - 1] = values[0];  //$ A
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.getNormal(noteData[2] - 1).GetComponent<NoteData>());
                break;

            case NoteType.Airial:
                values[0] = s_SelectNoteHolder.airials[s_line - 1]; //@ A
                values[1] = targetHolder.airials[noteData[2] - 1];  //@ B
                s_SelectNoteHolder.airials[s_line - 1] = values[1]; //$ B
                targetHolder.airials[noteData[2] - 1] = values[0];  //$ A
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.getAirial(noteData[2] - 1).GetComponent<NoteData>());
                break;

            case NoteType.Bottom:
                values[0] = s_SelectNoteHolder.bottoms[s_line - 1]; //@ A
                values[1] = targetHolder.bottoms[noteData[2] - 1];  //@ B
                s_SelectNoteHolder.bottoms[s_line - 1] = values[1]; //$ B
                targetHolder.bottoms[noteData[2] - 1] = values[0];  //$ A
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.getAirial(noteData[2] - 1).GetComponent<NoteData>());
                break;

            case NoteType.Effect:

                break;

            case NoteType.Speed:

                break;

            default: throw new Exception("Note Type Error");
        }
    }
    public static void LengthNote(int length)
    {
        if (length < 1) { return; }
        else if (length > 256) { return; }
        
        switch (s_noteType)
        {
            case NoteType.Normal:
                s_SelectNoteHolder.normals[s_line - 1].length = length;
                break;
            
            case NoteType.Bottom:
                s_SelectNoteHolder.bottoms[s_line - 1].length = length;
                break;
            
            case NoteType.Airial:
                s_SelectNoteHolder.airials[s_line - 1].length = length;
                break;

            default: return;
        }
        s_SelectNoteHolder.UpdateNote();
    }

    public static void BpmNote(float value)
    {
        if (value < 1) { return; }
        if (s_isMultyEditing) { return; }
        SpeedNote note;
        note = s_SelectNoteHolder.speedNote;
        if ( note == null) { print("returned"); return; }
        note.bpm = value;
        NoteClass.InitSpeedMs();
        NoteField.InitAllHolder();
        SpectrumManager.UpdateSpectrumPos();
        s_SelectNoteHolder.UpdateTextInfo();
    }
    public static void MultiplyNote(float value)
    {
        if (value < 0) { return; }
        if (s_isMultyEditing) { return; }
        SpeedNote note;
        note = s_SelectNoteHolder.speedNote;
        if ( note == null) { return; }
        note.multiple = value;
        s_SelectNoteHolder.UpdateTextInfo();
    }

    public static void MoveNoteInput(bool isUp)
    {
        if (s_shift && s_ctrl) { return; }

        //$ Page Movement
        if (s_ctrl)
        {
            EditNote(page: isUp ? s_page + 1 : s_page - 1);
        }
        //$ Legnth Change
        else if (s_shift)
        {
            LengthNote(length: isUp ? s_length + 1 : s_length - 1);
        }
        //$ Pos Movement
        else
        {
            int indexer;
            int[] copyArr;
            copyArr = GuideGenerate.s_guidePos;

            if (copyArr.Contains(s_posY % 1600))
            {
                indexer = Array.IndexOf(copyArr, s_posY % 1600);
                if (isUp)
                {
                    s_posY = s_posY - copyArr[indexer] + copyArr[indexer + 1];
                    if (s_posY == 1600)
                    {
                        s_page++;
                        s_posY = 0;
                    }
                }
                else
                {
                    if (indexer == 0)
                    {
                        if (s_page == 0) { s_posY = 0; }
                        else
                        {
                            s_page--;
                            s_posY = copyArr[copyArr.Length - 2];
                        }
                    }
                    else { s_posY = s_posY - copyArr[indexer] + copyArr[indexer - 1]; }
                }
            }
            else
            {
                if (isUp)
                {
                    for (int i = 0; i < copyArr.Length; i++)
                    {
                        if (copyArr[i] > s_posY) { s_posY = copyArr[i]; break; }
                    }
                    if (s_posY == 1600)
                    {
                        s_page++;
                        s_posY = 0;
                    }
                }
                else
                {
                    for (int i = 1; i < copyArr.Length; i++)
                    {
                        if (copyArr[i] > s_posY) { s_posY = copyArr[i - 1]; break; }
                    }
                }
            }
            EditNote(pos:s_posY);
        }
        InfoField.UpdateInfoField();
    }
    public static void LineNoteInput(bool isLeft)
    {
        if (s_SelectNoteHolder == null) { return; }

        int editLine;
        editLine = s_line;

        if (isLeft) { editLine--; }
        else { editLine++; }

        EditNote(line: editLine);
        InfoField.UpdateInfoField();
    }

    /*
    private static void HelperUpdate(bool Enable)
    {
        if (Enable)
        {
            p_tag = s_SelectedObject.transform.parent.tag;

            DragSelectHelper.SetActive(true);
            DragSelectHelper.transform.position = s_SelectedObject.transform.position;

            if (p_tag == noteTag[0])
            {
                DragSelectHelper.GetComponent<DragSelectHelper>().Resize(190, 55);
            }
            else if (p_tag == noteTag[1])
            {
                DragSelectHelper.GetComponent<DragSelectHelper>().Resize(480, 55);
            }
            else if (p_tag == noteTag[2])
            {
                DragSelectHelper.GetComponent<DragSelectHelper>().Resize(240, 55);
            }
            else
            {
                DragSelectHelper.transform.position = new Vector3
                    (-746, s_SelectedObject.transform.position.y, 0);
                DragSelectHelper.GetComponent<DragSelectHelper>().Resize(100, 130);
            }
        }
        else
        {
            DragSelectHelper.SetActive(false);
        }
    }
    */
}