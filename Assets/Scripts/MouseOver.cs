using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        print(string.Format("Mouse Overed By {0}", name));
        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.s_Page = transform.GetComponentInParent<GuideHolder>().page;
        NoteGenerate.s_Indexer = transform.GetComponentInParent<GuideHolder>().indexer;
    }
}
