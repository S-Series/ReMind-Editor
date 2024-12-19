using GameNote;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool isNoteParent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (NoteGenerate.s_isGenerating) { return; }

        //if (isNoteParent) { EditManager.SelectNote(GetComponentInParent<NoteData>()); }
        //else { EditManager.SelectNote(GetComponent<NoteData>()); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Draggable")) { return; }

        DragSelect.AddObject(this);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Draggable")) { return; }

        DragSelect.RemoveObject(this);
    }
}
