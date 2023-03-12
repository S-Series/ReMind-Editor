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
    }

    public static void Escape()
    {
        if (s_SelectedObject != null)
        {
            s_SelectedObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }

        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EditMode(true); }
        InputManager.Editing(false);
    }

    public void PosNote(bool isUp)
    {
        int pos;
        bool isFit;

        pos = s_SelectNoteHolder.stdPos;
        isFit = GuideGenerate.s_guidePos.Contains(pos % 1600);

        
    }
}