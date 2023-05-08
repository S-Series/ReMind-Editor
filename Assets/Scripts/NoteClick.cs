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
}
