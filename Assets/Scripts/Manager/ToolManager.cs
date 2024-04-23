using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class ToolManager : MonoBehaviour
{
    private static ToolManager s_this;
    public static NoteType noteType = NoteType.None;

    public void Awake()
    {
        s_this = this;
    }
    public static void Tool(int index)
    {
    }
}
