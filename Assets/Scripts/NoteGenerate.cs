using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerate : MonoBehaviour
{
    private static NoteGenerate s_this;
    public static bool s_isGenerating = false;
    public static int s_Page, s_Indexer, s_previewIndex;
    [SerializeField] GameObject[] previews;

    void Awake()
    {
        s_this = this;
        ChangePreview(-1);
    }
    private void Update()
    {
        if (!s_isGenerating) { return; }

        
    }

    public void ChangePreview(int index)
    {
        foreach(GameObject gameObject in previews) { gameObject.SetActive(false); }
        if (index < 0 || index >= previews.Length) { s_isGenerating = false; return; }
        else {}
    }
}
