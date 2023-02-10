using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHolder : MonoBehaviour
{
    public enum NoteType{Normal, Bottom, };
    public NoteType type = NoteType.Normal;
    public bool is2D = false;
    public bool isLong = false;

    [SerializeField] GameObject[] noteObject;

    public void OnMouseDown()
    {
        
    }

    public void UpdateNote()
    {
        
    }
}
