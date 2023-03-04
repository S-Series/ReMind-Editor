using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerUpHandler, IPointerClickHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        print("Overed");

        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.posX = NoteField.scroll;
        NoteGenerate.posY = NoteField.scroll;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Clicked");

        if (!NoteGenerate.s_isGenerating) { return; }
        
    }
}
