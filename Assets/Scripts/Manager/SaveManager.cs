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
using GameData;
using System.Linq;

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
    }
    public static void SaveNoteFile()
    {
        if (!isLoadable) { return; }

        string path = "";
        if (isAlt || !isLoaded)
        {
            isAlt = false;
            VistaSaveFileDialog dialog;
            dialog = new VistaSaveFileDialog();
            dialog.Filter = "All Files|*.*";
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
                    if (File.Exists(path))
                    {
                        while (path.Contains(".nd"))
                        {
                            string data;
                            data = File.ReadAllText(path);
                            File.Delete(path);
                            if (path == ".nd") { path = "_"; }
                            else { path = path.Substring(0, path.Length - 3);  }
                            File.WriteAllText(path, data);
                        }
                        File.Move(path, path + ".nd");
                    }
                }
                else { return; }
            }
            else { return; }
        }
        else { path = s_LoadedPath; }

        SaveFile saveFile;
        saveFile = new SaveFile();

        NoteField.SortNoteHolder();

        List<string> datas;
        datas = new List<string>();
        for (int i = 0; i < NoteHolder.s_holders.Count; i++)
        {
            datas.Add(HolderToData(NoteHolder.s_holders[i]));
        }
        saveFile.noteDatas = datas;
        saveFile.bpm = ValueManager.s_Bpm;
        saveFile.delay = ValueManager.s_Delay;
        saveFile.gameMode = (int)GameManager.gameMode - 4;
        saveFile.version = VersionManager.GetVersion();
        saveFile.noteDatas = new List<string>();

        s_this.StartCoroutine(s_this.IWriteFile(saveFile, path));
    }
    public static void LoadNoteFile()
    {
        string path;
        path = "";

        VistaOpenFileDialog dialog;
        dialog = new VistaOpenFileDialog();
        dialog.Filter = "nd files|*.nd";
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

        path = path.Replace(".nd", String.Empty);
        path += ".nd";

        File.WriteAllText(path, jsonData);
        yield return null;
    }
    private IEnumerator IReadFile(string path)
    {
        //$ Check NoteField
        if (NoteHolder.s_holders.Count != 0)
        {
            isActive = false;
            isPassed = false;
            PopUpObjects[0].GetComponent<Animator>().SetTrigger("On");

            while (true)
            {
                if (isActive)
                {
                    PopUpObjects[0].GetComponent<Animator>().SetTrigger("Off");
                    if (!isPassed) { yield break; }
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
            saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));
        }
        catch { yield break; }

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
                    if (!isPassed) { yield break; }
                    break;
                }
                yield return null;
            }
        }

        ValueManager.s_Bpm = saveFile.bpm;
        ValueManager.s_Delay = saveFile.delay;
        ValueManager.UpdateInputField();
        foreach (LineHolder holder in LineHolder.s_holders) { holder.UpdateMs(); }

        GameManager.UpdateGameMode((GameMode)saveFile.gameMode + 4);

        for (int i = 0; i < saveFile.noteDatas.Count; i++)
        {
            NoteHolder holder;
            holder = new NoteHolder(saveFile.noteDatas[i]);
            holder.UpdateNote();
        }
        NoteClass.InitSpeedMs();
    
        ObjectCooling.UpdateCooling();
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
    public static Int32 StringToLength(string value)
    {
        if (value == "--") { return 0; }

        char[] cArr;
        cArr = value.ToCharArray();

        int ret;
        ret = (Convert.ToInt32(cArr[0]) - 65) * 10 + (int)Char.GetNumericValue(cArr[1]);
        if (ret < 1) { ret = 1; }
        return ret;
    }

    private static string HolderToData(NoteHolder holder)
    {
        string ret = "";
        
        return ret;
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

    public static SaveFile GetSaveFile(string dataPath, bool isCheckAvailable = false)
    {
        SaveFile ret;
        ret = JsonUtility.FromJson<SaveFile>(File.ReadAllText(dataPath));

        if (isCheckAvailable)
        {
            if (ret.delay == -1 || ret.gameMode == -1 || ret.bpm == -1 ) { ret = null; }
            else if (ret.maxBpm.Contains(-1f)) { ret = null; }
            else if (ret.version.Contains(-1)) { ret = null; }
            else if (ret.editDate.Contains(-1)) { ret = null; }
        }

        return ret;
    }
}

public class SaveFile
{
    public int delay = -1, gameMode = -1;
    public float bpm = -1f;
    public float[] maxBpm = {-1f, -1f};
    public int[] version = { -1, -1, -1 };
    public int[] editDate = { -1, -1, -1, -1, -1 };

    public List<string> noteDatas = new List<string>();
}
