using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Overed");

        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.posY = transform.parent.GetComponent<GuideHolder>().posY;
        switch (tag.ToString())
        {

            case "01":
                NoteGenerate.s_Line = 1;
                break;

            case "02":
                NoteGenerate.s_Line = 2;
                break;

            case "03":
                NoteGenerate.s_Line = 3;
                break;

            case "04":
                NoteGenerate.s_Line = 4;
                break;

            default:
                NoteGenerate.s_Line = 0;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Clicked");

        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.GenerateNote();
    }
}
