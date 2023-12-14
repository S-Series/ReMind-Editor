using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameNote;
using AESWithJava.Con;
using Ookii.Dialogs;
using System.Windows.Forms;

public class SaveManager : MonoBehaviour
{
    private static SaveManager s_this;
    private static string s_LoadedPath = "", s_noteFileName;
    private static bool isAlt = false, isLoaded = false, isLoadable = true;
    private bool isActive, isPassed;
    public static string HiddenKey; //$ Public Hidden Key ------------------------------//
    [SerializeField] private string _HiddenKey; //$ Hidden Key Setting -----------------//
    [SerializeField] GameObject[] PopUpObjects;
    [SerializeField] InputAction[] AltAction;
    [SerializeField] TMPro.TMP_InputField noteFileInput;
    [SerializeField] UnityEngine.UI.Button[] fileButtons;

    private void Awake()
    {
        s_this = this;
        AltAction[0].performed += item => { isAlt = true; };
        AltAction[1].performed += item => { isAlt = false; };
        AltAction[0].Enable();
        AltAction[1].Enable();
    }
    private void Start()
    {
        HiddenKey = PlayerPrefs.GetString("HiddenKey");
        LoadNoteFile();
    }
    public static void SaveNoteFile()
    {
        if (!isLoadable) { return; }

        print("run");

        string path = "";
        if (isAlt || !isLoaded)
        {
            VistaSaveFileDialog dialog;
            dialog = new VistaSaveFileDialog();
            dialog.Filter = "nd files (*.nd)|*.nd";
            dialog.FilterIndex = 1;
            dialog.Title = "Save Data";
            dialog.InitialDirectory = (UnityEngine.Application.dataPath + @"\_DataBox").Replace("/", "\\");
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream;
                if ((stream = dialog.OpenFile()) != null)
                {
                    path = dialog.FileName;
                    stream.Close();
                    if (File.Exists(path)) { File.Delete(path); }
                }
                else { return; }
            }
            else { return; }
        }
        else { path = s_LoadedPath; }

        print(path);

        SaveFile saveFile;
        saveFile = new SaveFile();

        InputManager.EnableInput(false);

        string saveData;
        NoteHolder holder;

        saveFile.bpm = ValueManager.s_Bpm;
        saveFile.delay = ValueManager.s_Delay;
        saveFile.version = VersionManager.GetVersion();

        NoteField.SortNoteHolder();

