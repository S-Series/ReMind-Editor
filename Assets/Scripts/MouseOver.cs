using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerUpHandler, IPointerClickHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.s_Page = transform.GetComponentInParent<ColliderHolder>().page;
        NoteGenerate.s_Indexer = transform.GetComponentInParent<ColliderHolder>().index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!NoteGenerate.s_isGenerating) { return; }
        NoteGenerate.GenerateNote();
    }
}
