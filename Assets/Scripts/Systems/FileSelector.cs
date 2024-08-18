using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using TMPro;

public class FileSelector : MonoBehaviour
{
    public static FileSelector s_this;

    private static List<NoteData> NoteDataHolders;
    private static List<MusicData> MusicDataHolders;

    [SerializeField] GameObject[] _DataHolderPrefabs;
    private static GameObject[] DataHolderPrefabs;
    [SerializeField] Transform[] _ScrollViewContentFields;
    private static Transform[] ScrollViewContentFields;

    [SerializeField] TextMeshPro[] dataTmps;
    [SerializeField] Transform[] CopyDataField;
    [SerializeField] GameObject[] CopyDataCont;
    private static SaveFile lastNoteFile = null; 
    private static AudioClip lastAudioClip = null;

    private void Start()
    {
        s_this = this;
        DataHolderPrefabs = _DataHolderPrefabs;
        ScrollViewContentFields = _ScrollViewContentFields;
        //_DataHolderPrefabs = null;
        //_ScrollViewContentFields = null;
    }

    private async static void NoteFileLoader()
    {
        NoteDataHolders = new List<NoteData>();
        var TargetDirectory = new DirectoryInfo(Application.dataPath + @"\_DataBox\");

        var fileInfo = TargetDirectory.GetFiles("*.nd");
        foreach (FileInfo f in fileInfo)
        {
            NoteDataHolders.Add(new NoteData(f.Name, f.FullName));
        }

        for (int i = 0; i < NoteDataHolders.Count - 1; i++)
        {
            await Task.Run(() => DataToJson(i));
        }
    }
    private static IEnumerator MusicFileLoader()
    {
        AudioClip newClip;
        var TargetDirectory = new DirectoryInfo(Application.dataPath + @"\_DataBox\_MusicFile\");

        var fileInfo = TargetDirectory.GetFiles("*.mp3");
        foreach (FileInfo f in fileInfo)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia
                .GetAudioClip(f.FullName, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    newClip = null;
                }
                else { newClip = DownloadHandlerAudioClip.GetContent(www); }

                newClip.name = f.Name;
            }
            if (newClip != null)
            {
                MusicDataHolders.Add(
                    new MusicData(
                        newClip,
                        f.FullName,
                        Instantiate(DataHolderPrefabs[1], ScrollViewContentFields[1])
                    )
                );
            }
        }
    }
    
    private static void DataToJson(int index)
    {
        string path;
        SaveFile saveFile;
        path = NoteDataHolders[index].NoteFilePath;
        saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));
        NoteDataHolders[index].NoteFileData = saveFile;
    }

    public void ApplyNoteFile(GameObject copy, SaveFile saveFile, string Name)
    {
        if (lastNoteFile == saveFile) { CancelNoteFile(); return; }

        if (CopyDataCont[0] != null) { Destroy(CopyDataCont[0]); }
        CopyDataCont[0] = Instantiate(copy, CopyDataField[0], false);
        CopyDataCont[0].transform.localPosition = new Vector3(0, 0, 0);

        dataTmps[0].text = string.Format("# File Name\n{0}",Name);
        dataTmps[1].text = string.Format("{0}\n{1} - {2}\n{3}",
            saveFile.bpm,
            saveFile.maxBpm[0] == 0 ? "-" : saveFile.maxBpm[0],
            saveFile.maxBpm[1] == 0 ? "-" : saveFile.maxBpm[1],
            GameManager.GetGameModeName(saveFile.gameMode));
        dataTmps[2].text = string.Format("{0}.{1}.{2}\t{3}:{4}",
            saveFile.editDate[0], saveFile.editDate[1], saveFile.editDate[2],
            saveFile.editDate[3], saveFile.editDate[4]);

        lastNoteFile = saveFile;
    }
    public void ApplyMusicFile(GameObject copy, AudioClip audioClip)
    {
        if (lastAudioClip == audioClip) { CancelMusicFile(); return; }

        if (CopyDataCont[1] != null) { Destroy(CopyDataCont[1]); }
        CopyDataCont[1] = Instantiate(copy, CopyDataField[1], false);
        CopyDataCont[1].transform.localPosition = new Vector3(0, 0, 0);

        dataTmps[3].text = string.Format("{0}:{1}\n{2}Hz",
            Mathf.FloorToInt(audioClip.length / 60f), audioClip.length % 60, audioClip.frequency);
        dataTmps[4].text = string.Format("{0}\n{1} Ch.", audioClip.GetType(), audioClip.channels);

        lastAudioClip = audioClip;
    }

    public void CancelNoteFile()
    {

    }
    public void CancelMusicFile()
    {

    }

    public class NoteData
    {
        public string FileName { get; }
        public string NoteFilePath { get; }
        public SaveFile NoteFileData { get; set; }
        public NoteData(string name, string path)
        {
            FileName = name;
            NoteFilePath = path;
        }
    }
    private class MusicData
    {
        public AudioClip AudioClip { get; }
        public string AudioPath { get; }
        public GameObject AudioHolder { get; set; }
        public MusicData(AudioClip x, string y, GameObject z)
        {
            AudioClip = x;
            AudioPath = y;
            AudioHolder = z;
        }
    }
}
