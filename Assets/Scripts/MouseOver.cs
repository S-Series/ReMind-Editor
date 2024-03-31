using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!NoteGenerate.s_isGenerating) { return; }

        try { NoteGenerate.s_Line = System.Convert.ToInt32(tag.ToString()); }
        catch { NoteGenerate.s_Line = 0; }
        NoteGenerate.posY = transform.parent.GetComponent<GuideHolder>().posY;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.GenerateNote();
    }
}
