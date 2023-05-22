using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool isNoteParent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (NoteGenerate.s_isGenerating) { return; }

        if (!isNoteParent) { EditManager.Select(this.gameObject); }
        else { EditManager.Select(this.transform.parent.gameObject); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print("Entered");
        if (!other.gameObject.CompareTag("Draggable")) { return; }

        if (!isNoteParent) { DragSelect.AddObject(this.gameObject); }
        else { DragSelect.AddObject(this.transform.parent.gameObject); }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        print("Exited");
        if (!other.gameObject.CompareTag("Draggable")) { return; }

        if (!isNoteParent) { DragSelect.RemoveObject(this.gameObject); }
        else { DragSelect.RemoveObject(this.transform.parent.gameObject); }
    }
}
