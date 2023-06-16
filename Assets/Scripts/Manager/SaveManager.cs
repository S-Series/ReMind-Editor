using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameNote;

public class SaveManager : MonoBehaviour
{
    private static SaveManager s_this;
    private static string s_noteFileName;
    private static bool isLoadable = false;
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
        if (!isLoadable) { return; }

        string fileName;
        fileName = s_this.noteFileInput.text;
        if (fileName == "") { return; }

        SaveFile saveFile;
        saveFile = new SaveFile();

        InputManager.EnableInput(false);

        string saveData;
        NoteHolder holder;

        saveFile.bpm = ValueManager.s_Bpm;
        saveFile.delay = ValueManager.s_Delay;
        saveFile.editorVersion = s_version;

        NoteField.SortNoteHolder();

        for (int i = 0; i < NoteField.s_noteHolders.Count; i++)
        {
            saveData = "";
            holder = NoteField.s_noteHolders[i];

            saveData += string.Format("|{0:D11}|__", holder.stdMs);
            saveData += string.Format("|{0:D11}|__", holder.stdPos);
            saveData += string.Format("|{0:D2}|{1:D2}|{2:D2}|{3:D2}|__",
                holder.normals[0] == null ? 0 : LengthToString(holder.normals[0].length),
                holder.normals[1] == null ? 0 : LengthToString(holder.normals[1].length),
                holder.normals[2] == null ? 0 : LengthToString(holder.normals[2].length),
                holder.normals[3] == null ? 0 : LengthToString(holder.normals[3].length));
            saveData += string.Format("|{0}|{1}|{2}|{3}|__",
                holder.airials[0] == null ? "--" : LengthToString(holder.airials[0].length),
                holder.airials[1] == null ? "--" : LengthToString(holder.airials[1].length),
                holder.airials[2] == null ? "--" : LengthToString(holder.airials[2].length),
                holder.airials[3] == null ? "--" : LengthToString(holder.airials[3].length));
            saveData += string.Format("|{0}{1}|{2}{3}|{4}{5}|{6}{7}|__",
                holder.normals[0] == null ? "-" : holder.normals[0].isGuideLeft ? 1 : 0,
                holder.airials[0] == null ? "-" : holder.airials[0].isGuideLeft ? 1 : 0,
                holder.normals[1] == null ? "-" : holder.normals[1].isGuideLeft ? 1 : 0,
                holder.airials[1] == null ? "-" : holder.airials[1].isGuideLeft ? 1 : 0,
                holder.normals[2] == null ? "-" : holder.normals[2].isGuideLeft ? 1 : 0,
                holder.airials[2] == null ? "-" : holder.airials[2].isGuideLeft ? 1 : 0,
                holder.normals[3] == null ? "-" : holder.normals[3].isGuideLeft ? 1 : 0,
                holder.airials[3] == null ? "-" : holder.airials[3].isGuideLeft ? 1 : 0);
            saveData += string.Format("|{0:D2}|{1:D2}|{2:D2}|{3:D2}|__",
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].length,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].length,
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].SoundIndex,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].SoundIndex);
            saveData += string.Format("|{0}|{1}|{2:D5}|__",
                holder.speedNote == null ? "00" : "01",
                holder.effectNote == null ? "-1" : string.Format("{0:D2}", holder.effectNote.effectIndex),
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
        InputManager.EnableInput(false);

        //$ Check NoteField
        if (NoteField.s_noteHolders.Count != 0)
        {
            isActive = false;
            isPassed = false;
            PopUpObjects[0].GetComponent<Animator>().SetTrigger("On");

            while (true)
            {
                if (isActive)
                {
                    PopUpObjects[0].GetComponent<Animator>().SetTrigger("Off");
                    if (!isPassed) { InputManager.EnableInput(true);  yield break; }
                    break;
                }
                yield return null;
            }
        }

        yield return NoteField.IResetHolderList();

        string path;
        path = Application.dataPath + "/_DataBox/" + fileName + ".json";

        SaveFile saveFile;
        saveFile = new SaveFile();

        saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));

        //$ Check Old Version File
        if (saveFile.editorVersion < s_version)
        {
            isActive = false;
            isPassed = false;
            PopUpObjects[1].GetComponent<Animator>().SetTrigger("On");

            while (true)
            {
                if (isActive)
                {
                    PopUpObjects[1].GetComponent<Animator>().SetTrigger("Off");
                    if (!isPassed) { InputManager.EnableInput(true); yield break; }
                    break;
                }
                yield return null;
            }
        }

        ValueManager.s_Bpm = saveFile.bpm;
        ValueManager.s_Delay = saveFile.delay;
        ValueManager.UpdateInputField();

        string[] saveData;
        string[] noteData;

        NoteHolder copyHolder;
        NormalNote normal;
        SpeedNote speed;
        EffectNote effect;

        for (int i = 0; i < saveFile.noteDatas.Count; i++)
        {
            saveData = saveFile.noteDatas[i].Split("__", StringSplitOptions.RemoveEmptyEntries);

            copyHolder = NoteGenerate.GenerateNoteManual(Convert.ToInt32(saveData[1].Replace("|", "")));
            copyHolder.stdMs = Convert.ToInt32(saveData[0].Replace("|", ""));

            noteData = saveData[2].Split('|', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < 4; j++)
            {
                if (noteData[j] == "00") { copyHolder.normals[j] = null; }
                else
                {
                    normal = NormalNote.Generate();
                    normal.isAir = false;
                    normal.ms = copyHolder.stdMs;
                    normal.pos = copyHolder.stdPos;
                    normal.line = j + 1;
                    normal.holder = copyHolder;
                    normal.SoundIndex = 0;
                    normal.length = StringToLength(noteData[j]);
                    copyHolder.normals[j] = normal;
                }
            }

            noteData = saveData[3].Split('|', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < 4; j++)
            {
                if (noteData[j] == "--") { copyHolder.airials[j] = null; }
                else
                {
                    normal = NormalNote.Generate();
                    normal.isAir = true;
                    normal.ms = copyHolder.stdMs;
                    normal.pos = copyHolder.stdPos;
                    normal.line = j + 1;
                    normal.holder = copyHolder;
                    normal.SoundIndex = 0;
                    normal.length = StringToLength(noteData[j]);
                    copyHolder.airials[j] = normal;
                }
            }
            
            noteData = saveData[4].Split('|', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < 4; j++)
            {
                if (copyHolder.normals[j] != null)
                {
                    if (noteData[j][0] == '-')
                    {
                        copyHolder.normals[j].isGuideLeft =
                            copyHolder.normals[j].line < 3 ? true : false;
                    }
                    else
                    {
                        copyHolder.normals[j].isGuideLeft =
                            noteData[j][0] == '1' ? true : false;
                    }
                }
                if (copyHolder.airials[j] != null)
                {
                    if (noteData[j][1] == '-')
                    {
                        copyHolder.airials[j].isGuideLeft =
                            copyHolder.airials[j].line < 3 ? true : false;
                    }
                    else
                    {
                        copyHolder.airials[j].isGuideLeft =
                            noteData[j][1] == '1' ? true : false;
                    }
                }
            }

            noteData = saveData[5].Split('|', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < 2; j++)
            {
                if (noteData[j] == "00") { copyHolder.bottoms[j] = null; }
                else
                {
                    normal = NormalNote.Generate();
                    normal.ms = copyHolder.stdMs;
                    normal.pos = copyHolder.stdPos;
                    normal.line = j + 1;
                    normal.holder = copyHolder;
                    normal.length = StringToLength(noteData[j]);
                    normal.SoundIndex = Convert.ToInt32(noteData[j + 2]);
                    copyHolder.bottoms[j] = normal;
                }
            }

            noteData = saveData[6].Split('|', StringSplitOptions.RemoveEmptyEntries);
            if (noteData[1] != "-1")
            {
                effect = EffectNote.Generate();
                effect.effectIndex = Convert.ToInt32(noteData[1]);
                effect.value = Convert.ToInt32(noteData[2]);
            }
            if (noteData[0] == "01")
            {
                noteData = saveData[6].Split('|', StringSplitOptions.RemoveEmptyEntries);
                speed = SpeedNote.Generate();
                speed.ms = copyHolder.stdMs;
                speed.pos = copyHolder.stdPos;
                speed.bpm = Convert.ToDouble(noteData[0]) / 100d;
                speed.multiple = Convert.ToDouble(noteData[1]) / 1000d;
            }

            copyHolder.UpdateNote();
            copyHolder.CheckDestroy();
            yield return null;
        }
    
        InputManager.EnableInput(true);

        print(ValueManager.s_Bpm);
    }

    private static string LengthToString(int value)
    {
        if (value < 1) { return "--"; }
        else if (value > 259) { return "--"; }
        char c;
        c = (char)(Mathf.FloorToInt(value / 10.0f) + 63);
        return String.Format("{0}{1}", c, value % 10);
    }
    private static Int32 StringToLength(string value)
    {
        char[] cArr;
        cArr = value.ToCharArray();
        return (Convert.ToInt32(cArr[0]) - 63) * 10 + (int)Char.GetNumericValue(cArr[1]);
    }

    public void SelectInputFileName(bool select)
    {
        if (select)
        {
            noteFileInput.textComponent.color = new Color32(000, 000, 000, 255);
        }
        else
        {
            s_noteFileName = noteFileInput.text;

            if (s_noteFileName.Contains('!')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('@')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('#')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('$')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('%')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('^')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('&')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('*')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('+')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('=')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('`')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains(',')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('.')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('/')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('?')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains(':')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains(';')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('<')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('>')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('\'')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('\"')) { DisableLoad(); return; }
            else if (s_noteFileName.Contains('\\')) { DisableLoad(); return; }

            isLoadable = true;
            s_noteFileName.Replace(' ', '_');
            noteFileInput.text = s_noteFileName;
            noteFileInput.textComponent.color = new Color32(065, 180, 000, 255);
        }
    }
    public void ConfirmButton(bool pass)
    {
        isActive = true;
        isPassed = pass;
    }

    private void DisableLoad()
    {
        isLoadable = false;
        noteFileInput.textComponent.color = new Color32(220, 025, 000, 255);
    }
}

public class SaveFile
{
    public int delay = 0;
    public double bpm = 120.0;
    public double editorVersion = 1.0;

    // StdMs__StdPos__Normals__Airials__Bottom[2]|Sound[2]__Others
    public List<string> noteDatas = new List<string>();
}
