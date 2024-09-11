using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using TMPro;
using System;

public class FileSelector : MonoBehaviour
{
    public static FileSelector s_this;

    private static List<NoteData> NoteDataHolders;
    private static List<MusicData> MusicDataHolders;

    [SerializeField] GameObject[] _DataHolderPrefabs;
    private static GameObject[] DataHolderPrefabs;
    [SerializeField] RectTransform[] _ScrollViewContentFields;
    private static RectTransform[] ScrollViewContentFields;

    [SerializeField] TextMeshPro[] dataTmps;
    [SerializeField] Transform[] CopyDataField;
    [SerializeField] GameObject[] CopyDataCont;
    private static NoteData lastNoteData = null; 
    private static MusicData lastMusicData = null;

    private static IEnumerator MusicLoadCoroutine;

    private void Start()
    {
        s_this = this;
        DataHolderPrefabs = _DataHolderPrefabs;
        ScrollViewContentFields = _ScrollViewContentFields;
        //_DataHolderPrefabs = null;
        //_ScrollViewContentFields = null;
        NoteDataHolders = new List<NoteData>();
        MusicDataHolders = new List<MusicData>();
        MusicLoadCoroutine = MusicFileLoader();
        NoteFileLoader();
        StartCoroutine(MusicLoadCoroutine);
    }

    private static void NoteFileLoader()
    {
        NoteDataHolders = new List<NoteData>();
        var TargetDirectory = new DirectoryInfo(Application.dataPath + @"\_DataBox\");

        int copyCount = 0;
        GameObject copy;
        var fileInfo = TargetDirectory.GetFiles("*.nd");

        SaveFile saveFile;
        foreach (FileInfo file in fileInfo)
        {
            saveFile = SaveManager.GetSaveFile(file.FullName, isCheckAvailable: true);
            if (saveFile == null) { continue; }

            copy = Instantiate(DataHolderPrefabs[0], ScrollViewContentFields[0]);

            NoteData noteData;
            noteData = new NoteData(file.Name, file.FullName, copy, saveFile);
            NoteDataHolders.Add(noteData);

            copy.GetComponent<FileDataHolder>().ApplyDataFile(noteData);
            copy.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, copyCount * -100 - 60, 0);
            ScrollViewContentFields[0].sizeDelta = new Vector2(0, 100.25f * (copyCount + 1) + 14);
            copyCount++;
        }
    }
    private static IEnumerator MusicFileLoader()
    {
        AudioClip newClip;
        var TargetDirectory = new DirectoryInfo(Application.dataPath + @"\_DataBox\_MusicFile\");

        int copyCount = 0;

        var fileInfo = TargetDirectory.GetFiles("*.mp3");
        foreach (FileInfo f in fileInfo)
        {
            GameObject CopyObject;
            using (UnityWebRequest www = UnityWebRequestMultimedia
                .GetAudioClip(f.FullName, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    newClip = null;
                }
                else { newClip = DownloadHandlerAudioClip.GetContent(www); }

            }
            if (newClip != null)
            {
                newClip.name = f.Name.Split('.')[0];
                CopyObject = Instantiate(DataHolderPrefabs[1], ScrollViewContentFields[1]);

                MusicData musicData;
                musicData = new MusicData(newClip, f.FullName, CopyObject, false);

                MusicDataHolders.Add(musicData);
                CopyObject.GetComponent<MusicDataHolder>().ApplyMusicData(musicData);
                CopyObject.GetComponent<RectTransform>().
                    anchoredPosition = new Vector3(0, copyCount * -100 - 60, 0);

                ScrollViewContentFields[1].sizeDelta = new Vector2(0, 100.25f * (copyCount + 1) + 14);
                copyCount++;
            }
        }

        var fileInfo_ = TargetDirectory.GetFiles("*.wav");
        foreach (FileInfo f in fileInfo_)
        {
            GameObject CopyObject;
            CopyObject = Instantiate(DataHolderPrefabs[1], ScrollViewContentFields[1]);
            using (UnityWebRequest www = UnityWebRequestMultimedia
                .GetAudioClip(f.FullName, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    newClip = null;
                }
                else { newClip = DownloadHandlerAudioClip.GetContent(www); }
            }
            if (newClip != null)
            {
                newClip.name = f.Name.Split('.')[0];

                MusicData musicData;
                musicData = new MusicData(newClip, f.FullName, CopyObject, true);

                MusicDataHolders.Add(musicData);
                CopyObject.GetComponent<MusicDataHolder>().ApplyMusicData(musicData);
                CopyObject.GetComponent<RectTransform>().
                    anchoredPosition = new Vector3(0, copyCount * -100 - 60, 0);
                ScrollViewContentFields[1].sizeDelta = new Vector2(0, 100.25f * (copyCount + 1) + 14);
                copyCount++;
            }
        }
    }
    
    private static void DataToJson(int index)
    {
        var data = NoteDataHolders[index];

        string path;
        path = data.NoteFilePath;

        SaveFile saveFile;
        saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));

