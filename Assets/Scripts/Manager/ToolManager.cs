using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class ToolManager : MonoBehaviour
{
    private static ToolManager s_this;
    public enum NoteType { Null, Normal, Speed, Effect };
    public static NoteType noteType = NoteType.Null;

    public static class EraseManager
    {

    }

    public void Awake()
    {
        s_this = this;
    }
    public static void Tool(int index)
    {
        s_this.ToolButton(index);
    }
    public void ToolButton(int index)
    {
        NoteGenerate.ChangePreview(index);
    }
}
