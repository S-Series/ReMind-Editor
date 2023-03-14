using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager s_this;
    void Awake()
    {
        s_this = this;
    }
    public static void SaveNoteFile()
    {
        SaveFile saveFile;
        saveFile = new SaveFile();
    }
}
//  -----------
//  00000000000     // Note Ms Value
//  00000000000     // Note Pos Value
//  01|01|01|01     // Normal Notes
//  01|01|01|01     // Airial Notes
//  01|01|01|01     // Bottom Note[2] || Sound Index[2]
//  01|01|01|01     // Special Note Indexes
//  12000|00000
public class SaveFile
{
    public readonly double editorVersion = 1.0;
    public List<string> noteDatas = new List<string>();
}