        for (int i = 0; i < NoteField.s_noteHolders.Count; i++)
        {
            saveData = "";
            holder = NoteField.s_noteHolders[i];

            saveData += string.Format("|{0:D11}|#", holder.stdMs);
            saveData += string.Format("|{0:D11}|#", holder.stdPos);
            saveData += string.Format("|{0:D2}|{1:D2}|{2:D2}|{3:D2}|#",
                holder.normals[0] == null ? 0 : LengthToString(holder.normals[0].length),
                holder.normals[1] == null ? 0 : LengthToString(holder.normals[1].length),
                holder.normals[2] == null ? 0 : LengthToString(holder.normals[2].length),
                holder.normals[3] == null ? 0 : LengthToString(holder.normals[3].length));
            saveData += string.Format("|{0}|{1}|{2}|{3}|#",
                holder.airials[0] == null ? "--" : LengthToString(holder.airials[0].length),
                holder.airials[1] == null ? "--" : LengthToString(holder.airials[1].length),
                holder.airials[2] == null ? "--" : LengthToString(holder.airials[2].length),
                holder.airials[3] == null ? "--" : LengthToString(holder.airials[3].length));
            saveData += string.Format("|{0:D2}|{1:D2}|{2:D2}|{3:D2}|#",
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].length,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].length,
                holder.bottoms[0] == null ? 0 : holder.bottoms[0].SoundIndex,
                holder.bottoms[1] == null ? 0 : holder.bottoms[1].SoundIndex);
            saveData += string.Format("|{0}|{1}|{2:D5}|#",
                holder.speedNote == null ? "00" : "01",
                holder.effectNote == null ? "-1" : string.Format("{0:D2}", holder.effectNote.effectIndex),
                holder.effectNote == null ? 0 : holder.effectNote.value);
            saveData += string.Format("|{0:D5}|{1:D5}|",
                holder.speedNote == null ? 00 :
                    Mathf.RoundToInt(System.Convert.ToSingle(holder.speedNote.bpm * 100)),
                holder.speedNote == null ? 00 :
                    Mathf.RoundToInt(System.Convert.ToSingle(holder.speedNote.multiple * 1000)));
            saveFile.noteDatas.Add(saveData);
        }
        s_this.StartCoroutine(s_this.IWriteFile(saveFile, path));
    }
    public static void LoadNoteFile()
    {
        string path;
        path = "";

        VistaOpenFileDialog dialog;
        dialog = new VistaOpenFileDialog();
        dialog.Filter = "nd files (*.nd)|*.nd";
        dialog.FilterIndex = 1;
        dialog.Title = "Open Data";
        dialog.InitialDirectory = (UnityEngine.Application.dataPath + @"\_DataBox").Replace("/", "\\");
        dialog.RestoreDirectory = true;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            Stream stream;
            if ((stream = dialog.OpenFile()) != null)
            {
                stream.Close();
                path = dialog.FileName;
            }
            else { return; }
        }
        else { return; }

        print (path);
        if (path == "") { return; }

        s_this.StartCoroutine(s_this.IReadFile(path));
    }

    private IEnumerator IWriteFile(SaveFile saveFile, string path)
    {
        string jsonData;
        jsonData = JsonUtility.ToJson(saveFile, true);

        yield return null;


        File.WriteAllText(path + ".nd", JsonAES.Encrypt(jsonData, HiddenKey));

        yield return null;

        InputManager.EnableInput(true);
    }
    private IEnumerator IReadFile(string path)
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

        SaveFile saveFile;
        saveFile = new SaveFile();
        
        try 
        {
            saveFile = JsonUtility.FromJson<SaveFile>(JsonAES.Decrypt(File.ReadAllText(path), HiddenKey));
        }
        catch
        {
            try
            {
                saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));
            }
            catch { yield break; }
        }

        print(JsonAES.Decrypt(File.ReadAllText(path), HiddenKey));

        //$ Check Old Version File
        if (!VersionManager.isVersionMatch(saveFile.version))
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
            int dataIndex = 1;
            saveData = saveFile.noteDatas[i].Split("#", StringSplitOptions.RemoveEmptyEntries);

            copyHolder = NoteGenerate.GenerateNoteManual(Convert.ToInt32(saveData[dataIndex].Replace("|", "")));
            copyHolder.stdMs = Convert.ToInt32(saveData[dataIndex - 1].Replace("|", ""));

            dataIndex++; //$ == 2
            noteData = saveData[dataIndex].Split('|', StringSplitOptions.RemoveEmptyEntries);
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

            dataIndex++; //$ == 3
            noteData = saveData[dataIndex].Split('|', StringSplitOptions.RemoveEmptyEntries);
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
            
            dataIndex++; //$ == 4
            noteData = saveData[dataIndex].Split('|', StringSplitOptions.RemoveEmptyEntries);
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

            dataIndex++; //$ == 5
            noteData = saveData[dataIndex].Split('|', StringSplitOptions.RemoveEmptyEntries);
            if (noteData[1] != "-1")
            {
                effect = EffectNote.Generate();
                effect.effectIndex = Convert.ToInt32(noteData[1]);
                effect.value = Convert.ToInt32(noteData[2]);
            }
            if (noteData[0] == "01")
            {
                noteData = saveData[dataIndex + 1].Split('|', StringSplitOptions.RemoveEmptyEntries);
                speed = SpeedNote.Generate();
                speed.ms = copyHolder.stdMs;
                speed.pos = copyHolder.stdPos;
                speed.bpm = Convert.ToDouble(noteData[0]) / 100d;
                speed.multiple = Convert.ToDouble(noteData[1]) / 1000d;
            }

            copyHolder.UpdateNote();
            copyHolder.UpdateScale();
            copyHolder.CheckDestroy();
            yield return null;
        }
    
        InputManager.EnableInput(true);
        isLoaded = true;
        s_LoadedPath = path;
    }

    private static string LengthToString(int value)
    {
        if (value < 1) { return "--"; }
        else if (value > 259) { return "--"; }
        char c;
        c = (char)(Mathf.FloorToInt(value / 10.0f) + 65);
        return String.Format("{0}{1}", c, value % 10);
    }
    private static Int32 StringToLength(string value)
    {
        char[] cArr;
        cArr = value.ToCharArray();
        return (Convert.ToInt32(cArr[0]) - 65) * 10 + (int)Char.GetNumericValue(cArr[1]);
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

    #region Setting Hidden Key
    [UnityEngine.ContextMenu("Saving Hidden Key")]
    private void SaveHiddenKey()
    {
        PlayerPrefs.SetString("HiddenKey", _HiddenKey);
        _HiddenKey = "";
        print(PlayerPrefs.GetString("HiddenKey"));
    }
    [UnityEngine.ContextMenu("Show Hidden Key")]
    private void LoadHiddenKey() { StartCoroutine(ILoadHiddenKey()); }
    private IEnumerator ILoadHiddenKey()
    {
        _HiddenKey = PlayerPrefs.GetString("HiddenKey");
        yield return new WaitForSeconds(0.5f);
        _HiddenKey = "";
    }
    #endregion
}

public class SaveFile
{
    public int delay = 0;
    public int[] version = { 1, 0, 0 };
    public double bpm = 120.0;

    public List<string> noteDatas = new List<string>();
}
