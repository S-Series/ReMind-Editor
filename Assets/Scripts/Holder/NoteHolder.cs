using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class NoteHolder : MonoBehaviour
{
    public enum NoteType{Normal, Bottom, Air};
    public NoteType type = NoteType.Normal;
    public NoteHolder linkedHolder;
    public int legnth = 0;
    public bool is2D = false;
    public BoxCollider2D noteCollider;
    public NormalNote noteClass;

    [SerializeField] GameObject[] noteObject;

    public void UpdateNote(bool repeat)
    {
        
    }

    public void OnMouseDown()
    {
        
    }
}
