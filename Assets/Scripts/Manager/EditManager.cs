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

    public static NoteHolder s_SelectNoteHolder;
    public static GameObject s_SelectedObject;
    public static int s_page, s_posY, s_line, s_legnth, s_soundIndex;
    public static bool s_isAirial = false;
    private static bool s_shift = false, s_ctrl = false;

    private static readonly string[] noteTag = { "Normal", "Bottom", "Airial" };

    [SerializeField] InputAction[] actions;

    private void Awake()
    {
        s_this = this;
        actions[0].performed += item => { s_shift = true; };
        actions[1].performed += item => { s_shift = false; };
        actions[2].performed += item => { s_ctrl = true; };
        actions[3].performed += item => { s_ctrl = false; };
        InputEnable(true);
    }

    public static void InputEnable(bool isEnable)
    {
        foreach (InputAction action in s_this.actions)
        {
            if (isEnable) { action.Enable(); }
            else { action.Disable(); }
        }
    }

    public static void Select(GameObject obj)
    {
        int _count;
        if (s_SelectedObject != null)
        {
            _count = s_SelectedObject.transform.childCount;
            if (_count == 0)
            {
                s_SelectedObject.transform.GetComponent<BoxCollider2D>().enabled = true;
                s_SelectedObject.transform.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                for (int i = 0; i < _count; i++)
                {
                    s_SelectedObject.transform.GetChild(i)
                        .TryGetComponent<BoxCollider2D>(out var collider2D);
                    s_SelectedObject.transform.GetChild(i)
                        .GetComponent<SpriteRenderer>().color = Color.white;
                    if (collider2D != null) { collider2D.enabled = true; }
                }
            }
        }

        _count = obj.transform.childCount;
        if (_count == 0)
        {
            obj.transform.GetComponent<BoxCollider2D>().enabled = false;
            obj.transform.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            for (int i = 0; i < _count; i++)
            {
                obj.transform.GetChild(i).TryGetComponent<BoxCollider2D>(out var collider2D);
                obj.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.yellow;
                if (collider2D != null) { collider2D.enabled = false; }
            }
        }

        s_SelectNoteHolder = obj.GetComponentInParent<NoteHolder>();
        s_SelectedObject = obj;

        s_posY = s_SelectNoteHolder.stdPos % 1600;
        s_page = Mathf.FloorToInt(s_SelectNoteHolder.stdPos / 1600f);

        if (obj.transform.parent.CompareTag(noteTag[0]))
        {
            s_isAirial = false;
            s_line = Convert.ToInt32(obj.tag);
            s_legnth = s_SelectNoteHolder.normals[s_line - 1].length;
            s_soundIndex = s_SelectNoteHolder.normals[s_line - 1].SoundIndex;
        }
        else if (obj.transform.parent.CompareTag(noteTag[1]))
        {
            s_isAirial = false;
            s_line = Convert.ToInt32(obj.tag);
            s_legnth = s_SelectNoteHolder.bottoms[s_line - 1].length;
            s_soundIndex = s_SelectNoteHolder.bottoms[s_line - 1].SoundIndex;
        }
        else if (obj.transform.parent.CompareTag(noteTag[2]))
        {
            s_isAirial = true;
            s_line = Convert.ToInt32(obj.tag);
            s_legnth = 0;
            s_soundIndex = s_SelectNoteHolder.airials[s_line - 1].SoundIndex;
            // obj.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 000, 255);
        }
        else
        {
            s_line = 0;
            s_legnth = 0;
            // obj.GetComponent<SpriteRenderer>().color = new Color32(100, 255, 100, 255);
        }

        InputManager.Editing(true);
        EditBox.PopUpBox(obj);

        int pagePos, startPos, endPos;
        startPos = NoteField.s_StartPos;
        endPos = NoteField.s_StartPos + Mathf.FloorToInt(1600 * (NoteField.s_Zoom / 10f));
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
            print(NoteField.s_Zoom);
        }
        NoteField.s_this.UpdateField(); 
    }

    public static void Escape()
    {
        if (s_SelectedObject != null)
        {
            int _count;
            _count = s_SelectedObject.transform.childCount;
            if (_count == 0)
            {
                s_SelectedObject.transform.GetComponent<BoxCollider2D>().enabled = true;
                s_SelectedObject.transform.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                for (int i = 0; i < _count; i++)
                {
                    s_SelectedObject.transform.GetChild(i)
                        .TryGetComponent<BoxCollider2D>(out var collider2D);
                    s_SelectedObject.transform.GetChild(i)
                        .GetComponent<SpriteRenderer>().color = Color.white;
                    if (collider2D != null) { collider2D.enabled = true; }
                }
            }
        }

        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EditMode(true); }
        InputManager.Editing(false);
        EditBox.Deselect();
    }

    public static void PosNote(int editPos)
    {
        editPos = 1600 * s_page + editPos;

        NoteHolder targetHolder;
        GameObject targetObject;
        targetHolder = NoteField.s_noteHolders.Find(item => item.stdPos == editPos);

        if (targetHolder == null) { targetHolder = NoteGenerate.GenerateNoteManual(editPos); }

        if (s_SelectedObject.transform.parent.CompareTag(noteTag[0]))
        {
            if (targetHolder.normals[s_line - 1] != null)
                { Select(targetHolder.Normal(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.normals[s_line - 1];

                targetHolder.normals[note.line - 1] = note;
                s_SelectNoteHolder.normals[note.line - 1] = null;
                note.pos = editPos;

                targetObject = targetHolder.Normal(note.line - 1);
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[1]))
        {
            if (targetHolder.airials[s_line - 1] != null)
            { Select(targetHolder.Airial(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.airials[Convert.ToInt32(s_SelectedObject.tag) - 1];

                targetHolder.airials[note.line - 1] = note;
                s_SelectNoteHolder.airials[note.line - 1] = null;
                note.pos = editPos;

                targetObject = targetHolder.Airial(note.line - 1);
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[2]))
        {
            if (targetHolder.bottoms[s_line - 1] != null)
            { Select(targetHolder.Bottom(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

                targetHolder.bottoms[note.line - 5] = note;
                s_SelectNoteHolder.bottoms[note.line - 5] = null;
                note.pos = editPos;

                targetObject = targetHolder.Bottom(note.line - 1);
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }
        else if (s_SelectedObject.CompareTag("01"))
        {
            if (targetHolder.speedNote != null)
            { Select(targetHolder.Speed()); }
            else
            {
                SpeedNote note;
                note = s_SelectNoteHolder.speedNote;

                targetHolder.speedNote = note;
                s_SelectNoteHolder.speedNote = null;

                targetObject = targetHolder.Speed();
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }
        else
        {
            if (targetHolder.effectNote != null)
            { Select(targetHolder.Effect()); }
            else
            {
                EffectNote note;
                note = s_SelectNoteHolder.effectNote;

                targetHolder.effectNote = note;
                s_SelectNoteHolder.effectNote = null;

                targetObject = targetHolder.Effect();
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }

        targetHolder.UpdateNote();
        s_SelectNoteHolder.UpdateNote();
        s_SelectNoteHolder.CheckDestroy();
    }
    public static void PageNote(int editPage)
    {
        int editPos;
        editPos = editPage * 1600 + s_SelectNoteHolder.stdPos % 1600;

        NoteHolder targetHolder;
        targetHolder = NoteField.s_noteHolders.Find(item => item.stdPos == editPos);
        if (targetHolder == null) { targetHolder = NoteGenerate.GenerateNoteManual(editPos); }

        if (s_SelectedObject.transform.parent.CompareTag(noteTag[0]))
        {
            if (targetHolder.normals[s_line - 1] != null)
                { Select(targetHolder.Normal(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.normals[s_line - 1];

                targetHolder.normals[note.line - 1] = note;
                s_SelectNoteHolder.normals[note.line - 1] = null;
                note.pos = editPos;

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.Normal(note.line - 1));
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[1]))
        {
            if (targetHolder.airials[s_line - 1] != null)
            { Select(targetHolder.Airial(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.airials[Convert.ToInt32(s_SelectedObject.tag) - 1];

                targetHolder.airials[note.line - 1] = note;
                s_SelectNoteHolder.airials[note.line - 1] = null;
                note.pos = editPos;

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.Airial(note.line - 1));
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[2]))
        {
            if (targetHolder.bottoms[s_line - 1] != null)
            { Select(targetHolder.Bottom(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

                targetHolder.bottoms[note.line - 5] = note;
                s_SelectNoteHolder.bottoms[note.line - 5] = null;
                note.pos = editPos;

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.Bottom(note.line - 4 - 1));
            }
        }
        else if (s_SelectedObject.CompareTag("01"))
        {
            if (targetHolder.speedNote != null)
            { Select(targetHolder.Speed()); }
            else
            {
                SpeedNote note;
                note = s_SelectNoteHolder.speedNote;

                targetHolder.speedNote = note;
                s_SelectNoteHolder.speedNote = null;
                Select(targetHolder.Speed());

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.Speed());
            }
        }
        else
        {
            if (targetHolder.effectNote != null)
            { Select(targetHolder.Effect()); }
            else
            {
                EffectNote note;
                note = s_SelectNoteHolder.effectNote;

                targetHolder.effectNote = note;
                s_SelectNoteHolder.effectNote = null;
                Select(targetHolder.Effect());

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.Effect());
            }
        }
    }
    public static void LineNote(int editLine)
    {
        NormalNote editNormal;

        if (editLine < 1 || editLine > 4) { return; }

        if (s_SelectedObject.transform.parent.CompareTag(noteTag[0]))
        {
            if (s_SelectNoteHolder.normals[editLine - 1] != null)
            { Select(s_SelectNoteHolder.Normal(editLine - 1)); }
            else
            {
                editNormal = s_SelectNoteHolder.normals[s_line - 1];
                editNormal.line = editLine;

                s_SelectNoteHolder.normals[s_line - 1] = null;
                s_SelectNoteHolder.normals[editLine - 1] = editNormal;

                s_SelectNoteHolder.UpdateNote();
                Select(s_SelectNoteHolder.Normal(editLine - 1));
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[1]))
        {
            if (s_SelectNoteHolder.airials[editLine - 1] != null)
            { Select(s_SelectNoteHolder.Airial(editLine - 1)); }
            else
            {
                editNormal = s_SelectNoteHolder.airials[s_line - 1];
                editNormal.line = editLine;

                s_SelectNoteHolder.airials[s_line - 1] = null;
                s_SelectNoteHolder.airials[editLine - 1] = editNormal;

                s_SelectNoteHolder.UpdateNote();
                Select(s_SelectNoteHolder.Airial(editLine - 1));
            }
        }
        else
        {
            if (editLine > 2) { return; }

            if (s_SelectNoteHolder.bottoms[editLine - 1] != null)
            { Select(s_SelectNoteHolder.Bottom(editLine - 1)); }
            else
            {
                editNormal = s_SelectNoteHolder.bottoms[s_line - 4 - 1];
                editNormal.line = editLine + 4;

                s_SelectNoteHolder.bottoms[s_line - 4 - 1] = null;
                s_SelectNoteHolder.bottoms[editLine - 1] = editNormal;

                s_SelectNoteHolder.UpdateNote();
                Select(s_SelectNoteHolder.Bottom(editLine - 1));
            }
        }
    }
    public static void LegnthNote(int editLegnth)
    {
        if (editLegnth < 1) { editLegnth = 1; }


        if (s_SelectedObject.transform.parent.CompareTag("Normal"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.normals[Convert.ToInt32(s_SelectedObject.tag) - 1];

            note.length = editLegnth;
            s_SelectNoteHolder.UpdateNote();
        }
        else if (s_SelectNoteHolder.transform.parent.CompareTag("Bottom"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

            note.length = editLegnth;
            s_SelectNoteHolder.UpdateNote();
        }
    }

    public static void MoveNoteInput(bool isUp)
    {
        if (s_shift && s_ctrl) { return; }

        //$ Page Movement
        if (s_ctrl)
        {
            s_page = isUp ? s_page + 1 : s_page - 1;

            if (s_page < 0) { s_page = 0; }
            if (s_page > 998) { s_page = 998; }

            PageNote(s_page);
        }
        //$ Legnth Change
        else if (s_shift)
        {
            if (s_legnth == 0) { return; }

            s_legnth = isUp ? s_legnth + 1 : s_legnth - 1;

            if (s_legnth < 01) { s_legnth = 01; }
            if (s_legnth > 99) { s_legnth = 99; }

            LegnthNote(s_legnth);
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
            PosNote(s_posY);
        }
    }
    public static void LineNoteInput(bool isLeft)
    {
        if (s_line == 0) { return; }
        if (s_SelectNoteHolder == null || s_SelectedObject == null) { return; }

        int editLine;
        editLine = s_line;

        if (isLeft) { editLine--; }
        else { editLine++; }

        LineNote(editLine);
    }
}