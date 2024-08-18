using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileDataHolder : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] dataTmps;

    private string fileName;
    public SaveFile dataFile;

    public void ApplyDataFile(SaveFile saveFile, string Name)
    {
        //$ Note Data File Name
        fileName = Name;
        dataTmps[0].text = Name;

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
        
        dataFile = saveFile;
    }

    public void OnDataSelected() //# Activate by Button Action
    {
        FileSelector.s_this.ApplyNoteFile(gameObject, dataFile, fileName);
    }
}
