using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileDataHolder : MonoBehaviour
{
    public bool isCopyObject = false;
    [SerializeField] private TextMeshPro[] dataTmps;

    private FileSelector.NoteData noteData;

    public void ApplyDataFile(FileSelector.NoteData data)
    {
        noteData = data;
        UpdateData();
    }
    public void UpdateData()
    {
        var saveFile = noteData.NoteFileData;

        //$ Note Data File Name
        dataTmps[0].text = noteData.FileName.Replace(".nd", string.Empty);

        //$ Bpm
        dataTmps[1].text = 
            saveFile.maxBpm[0] == 0f && saveFile.maxBpm[1] == 0f ?
            string.Format("BPM || {0}", saveFile.bpm) :
            string.Format("BPM || {0} - {1}", saveFile.maxBpm[0], saveFile.maxBpm[1]);

        //$ GameMode
        dataTmps[2].text = 
            string.Format("Mode || {0}", (GameData.GameMode)saveFile.gameMode);

        //$ Last Edit Date & Time
        dataTmps[3].text = saveFile.editDate;
    }

    public void OnDataSelected() //# Activate by Button Action
    {
        if (isCopyObject) { FileSelector.CancelNoteFile(); }
        else { FileSelector.s_this.ApplyNoteFile(gameObject, noteData);}
    }
}
