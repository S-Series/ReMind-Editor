using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class SaveLoad : MonoBehaviour
{
    void Awake()
    {
        SaveToJson("Test");
    }
    private const string c_path = "";

    //$ Save NoteFile To Json
    public void SaveToJson(string FileName)
    {
        JsonNoteFile jsonFile = new JsonNoteFile();

        #region Convert Notes To Json File
        
        #endregion

        string jsonData;
        jsonData = JsonUtility.ToJson(jsonFile, true);

        string path = Application.dataPath + "/_DataBox/" + FileName + ".json";

        File.WriteAllText(path, jsonData);
        PlayerPrefs.SetString("NoteFileName", FileName);
    }

    //$ Load NoteFile From Json
    public void LoadFromJson(string FileName)
    {
        
    }
}

public class JsonNoteFile
{

}