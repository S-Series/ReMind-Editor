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
        saveFile = new SaveFile((int)GameManager.gameMode);

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
        List<int> values;
        string ret;
        values = ConvertBase(value, from: 10, to: 26);

        if (values.Count < 1 || values.Count > 4) { return "ZZZZ"; }

        while (values.Count > 3) { values.Insert(0, 0); }

        ret = String.Format("{0}{1}{2}{3}",
            values[0] == 0 ? '0' : (char)(values[0] + 65),
            values[1] == 0 ? '0' : (char)(values[1] + 65),
            values[2] == 0 ? '0' : (char)(values[2] + 65),
            values[3] == 0 ? '0' : (char)(values[3] + 65)
        );
        return ret;
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
    //$ Convert input Number from Base N to Base
    private static List<int> ConvertBase(int input, int from, int to)
    {
        List<int> ret = new List<int>();

        int passNum = 0;
        for (int index = 0; true; index++)
        {
            passNum += Mathf.FloorToInt(input % Mathf.Pow(10, index + 1) 
                / Mathf.Pow(10, index)) * Mathf.FloorToInt(Mathf.Pow(from, index));
            if (MathF.Pow(10 , index) >= input) { break;}
        }

        print(passNum);

        for (int index = 0; true; index++)
        {
            if (Mathf.Pow(to, index) >= passNum) { break; }
            ret.Add(Mathf.FloorToInt(passNum / Mathf.Pow(to, index)) % to);
        }
        ret.Reverse();
        return ret;
    }

    private static string HolderToData(NoteHolder holder)
    {
        /* Note Information
        Holder = [Normal Note * 6] + [Airial Note * 6] + [Shift Note * 2] + [Bpm Note] + [Effect Note]
        000000#
        AAAA|AAAA|AAAA|AAAA|AAAA|AAAA#
        AAAA|AAAA|AAAA|AAAA|AAAA|AAAA#
        AAAA|AAAA%AAA|AAA%0|1#
        00000|000#
        00000|01010101010101|"effectName",
        */

        string ret = String.Empty;
        var effect = holder.effectNote;
        
        ret = String.Format("{0:d6}#{1}#{2}#{3}#{4}#{5}",
            holder.stdMs,
            String.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                LengthToString(holder.normals[0].length),
                LengthToString(holder.normals[1].length),
                LengthToString(holder.normals[2].length),
                LengthToString(holder.normals[3].length),
                LengthToString(holder.normals[4].length),
                LengthToString(holder.normals[5].length)
            ),
            String.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                LengthToString(holder.airials[0].length),
                LengthToString(holder.airials[1].length),
                LengthToString(holder.airials[2].length),
                LengthToString(holder.airials[3].length),
                LengthToString(holder.airials[4].length),
                LengthToString(holder.airials[5].length)
            ),
            String.Format("{0}|{1}%{2}|{3}%{4}|{5}",
                LengthToString(holder.floors[0].length),
                LengthToString(holder.floors[1].length),
                holder.floors[0].value,
                holder.floors[1].value,
                holder.floors[0].isPowered ? 1 : 0,
                holder.floors[1].isPowered ? 1 : 0
            ),
            String.Format("{0:d5}|{1:d3}",
                Mathf.FloorToInt((float)holder.speedNote.bpm * 100),
                Mathf.FloorToInt((float)holder.speedNote.multiple * 100)
            ),
            String.Format("{0:d5}|{1}|{2}",
                holder.effectNote.value,
                String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}",
                    effect.isEffected[00] ? 0 : 1, effect.isEffected[01] ? 0 : 1,
                    effect.isEffected[02] ? 0 : 1, effect.isEffected[03] ? 0 : 1,
                    effect.isEffected[04] ? 0 : 1, effect.isEffected[05] ? 0 : 1,
                    effect.isEffected[06] ? 0 : 1, effect.isEffected[07] ? 0 : 1,
                    effect.isEffected[08] ? 0 : 1, effect.isEffected[09] ? 0 : 1,
                    effect.isEffected[10] ? 0 : 1, effect.isEffected[11] ? 0 : 1,
                    effect.isEffected[12] ? 0 : 1, effect.isEffected[13] ? 0 : 1
                ),
                holder.effectNote.effectName
            )
        );
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
            try
            {
                if (ret.delay == -1 || ret.gameMode == -1 || ret.bpm == -1) { ret = null; }
                else if (ret.maxBpm.Contains(-1f)) { ret = null; }
                else if (ret.version.Contains(-1)) { ret = null; }
            }
            catch { ret = null; }
        }

        return ret;
    }

    [UnityEngine.ContextMenu("Generate")]
    public void a()
    {
        CreateNewFile(4);
    }
    public static bool CreateNewFile(int gameMode = 4)
    {
        bool isCreated;
        isCreated = false;

        string path = "";
        string dialogPath = (UnityEngine.Application.dataPath + "\\_DataBox").Replace("/", "\\");

        VistaSaveFileDialog dialog;
        dialog = new VistaSaveFileDialog();
        dialog.Filter = "ND Files|*.nd";
        dialog.FilterIndex = 1;
        dialog.Title = "Save Data";
        dialog.InitialDirectory = dialogPath;
        dialog.RestoreDirectory = true;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            Stream stream;
            if ((stream = dialog.OpenFile()) != null)
            {
                path = dialog.FileName;
                print(path);
                stream.Close();
                /*if (File.Exists(path))
                {
                    while (path.Contains(".nd"))
                    {
                        string data;
                        data = File.ReadAllText(path);
                        File.Delete(path);
                        if (path == ".nd") { path = "_"; }
                        else { path = path.Substring(0, path.Length - 3); }
                        File.WriteAllText(path, data);
                    }
                    File.Move(path, path + ".nd");
                }*/
                
                SaveFile file;
                file = new SaveFile(gameMode);
                File.WriteAllText(path, JsonUtility.ToJson(file, true));
            }
            else { print("Open Failed"); return false; }
        }
        else { print("Select Failed"); return false; }

        return isCreated;
    }
}

public class SaveFile
{
    public int delay = -1, gameMode = -1;
    public float bpm = -1f;
    public float[] maxBpm = {-1f, -1f};
    public int[] version = { -1, -1, -1 };
    public string editDate = String.Empty;

    public List<string> noteDatas = new List<string>();
    public SaveFile() { }
    public SaveFile(int modeInt)
    {
        delay = 0;
        gameMode = modeInt;
        bpm = 120.0f;
        maxBpm = new float[] { 0f, 0f };
        version = VersionManager.GetVersion();
        editDate = DateTime.Now.ToString("yyyy-MM-dd\tHH:mm");
    }
    public SaveFile(int[] data, float[] bpms)
    {
        delay = data[0];
        gameMode = data[1];
        bpm = bpms[0];
        maxBpm = new float[] { bpms[1], bpms[2] };
        version = VersionManager.GetVersion();
        editDate = DateTime.Now.ToString("yyyy-MM-dd\tHH:mm");
    }
}
