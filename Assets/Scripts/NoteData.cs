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

    public void Length(int length)
    {
        if (noteType != NoteType.Normal || noteType != NoteType.Scratch) { return; }

        if (length <= 1)
        {
            LongNotes[0].gameObject.SetActive(true);

            LongNotes[1].gameObject.SetActive(false);
            LongNotes[2].gameObject.SetActive(false);
            LongNotes[3].gameObject.SetActive(false);
        }
        else
        {
            LongNotes[0].gameObject.SetActive(false);

            LongNotes[1].gameObject.SetActive(true);
            LongNotes[2].gameObject.SetActive(true);
            LongNotes[3].gameObject.SetActive(true);

            if (isGameNote)
            {
                if (noteType == NoteType.Normal)
                {
                    LongNotes[1].localScale = new Vector3(1, 2 * length * 0.05683594f, 1);
                    LongNotes[3].localPosition = new Vector3(0, 2 * length * 0.5681819f, 0);
                }
                else
                {
                    LongNotes[1].localScale = new Vector3(1, 2 * length * 0.05209961f, 1);
                    LongNotes[3].localPosition = new Vector3(0, 2 * length * 0.5208334f, 0);
                }

            }
            else
            {
                LongNotes[1].localScale = new Vector3(96, length * 10, 96);
                LongNotes[3].localPosition = new Vector3(0, 100 * length, 0);
            }
        }
    }

    public void Selected(bool isTure)
    {
        if (renderers == null) { renderers = GetComponentsInChildren<SpriteRenderer>(); }

        if (isTure)
        {
            switch (noteType)
            {
                case NoteType.Normal:
                    Color32 = new Color32(100, 255, 0, 255);
                    break;

                case NoteType.Airial:
                    Color32 = new Color32(255, 200, 0, 255);
                    break;

                case NoteType.Scratch:
                    Color32 = new Color32(0, 255, 200, 255);
                    break;

                case NoteType.Speed:
                    Color32 = new Color32(255, 0, 125, 255);
                    break;
                
                case NoteType.Effect:
                    Color32 = new Color32(0, 255, 200, 255);
                    break;

                default: throw new System.Exception("NoteType UnAvaialble");
            }
        }
        else { Color32 = new Color32(255, 255, 255, 255); }

        foreach (SpriteRenderer renderer in renderers) { renderer.color = Color32; }
    }
    public Transform GetTransform(bool isSingle)
    {
        if (isSingle) { return LongNotes[0]; }
        else { return LongNotes[1]; }
    }
    public NoteHolder GetNoteHolder()
    {
        NoteHolder holder;
        holder = GetComponentInParent<NoteHolder>();
        if (holder == null) { throw new System.Exception("Holder is NULL"); }
        return holder;
    }
}
