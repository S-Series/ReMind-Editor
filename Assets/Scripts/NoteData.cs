using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class NoteData : MonoBehaviour
{
    public int NoteLine;
    public NoteType noteType;

    private Color32 Color32;
    private SpriteRenderer[] renderers = null;
    [SerializeField] private bool isGameNote;
    [SerializeField] private Transform[] LongNotes;

    public void Selected()
    {
        NoteHolder holder;
        holder = GetComponentInParent<NoteHolder>();
        if (holder == null) { throw new System.Exception("Holder is NULL"); }
        EditManager.SelectHolder(holder);

        int index = NoteLine - 1;
        switch (noteType)
        {
            case NoteType.Normal:
                EditManager.SelectNote(holder.normals[index]);
                break;
            case NoteType.Airial:
                EditManager.SelectNote(holder.airials[index]);
                break;
            case NoteType.Floor:
                EditManager.SelectNote(holder.floors[index]);
                break;
            case NoteType.Speed:
                EditManager.SelectNote(holder.speedNote);
                break;
            case NoteType.Effect:
                EditManager.SelectNote(holder.effectNote);
                break;
            default: throw new System.Exception("");
        }
    }
    public NoteHolder GetNoteHolder()
    {
        NoteHolder holder;
        holder = GetComponentInParent<NoteHolder>();
        if (holder == null) { throw new System.Exception("Holder is NULL"); }
        return holder;
    }
}
