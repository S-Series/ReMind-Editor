using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class EditManager : MonoBehaviour
{
    public static NoteHolder s_SelectNoteHolder;
    public static GameObject s_SelectedObject;

    public static void Select(GameObject obj)
    {
        if (s_SelectedObject != null)
        {
            s_SelectedObject.GetComponent<BoxCollider2D>().enabled = true;
            s_SelectedObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }

        s_SelectNoteHolder = obj.GetComponentInParent<NoteHolder>();
        s_SelectedObject = obj;

        obj.GetComponent<BoxCollider2D>().enabled = false;

        if (obj.transform.parent.CompareTag("Normal"))
        { obj.GetComponent<SpriteRenderer>().color = new Color32(100, 255, 100, 255); }
        else if (obj.transform.parent.CompareTag("Bottom"))
        { obj.GetComponent<SpriteRenderer>().color = new Color32(255, 000, 000, 255); }
        else if (obj.transform.parent.CompareTag("Airial"))
        { obj.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 000, 255); }
        else
        { obj.GetComponent<SpriteRenderer>().color = new Color32(100, 255, 100, 255); }

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
        editPos = s_SelectNoteHolder.stdPos - s_SelectNoteHolder.stdPos % 1600 + editPos;
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
}