using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class FileSelector : MonoBehaviour
{
    private List<NoteData> NoteDataHolders;
    private List<MusicData> MusicDataHolders;

    [SerializeField] GameObject[] DataHolderPrefabs;
    [SerializeField] Transform[] ScrollViewContentFields;

    private async void NoteFileLoader()
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
    private IEnumerator MusicFileLoader()
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
    
    private void DataToJson(int index)
    {
        string path;
        SaveFile saveFile;
        path = NoteDataHolders[index].NoteFilePath;
        saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));
        NoteDataHolders[index].NoteFileData = saveFile;
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
