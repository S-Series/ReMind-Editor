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
    private static int s_page, s_posY, s_legnth;
    private static bool s_shift = false, s_ctrl = false; 

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
        if (s_SelectedObject != null)
        {
            s_SelectedObject.GetComponent<BoxCollider2D>().enabled = true;
            s_SelectedObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }

        s_SelectNoteHolder = obj.GetComponentInParent<NoteHolder>();
        s_SelectedObject = obj;

        s_posY = s_SelectNoteHolder.stdPos % 1600;
        s_page = Mathf.FloorToInt(s_SelectNoteHolder.stdPos / 1600f);

        obj.GetComponent<BoxCollider2D>().enabled = false;

        if (obj.transform.parent.CompareTag("Normal"))
        {
            s_legnth = s_SelectNoteHolder.normals[Convert.ToInt32(obj.tag) - 1].legnth;
            obj.GetComponent<SpriteRenderer>().color = new Color32(100, 255, 100, 255);
        }
        else if (obj.transform.parent.CompareTag("Bottom"))
        {
            s_legnth = s_SelectNoteHolder.bottoms[Convert.ToInt32(obj.tag) - 1].legnth;
            obj.GetComponent<SpriteRenderer>().color = new Color32(255, 000, 000, 255);
        }
        else if (obj.transform.parent.CompareTag("Airial"))
        {
            s_legnth = 0;
            obj.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 000, 255);
        }
        else
        {
            s_legnth = 0;
            obj.GetComponent<SpriteRenderer>().color = new Color32(100, 255, 100, 255);
        }

        InputManager.Editing(true);
        EditBox.PopUpBox(obj);
    }

    public static void Escape()
    {
        if (s_SelectedObject != null)
        {
            s_SelectedObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }

        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EditMode(true); }
        InputManager.Editing(false);
        EditBox.Deselect();
    }

    public static void PosNote(int editPos)
    {
        editPos = 1600 * s_page + s_posY;
        print(editPos);

        NoteHolder targetHolder;
        GameObject targetObject;
        targetHolder = NoteField.s_noteHolders.Find(item => item.stdPos == editPos);

        if (targetHolder == null) { targetHolder = NoteGenerate.GenerateNoteManual(editPos); }

        if (s_SelectedObject.transform.parent.CompareTag("Normal"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.normals[Convert.ToInt32(s_SelectedObject.tag) - 1];

            targetHolder.normals[note.line - 1] = note;
            s_SelectNoteHolder.normals[note.line - 1] = null;
            note.pos = editPos;

            targetObject = targetHolder.Normal(note.line - 1);
        }
        else if (s_SelectedObject.transform.parent.CompareTag("Airials"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.airials[Convert.ToInt32(s_SelectedObject.tag) - 1];

            targetHolder.airials[note.line - 1] = note;
            s_SelectNoteHolder.airials[note.line - 1] = null;
            note.pos = editPos;
        
            targetObject = targetHolder.Airial(note.line - 1);
        }
        else if (s_SelectedObject.transform.parent.CompareTag("Bottoms"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

            targetHolder.bottoms[note.line - 5] = note;
            s_SelectNoteHolder.bottoms[note.line - 5] = null;
            note.pos = editPos;
        
            targetObject = targetHolder.Bottom(note.line - 1);
        }
        else if (s_SelectedObject.CompareTag("01"))
        {
            SpeedNote note;
            note = s_SelectNoteHolder.speedNote;

            targetHolder.speedNote = note;
            s_SelectNoteHolder.speedNote = null;
        
            targetObject = targetHolder.Speed();
        }
        else
        {
            EffectNote note;
            note = s_SelectNoteHolder.effectNote;

            targetHolder.effectNote = note;
            s_SelectNoteHolder.effectNote = null;

            targetObject = targetHolder.Effect();
        }

        targetHolder.UpdateNote();
        s_SelectNoteHolder.UpdateNote();
        s_SelectNoteHolder.CheckDestroy();

        Select(targetObject);
    }
    public static void PageNote(int editPage)
    {
        int editPos;
        editPos = editPage * 1600 + s_SelectNoteHolder.stdPos % 1600;

        NoteHolder targetHolder;
        targetHolder = NoteField.s_noteHolders.Find(item => item.stdPos == editPos);
        if (targetHolder == null) { targetHolder = NoteGenerate.GenerateNoteManual(editPos); }

        if (s_SelectedObject.transform.parent.CompareTag("Normal"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.normals[Convert.ToInt32(s_SelectedObject.tag) - 1];

            targetHolder.normals[note.line - 1] = note;
            s_SelectNoteHolder.normals[note.line - 1] = null;
            note.pos = editPos;
        }
        else if (s_SelectedObject.transform.parent.CompareTag("Airials"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.airials[Convert.ToInt32(s_SelectedObject.tag) - 1];

            targetHolder.airials[note.line - 1] = note;
            s_SelectNoteHolder.airials[note.line - 1] = null;
            note.pos = editPos;
        }
        else if (s_SelectedObject.transform.parent.CompareTag("Bottoms"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

            targetHolder.bottoms[note.line - 5] = note;
            s_SelectNoteHolder.bottoms[note.line - 5] = null;
            note.pos = editPos;
        }
        else if (s_SelectedObject.CompareTag("01"))
        {
            SpeedNote note;
            note = s_SelectNoteHolder.speedNote;

            targetHolder.speedNote = note;
            s_SelectNoteHolder.speedNote = null;
        }
        else
        {
            EffectNote note;
            note = s_SelectNoteHolder.effectNote;

            targetHolder.effectNote = note;
            s_SelectNoteHolder.effectNote = null;
        }

        targetHolder.UpdateNote();
        s_SelectNoteHolder.UpdateNote();
        s_SelectNoteHolder.CheckDestroy();

        targetHolder.UpdateNote();
        s_SelectNoteHolder.UpdateNote();
        s_SelectNoteHolder.CheckDestroy();
    }
    public static void LineNote(int editLine)
    {
        if (editLine < 1) { editLine = 1; }
        else if (editLine > 4) { editLine = 4; }

        if (s_SelectedObject.transform.parent.CompareTag("Normal"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.normals[Convert.ToInt32(s_SelectedObject.tag) - 1];

            if (note.line == editLine) { return; }
            if (s_SelectNoteHolder.normals[editLine - 1] != null) { return; }

            s_SelectNoteHolder.normals[editLine - 1] = note;
            s_SelectNoteHolder.normals[note.line - 1] = null;
            note.line = editLine;
        }
        else if (s_SelectedObject.transform.parent.CompareTag("Airials"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.airials[Convert.ToInt32(s_SelectedObject.tag) - 1];

            if (note.line == editLine) { return; }
            if (s_SelectNoteHolder.airials[editLine - 1] != null) { return; }

            s_SelectNoteHolder.airials[editLine - 1] = note;
            s_SelectNoteHolder.airials[note.line - 1] = null;
            note.line = editLine;
        }
        else
        {
            if (editLine > 2) { editLine = 2; }

            NormalNote note;
            note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

            if (note.line == editLine) { return; }
            if (s_SelectNoteHolder.bottoms[editLine - 1] != null) { return; }

            s_SelectNoteHolder.bottoms[editLine - 1] = note;
            s_SelectNoteHolder.bottoms[note.line - 1] = null;
            note.line = editLine;
        }
        s_SelectNoteHolder.UpdateNote();
        s_SelectNoteHolder.CheckDestroy();
    }
    public static void LegnthNote(int editLegnth)
    {
        if (editLegnth < 1) { editLegnth = 1; }

        if (s_SelectedObject.transform.parent.CompareTag("Normal"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.normals[Convert.ToInt32(s_SelectedObject.tag) - 1];

            note.legnth = editLegnth;
            s_SelectNoteHolder.UpdateNote();
        }
        else if (s_SelectNoteHolder.transform.parent.CompareTag("Bottom"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

            note.legnth = editLegnth;
            s_SelectNoteHolder.UpdateNote();
        }
    }

    public static void MoveNoteInput(bool isUp)
    {
        if (s_shift && s_ctrl) { return; }

        //$ Page Movement
        if (s_shift)
        {
            s_page = isUp ? s_page + 1 : s_page - 1;
            
            if (s_page < 0) { s_page = 0; }
            if (s_page > 998) { s_page = 998; }

            PageNote(s_page);
        }
        //& Legnth Change
        else if (s_ctrl)
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
    public static void LineNOteInput(bool isLeft)
    {

    }
}