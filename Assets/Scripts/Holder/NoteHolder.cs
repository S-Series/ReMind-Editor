using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameNote;

public class NoteHolder : MonoBehaviour, IPointerClickHandler
{
    public static List<NoteHolder> holders = new List<NoteHolder>();

    public enum NoteType { Normal, Bottom, Air };
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {

        }
    }
}
