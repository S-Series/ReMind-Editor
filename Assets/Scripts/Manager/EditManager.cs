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
        obj.GetComponent<SpriteRenderer>().color = new Color32(100, 255, 100, 255);
    }
}