using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHolder : MonoBehaviour
{
    public enum NoteType{Normal, Bottom, Air};
    public NoteType type = NoteType.Normal;
    public int legnth = 0;
    public bool is2D = false;

    [SerializeField] GameObject[] noteObject;

    public void OnMouseDown()
    {
        
    }

    public void UpdateNote()
    {
        
    }
}
