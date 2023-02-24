using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class EditManager : MonoBehaviour
{
    public static EditManager s_this;

    public static bool s_isEditing = false;
    public static EditType s_editType = EditType.None;
    public enum EditType { None, Normal, Speed, Effect };

    public static MultyNoteHolder noteHolder;
    private static int[] holderIndex = new int[2] { 0, 0 };

    [SerializeField] GameObject[] EditWindowObjects;

    private void Awake()
    {
        s_this = this;
    }

    public static void SelectNote(MultyNoteHolder holder, EditType type, NormalNote note = null)
    {
        noteHolder = holder;
        s_editType = type;

        if (type == EditType.Normal)
        {
            holderIndex = new int[2] { note.line, Convert.ToInt32(note.isAir) };
            s_this.EditWindowObjects[0].SetActive(true);
            s_this.EditWindowObjects[1].SetActive(false);
            s_this.EditWindowObjects[2].SetActive(false);
        }
        else if (type == EditType.Speed)
        {
            holderIndex = new int[2] { 0, 0 };
            s_this.EditWindowObjects[0].SetActive(false);
            s_this.EditWindowObjects[1].SetActive(true);
            s_this.EditWindowObjects[2].SetActive(false);
        }
        else if (type == EditType.Effect)
        {
            holderIndex = new int[2] { 0, 0 };
            s_this.EditWindowObjects[0].SetActive(false);
            s_this.EditWindowObjects[1].SetActive(false);
            s_this.EditWindowObjects[2].SetActive(true);
        }
        else
        {
            s_this.EscapeEditMode();
            return;
        }
    }

    public void EscapeEditMode()
    {
        holderIndex = new int[2] { 0, 0 };
        s_this.EditWindowObjects[0].SetActive(false);
        s_this.EditWindowObjects[1].SetActive(false);
        s_this.EditWindowObjects[2].SetActive(false);
        s_isEditing = false;
    }

    public void SetNotePos(int pos)
    {
        bool isGenerated = false;
        MultyNoteHolder targetHolder;

        if (s_editType == EditType.Normal)
        {
            //# Airial Note
            if (holderIndex[1] == 1)
            {
                NoteField.s_multyHolders.Find(item => item.stdPos == pos);
            }
        }
    }

    public void MoveNote(bool isUp = false, bool isDown = false, bool isLeft = false)
    {
        if (Input.GetKey(KeyCode.LeftControl)) { LegnthNote(isUp, isDown); }

        int stdPos;
        stdPos = noteHolder.stdPos;

        bool isSame;
        if (stdPos % (1600.0f / GuideGenerate.s_guideCount) == 0) { isSame = true; }
        else { isSame = false; }

        if (isUp)
        {

        }
        else if (isDown)
        {

        }
        else if (isLeft)
        {

        }
        else
        {

        }
    }
    
    private void LegnthNote(bool isUp, bool isDown)
    {
        if (isUp == isDown) { return; }

        if (isUp)
        {

        }
        else
        {

        }
    }
}