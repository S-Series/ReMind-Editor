using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{
    void OnMouseOver()
    {
        if (!NoteGenerate.s_isGenerating) { return; }

        NoteGenerate.s_Page = transform.GetComponentInParent<GuideHolder>().page;
        NoteGenerate.s_Indexer = transform.GetComponentInParent<GuideHolder>().indexer;
    }
}
