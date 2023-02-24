using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerate : MonoBehaviour
{
    private static NoteGenerate s_this;
    public static bool s_isGenerating = false;
    public static int s_Line, s_Page, s_Indexer, s_previewIndex;

    private int posX = 0, posY = 0, posZ = 0;

    [SerializeField] GameObject[] previews;
    /// <summary>
    /// previews[0] = Normal Note
    /// previews[1] = Bottom Note
    /// previews[2] = Airial Note
    /// previews[3] = Speed Note
    /// previews[4] = Effect Note
    /// previews[5] = 
    /// </summary>

    void Awake()
    {
        s_this = this;
        ChangePreview(-1);
    }

    private void Update()
    {
        if (!s_isGenerating) { return; }

        posX = s_Line * 200 - 500;
        posY = (s_Page - 1) * 1600 + Mathf.RoundToInt(1600.0f / GuideGenerate.s_guideCount * s_Indexer);
        posZ = 0;

        if (s_previewIndex == 1)
        {
            if (posX < 0) { posX = -200; }
            else { posX = 200; }
        }
        else if (s_previewIndex == 2) { posZ = -500; }
        else if (s_previewIndex == 3 || s_previewIndex == 4) { posX = 0; }

        previews[s_previewIndex].transform.localPosition = new Vector3(posX, posY, posZ);
    }

    public static void GenerateNote()
    {
        switch (ToolManager.noteType)
        {
            case ToolManager.NoteType.Normal:
                break;

            case ToolManager.NoteType.Speed:
                break;

            case ToolManager.NoteType.Effect:
                break;

            default: return;
        }
    }

    public static void ChangePreview(int index)
    {
        foreach (GameObject gameObject in s_this.previews) { gameObject.SetActive(false); }
        if (index < 0 || index >= s_this.previews.Length) { s_isGenerating = false; return; }
        else { s_isGenerating = true; }

        s_this.previews[index].SetActive(true);
    }

    public void Escape()
    {
        s_isGenerating = true;
        foreach (GameObject gameObject in previews) { gameObject.SetActive(false); }
    }
}
