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

        string saveData;
        NoteHolder holder;

        for (int i = 0; i < NoteField.s_noteHolders.Count; i++)
        {
            saveData = "";
            holder = NoteField.s_noteHolders[i];
            
            saveData += string.Format("{0:D11}\n", holder.stdMs);
            saveData += string.Format("{0:11D}\n", holder.stdPos);
            saveData += string.Format("{0:2D}|{1:2D}|{2:2D}|{3:2D}",
                holder.normals[0] == null ? 0 : holder.normals[0].legnth,
                holder.normals[1] == null ? 0 : holder.normals[1].legnth,
                holder.normals[2] == null ? 0 : holder.normals[2].legnth,
                holder.normals[3] == null ? 0 : holder.normals[3].legnth);
            saveData += string.Format("{0:2D}|{1:2D}|{2:2D}|{3:2D}",
                holder.airials[0] == null ? 0 : holder.airials[0].legnth,
                holder.airials[1] == null ? 0 : holder.airials[1].legnth,
                holder.airials[2] == null ? 0 : holder.airials[2].legnth,
                holder.airials[3] == null ? 0 : holder.airials[3].legnth);
            saveData += string.Format("{0:2D}|{1:2D}|{2:2D}|{3:2D}",
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].legnth,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].legnth,
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].SoundIndex,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].SoundIndex);
        }
    }
}

public class SaveFile
{
    //  00000000000     // Note Ms Value
    //  00000000000     // Note Pos Value
    //  01|01|01|01     // Normal Notes
    //  01|01|01|01     // Airial Notes
    //  01|01|01|01     // Bottom Note[2] || Sound Index[2]
    //  01|01|01|01     // Special Note Indexes
    //  12000|00000

    public readonly double editorVersion = 1.0;
    public List<string> noteDatas = new List<string>();
}
