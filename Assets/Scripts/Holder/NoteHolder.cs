using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class NoteHolder : MonoBehaviour
{
    public static List<NoteHolder> holders = new List<NoteHolder>();

    public enum NoteType { Normal, Bottom, Air };
    public NoteType type = NoteType.Normal;
    public NoteHolder linkedHolder;
    public bool is2D = false;
    public BoxCollider2D noteCollider;
    public NormalNote noteClass;

    [SerializeField] GameObject[] noteObject;
    [SerializeField] Sprite[] AirSprite;

    public void UpdateNote(bool repeat)
    {

    }
}