        data.NoteFileData = saveFile;
        data.DataHolder.GetComponent<FileDataHolder>().ApplyDataFile(data);
    }

    public void ApplyNoteFile(GameObject copy, NoteData noteData)
    {
        if (lastNoteData == noteData) { CancelNoteFile(); return; }

        if (CopyDataCont[0] != null) { Destroy(CopyDataCont[0]); }
        CopyDataCont[0] = Instantiate(copy, CopyDataField[0], false);
        CopyDataCont[0].transform.localPosition = new Vector3(0, 0, 0);

        var saveFile = noteData.NoteFileData;

        dataTmps[0].text = string.Format("# File Name\n{0}",noteData.FileName);
        dataTmps[1].text = string.Format("{0}\n{1} - {2}\n{3}",
            saveFile.bpm,
            saveFile.maxBpm[0] == 0 ? "-" : saveFile.maxBpm[0],
            saveFile.maxBpm[1] == 0 ? "-" : saveFile.maxBpm[1],
            GameManager.GetGameModeName(saveFile.gameMode));
        dataTmps[2].text = string.Format("{0}.{1}.{2}\t{3}:{4}",
            saveFile.editDate[0], saveFile.editDate[1], saveFile.editDate[2],
            saveFile.editDate[3], saveFile.editDate[4]);

        lastNoteData = noteData;
    }
    //public void ApplyMusicFile(GameObject copy, AudioClip audioClip, bool isWav)
    public void ApplyMusicFile(GameObject copy, MusicData musicData)
    {
        if (lastMusicData == musicData) { CancelMusicFile(); return; }

        if (CopyDataCont[1] != null) { Destroy(CopyDataCont[1]); }
        CopyDataCont[1] = Instantiate(copy, CopyDataField[1], false);
        CopyDataCont[1].transform.localPosition = new Vector3(0, 0, 0);

        var audioClip = musicData.AudioClip;
        int length = Mathf.FloorToInt(audioClip.length);

        dataTmps[3].text = string.Format("{0}:{1:D2}\n<size=2>{2}Hz",
            Mathf.FloorToInt(length / 60f), length % 60, audioClip.frequency);
        dataTmps[4].text = string.Format("{0}\n{1} Ch.", 
            musicData.isWav ? "Wav" : "Mp3", audioClip.channels);

        lastMusicData = musicData;
    }

    public void CancelNoteFile()
    {
        lastNoteData = null;
        if (CopyDataCont[0] != null) { Destroy(CopyDataCont[0]); }

        dataTmps[0].text = "# File Name\n- - - - - - - - - - - - - - - -";
        dataTmps[1].text = "- - -\n0 - 999\nNaN";
        dataTmps[2].text = "# Last Edit Date\n- - - -.- -.- -\t-- : --";
    }
    public void CancelMusicFile()
    {
        lastMusicData = null;
        if (CopyDataCont[1] != null) { Destroy(CopyDataCont[1]); }
        
        dataTmps[3].text = "-- : --\n<size=2>- - - - Hz";
        dataTmps[4].text = "Nan\n0 Ch.";
    }

    public void CreateNewFile()
    {

    }
    public void OpenFolder(bool isNoteFile)
    {
        Application.OpenURL( Application.dataPath + (isNoteFile ? @"\_DataBox\" : @"\_DataBox\_MusicFile\"));
    }
    public void ReloadFiles(bool isNoteFile)
    {
        if (isNoteFile)
        {
            int count;
            count = NoteDataHolders.Count;
            for (int i = 0; i < count; i++)
            {
                Destroy(NoteDataHolders[i].DataHolder);
            }
            NoteDataHolders = new List<NoteData>();
            NoteFileLoader();
        }
        else
        {
            StopCoroutine(MusicLoadCoroutine);
            
            int count;
            count = MusicDataHolders.Count;
            for (int i = 0; i < count; i++)
            {
                Destroy(MusicDataHolders[i].AudioHolder);
            }
            MusicDataHolders = new List<MusicData>();

            MusicLoadCoroutine = MusicFileLoader();
            StartCoroutine(MusicLoadCoroutine);
        }
    }
    public class NoteData
    {
        public string FileName { get; }
        public string NoteFilePath { get; }
        public SaveFile NoteFileData { get; set; }
        public GameObject DataHolder { get; set; }
        public NoteData(string name, string path, GameObject @object)
        {
            FileName = name;
            NoteFilePath = path;
            DataHolder = @object;
        }
        public NoteData(string name, string path, GameObject @object, SaveFile file)
        {
            FileName = name;
            NoteFilePath = path;
            DataHolder = @object;
            NoteFileData = file;
        }
    }
    public class MusicData
    {
        public AudioClip AudioClip { get; }
        public string AudioPath { get; }
        public bool isWav { get; }
        public GameObject AudioHolder { get; set; }
        public MusicData(AudioClip x, string y, GameObject z, bool w)
        {
            AudioClip = x;
            AudioPath = y;
            AudioHolder = z;
            isWav = w;
        }
    }
}
