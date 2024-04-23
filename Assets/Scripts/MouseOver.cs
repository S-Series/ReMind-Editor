using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] int lineNum;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.s_Line = lineNum;
        NoteGenerate.posY = transform.parent.GetComponent<GuideHolder>().posY;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.GenerateNote();
    }
}
