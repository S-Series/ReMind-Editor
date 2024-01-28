using GameNote;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteClick : MonoBehaviour, IPointerClickHandler
{
    public int NoteLine;
    public NoteType noteType;
    [SerializeField] private bool isNoteParent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (NoteGenerate.s_isGenerating) { return; }

        EditManager.Select(this);
    }
    public NoteHolder GetNoteHolder()
    {
        NoteHolder holder;
        holder = GetComponentInParent<NoteHolder>();
        if (holder == null) { throw new System.Exception("Holder is NULL"); }
        return holder;
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
