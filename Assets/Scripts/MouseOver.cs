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

        NoteGenerate.ShowPreview(
            lineValue: lineNum, 
            posValue: GetPosValue()
        );
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.GenerateNote(GetPosValue());
    }
    private int GetPosValue()
    {
        return transform.parent.GetComponent<GuideHolder>().posY;
    }
}
