using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FileSelector : MonoBehaviour
{
    private List<NoteData> NoteDataHolders;
    private List<MusicData> MusicDataHolders;

    [SerializeField] GameObject[] DataHolderPrefabs;
    [SerializeField] Transform[] ScrollViewContentFields;

    private void Start()
    {
        
    }

    private IEnumerator NoteFileLoader()
    {
        TextAsset textAsset;
        var TargetDirectory = new DirectoryInfo(Application.dataPath + @"\_DataBox\");

        var fileInfo = TargetDirectory.GetFiles("*.nd");
        foreach (FileInfo f in fileInfo)
        {
            //textAsset = File.ReadAllText(f.FullName);
        }
        yield return null;
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

    private struct MusicData
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
    private struct NoteData
    {
        public string NoteFilePath { get; }
        public string NoteFileData { get; set; }
        public NoteData(string path)
        {
            NoteFilePath = path;
            NoteFileData = string.Empty;
        }
    }
}
