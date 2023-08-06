using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using TMPro;
using Ookii.Dialogs;
using System.Windows.Forms;

public class MusicBox : MonoBehaviour
{
    private static MusicBox s_this;
    private bool isUp = false;
    public static AudioSource audioSource;
    [SerializeField] private TextMeshPro MusicFileName;
    [SerializeField] private UnityEngine.UI.Button loadBtn;
    [SerializeField] private Slider slider;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!Directory.Exists(UnityEngine.Application.dataPath + "/_DataBox/_MusicFile/"))
        {
            Directory.CreateDirectory(UnityEngine.Application.dataPath + "/_DataBox/_MusicFile/");
        }
    }
    public void OnLoadBtn()
    {
        string path;
        path = "";

        VistaOpenFileDialog dialog;
        dialog = new VistaOpenFileDialog();
        dialog.Filter = "mp3 files (*.mp3)|*.mp3|wav files (*.wav)|*.wav";
        dialog.FilterIndex = 2;
        dialog.Title = "Open Music File";
        dialog.InitialDirectory = Environment.CurrentDirectory + @"\Assets\_DataBox\_MusicFile\";
        dialog.RestoreDirectory = true;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            Stream stream;
            if ((stream = dialog.OpenFile()) != null)
            {
                stream.Close();
                path = dialog.FileName;
            }
        }
        else { return; }

        loadBtn.interactable = false;
        StartCoroutine(ILoadMusic(path));
    }
    public void OnVolumeChange()
    {
        audioSource.volume = slider.value / 10.0f;
    }
    public IEnumerator ILoadMusic(string path)
    {
        if (path == "") { yield break; }
        if (path.ToString().Length < 3) { yield break; }

        using (UnityWebRequest www = UnityWebRequestMultimedia
            .GetAudioClip(path, path.ToString().Substring(path.ToString().Length - 3) 
            == ".wav" ? AudioType.WAV : AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                audioSource.clip = null;
                loadBtn.interactable = true;
                yield break;
            }
            else
            {
                try
                {
                    audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                }
                catch { }
                loadBtn.interactable = true;
            }
        }
    }
}
