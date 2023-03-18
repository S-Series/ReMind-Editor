using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager s_this;
    [SerializeField] TMPro.TMP_InputField noteFileInput;
    void Awake()
    {
        s_this = this;
    }
    public static void SaveNoteFile()
    {
        string fileName;
        fileName = s_this.noteFileInput.text;
        if (fileName == "") { return; }

        SaveFile saveFile;
        saveFile = new SaveFile();

        InputManager.EnableInput(false);

        string saveData;
        NoteHolder holder;

        for (int i = 0; i < NoteField.s_noteHolders.Count; i++)
        {
            saveData = "";
            holder = NoteField.s_noteHolders[i];
            
            saveData += string.Format("{0:D11}\n", holder.stdMs);
            saveData += string.Format("{0:D11}\n", holder.stdPos);
            saveData += string.Format("{0:D2}|{1:D2}|{2:D2}|{3:D2}\n",
                holder.normals[0] == null ? 0 : holder.normals[0].legnth,
                holder.normals[1] == null ? 0 : holder.normals[1].legnth,
                holder.normals[2] == null ? 0 : holder.normals[2].legnth,
                holder.normals[3] == null ? 0 : holder.normals[3].legnth);
            saveData += string.Format("{0:D2}|{1:D2}|{2:D2}|{3:D2}\n",
                holder.airials[0] == null ? 0 : holder.airials[0].legnth,
                holder.airials[1] == null ? 0 : holder.airials[1].legnth,
                holder.airials[2] == null ? 0 : holder.airials[2].legnth,
                holder.airials[3] == null ? 0 : holder.airials[3].legnth);
            saveData += string.Format("{0:D2}|{1:D2}|{2:D2}|{3:D2}\n",
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].legnth,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].legnth,
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].SoundIndex,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].SoundIndex);
            saveData += string.Format("{0:D2}|{1:D2}|{2:D5}\n",
                holder.speedNote == null ? 00 : 01,
                holder.effectNote == null ? -1 : holder.effectNote.effectIndex,
                holder.effectNote == null ? 0 : holder.effectNote.value);
            saveData += string.Format("{0:D5}|{1:D5}",
                holder.speedNote == null ? 00 : 
                    Mathf.RoundToInt(System.Convert.ToSingle(holder.speedNote.bpm * 100)),
                holder.speedNote == null ? 00 : 
                    Mathf.RoundToInt(System.Convert.ToSingle(holder.speedNote.multiple * 1000)));
            print(saveData);
            saveFile.noteDatas.Add(saveData);
        }
        s_this.StartCoroutine(s_this.IWriteFile(saveFile, fileName));
    }

    private IEnumerator IWriteFile(SaveFile saveFile, string fileName)
    {
        
        string path, jsonData;
        path = Application.dataPath + "/_DataBox/" + fileName + ".json";
        jsonData = JsonUtility.ToJson(saveFile, true);

        yield return null;

        File.WriteAllText(jsonData, path);

        yield return null;

        InputManager.EnableInput(true);
    }
}

public class SaveFile
{
    //  00000000000     // Note Ms Value
    //  00000000000     // Note Pos Value
    //  01|01|01|01     // Normal Notes
    //  01|01|01|01     // Airial Notes
    //  01|01|01|01     // Bottom Note[2] || Sound Index[2]
    //  01|01|01600     // Special Note Indexes
    //  12000|00000

    public readonly double editorVersion = 1.0;
    public double bpm;
    public List<string> noteDatas = new List<string>();
}
