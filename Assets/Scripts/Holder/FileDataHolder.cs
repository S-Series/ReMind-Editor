using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileDataHolder : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] dataTmps;

    private FileSelector.NoteData noteData;

    public void ApplyDataFile(FileSelector.NoteData data)
    {
        noteData = data;
    }
    public void UpdateData()
    {
        var saveFile = noteData.NoteFileData;

        //$ Note Data File Name
        dataTmps[0].text = noteData.FileName;

        //$ Bpm
        dataTmps[1].text = 
            saveFile.maxBpm[0] == 0f && saveFile.maxBpm[1] == 0f ?
            string.Format("BPM || {0}", saveFile.bpm) :
            string.Format("BPM || {0} - {1}", saveFile.maxBpm[0], saveFile.maxBpm[1]);

        //$ GameMode
        dataTmps[2].text = 
            string.Format("Mode || Line{0}", (GameData.GameMode)saveFile.gameMode);

        //$ Last Edit Date & Time
        dataTmps[3].text = 
            string.Format("{0}.{1:D2}.{2:D2} {3:D2}:{4:D2}", 
            saveFile.editDate[0], saveFile.editDate[1], saveFile.editDate[2],
            saveFile.editDate[3], saveFile.editDate[4]);
    }

    public void OnDataSelected() //# Activate by Button Action
    {
        FileSelector.s_this.ApplyNoteFile(gameObject, noteData);
    }
}
