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
        string ret;
        ret = String.Format("{0:D8}#" +                             //# stdPos
            "{01:D4}|{02:D4}|{03:D4}|{04:D4}|{05:D4}|{06:D4}#" +    //# NormalNote
            "{07:D4}|{08:D4}|{09:D4}|{10:D4}|{11:D4}|{12:D4}#" +    //# AirialNote
            "{13}#{14}#{15}", holder.stdPos,                        //# ScratchNote(13, 14) + [SpeedNote + EffectNote](15)
            holder.normals[0] == null ? "----" : holder.normals[0].length,
            holder.normals[1] == null ? "----" : holder.normals[1].length,
            holder.normals[2] == null ? "----" : holder.normals[2].length,
            holder.normals[3] == null ? "----" : holder.normals[3].length,
            holder.normals[4] == null ? "----" : holder.normals[4].length,
            holder.normals[5] == null ? "----" : holder.normals[5].length,

            holder.airials[0] == null ? "----" : holder.airials[0].length,
            holder.airials[1] == null ? "----" : holder.airials[1].length,
            holder.airials[2] == null ? "----" : holder.airials[2].length,
            holder.airials[3] == null ? "----" : holder.airials[3].length,
            holder.airials[4] == null ? "----" : holder.airials[4].length,
            holder.airials[5] == null ? "----" : holder.airials[5].length,

            holder.bottoms[0] == null ? "-----|----|----|----" : String.Format(
                "{4}{0:D4}|{5}{1:D3}|{6}{2:D3}|{7}{3:D3}",
                Mathf.Abs(holder.bottoms[0].length),
                Mathf.Abs(holder.bottoms[0].startX),
                Mathf.Abs(holder.bottoms[0].powerX),
                Mathf.Abs(holder.bottoms[0].endX),
                holder.bottoms[0].isPower ? "+" : "-",
                holder.bottoms[0].startX > 0 ? "+" : "-",
                holder.bottoms[0].powerX > 0 ? "+" : "-",
                holder.bottoms[0].endX > 0 ? "+" : "-"
            ),
            holder.bottoms[1] == null ? "-----|----|----|----" : String.Format(
                "{4}{0:D4}|{5}{1:D3}|{6}{2:D3}|{7}{3:D3}",
                Mathf.Abs(holder.bottoms[1].length),
                Mathf.Abs(holder.bottoms[1].startX),
                Mathf.Abs(holder.bottoms[1].powerX),
                Mathf.Abs(holder.bottoms[1].endX),
                holder.bottoms[1].isPower ? "T" : "F",
                holder.bottoms[1].startX > 0 ? "+" : "-",
                holder.bottoms[1].powerX > 0 ? "+" : "-",
                holder.bottoms[1].endX > 0 ? "+" : "-"
            ),

            String.Format(
                "{0}#{1}",
                holder.speedNote == null ? "-----|-----" : String.Format(
                    "{0:D5}|{1:D5}",
                    Mathf.RoundToInt((Single)(holder.speedNote.bpm * 100d)),
                    Mathf.RoundToInt((Single)(holder.speedNote.multiple * 10000d))
                ),
                holder.effectNote == null ? "--|------" : String.Format(
                    "{0:D2}|{1:D6}",
                    holder.effectNote.effectIndex,
                    holder.effectNote.value
                )
            )
        ) ;
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
}

public class SaveFile
{
    public int delay = 0, gameMode = 0;
    public float bpm = 120.0f;
    public int[] version = { 1, 0, 0 };

    public List<string> noteDatas = new List<string>();
}
