using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManager : MonoBehaviour
{
    private static SaveManager s_this;
    private const double s_version = 1.0;
    private bool isActive, isPassed;
    [SerializeField] GameObject[] PopUpObjects;
    [SerializeField] TMPro.TMP_InputField noteFileInput;
    [SerializeField] UnityEngine.UI.Button[] fileButtons;

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

        saveFile.bpm = ValueManager.s_Bpm;
        saveFile.editorVersion = s_version;

        for (int i = 0; i < NoteField.s_noteHolders.Count; i++)
        {
            saveData = "";
            holder = NoteField.s_noteHolders[i];
            
            saveData += string.Format("|{0:D11}|__", holder.stdMs);
            saveData += string.Format("|{0:D11}|__", holder.stdPos);
            saveData += string.Format("|{0:D2}|{1:D2}|{2:D2}|{3:D2}|__",
                holder.normals[0] == null ? 0 : holder.normals[0].legnth,
                holder.normals[1] == null ? 0 : holder.normals[1].legnth,
                holder.normals[2] == null ? 0 : holder.normals[2].legnth,
                holder.normals[3] == null ? 0 : holder.normals[3].legnth);
            saveData += string.Format("|{0:D2}|{1:D2}|{2:D2}|{3:D2}|__",
                holder.airials[0] == null ? 0 : holder.airials[0].legnth,
                holder.airials[1] == null ? 0 : holder.airials[1].legnth,
                holder.airials[2] == null ? 0 : holder.airials[2].legnth,
                holder.airials[3] == null ? 0 : holder.airials[3].legnth);
            saveData += string.Format("|{0:D2}|{1:D2}|{2:D2}|{3:D2}|__",
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].legnth,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].legnth,
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].SoundIndex,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].SoundIndex);
            saveData += string.Format("|{0:D2}|{1}|{2:D5}|__",
                holder.speedNote == null ? 00 : 01,
                holder.effectNote == null ? -1 : string.Format("{0:D2}", holder.effectNote.effectIndex),
                holder.effectNote == null ? 0 : holder.effectNote.value);
            saveData += string.Format("|{0:D5}|{1:D5}|",
                holder.speedNote == null ? 00 : 
                    Mathf.RoundToInt(System.Convert.ToSingle(holder.speedNote.bpm * 100)),
                holder.speedNote == null ? 00 : 
                    Mathf.RoundToInt(System.Convert.ToSingle(holder.speedNote.multiple * 1000)));
            print(saveData);
            saveFile.noteDatas.Add(saveData);
        }
        s_this.StartCoroutine(s_this.IWriteFile(saveFile, fileName));
    }

    public static void LoadNoteFile()
    {
        string fileName;
        fileName = s_this.noteFileInput.text;
        if (fileName == "") { return; }

        s_this.StartCoroutine(s_this.IReadFile(fileName));
    }

    private IEnumerator IWriteFile(SaveFile saveFile, string fileName)
    {
        
        string path, jsonData;
        path = Application.dataPath + "/_DataBox/" + fileName + ".json";
        jsonData = JsonUtility.ToJson(saveFile, true);

        yield return null;

        File.WriteAllText(path, jsonData);

        yield return null;

        InputManager.EnableInput(true);
    }
    private IEnumerator IReadFile(string fileName)
    {
        string path;
        path = Application.dataPath + "/_DataBox/" + fileName + ".json";

        SaveFile saveFile;
        saveFile = new SaveFile();

        saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));

        if (saveFile.editorVersion < s_version)
        {
            isActive = false;
            isPassed = false;
            PopUpObjects[0].GetComponent<Animator>().SetTrigger("On");

            while (true)
            {
                if (isActive)
                {
                    if (!isPassed) { }
                    break;
                }
                yield return null;
            }
        }



        for ()
    }

    public void ConfirmButton(bool pass)
    {
        isActive = true;
        isPassed = pass;
    }
}

public class SaveFile
{
    public double bpm = 120.0;
    public double editorVersion = 1.0;

    // StdMs__StdPos__Normals__Airials__Bottom[2]|Sound[2]__Others
    public List<string> noteDatas = new List<string>();
}
