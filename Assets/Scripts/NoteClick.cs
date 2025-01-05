using GameNote;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteClick : MonoBehaviour, IPointerClickHandler
{
    private NoteData data;
    [SerializeField] private bool isNoteParent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (NoteGenerate.s_isGenerating) { return; }

        if (data == null) { data = GetComponentInParent<NoteData>(); }

        data.Selected();
    }

    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Draggable")) { return; }

        DragSelect.AddObject(this);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Draggable")) { return; }

        DragSelect.RemoveObject(this);
    }*/
}
