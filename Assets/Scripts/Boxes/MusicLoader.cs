using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class MusicLoader : MonoBehaviour
{
    private static MusicLoader s_this;
    private bool isUp = false;
    public static AudioSource audioSource;
    public List<AudioClip> clips = new List<AudioClip>();
    private static string path;

    [SerializeField] TMP_Dropdown dropdown;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!Directory.Exists(Environment.CurrentDirectory + @"\Assets\_DataBox\_MusicFile\"))
        {
            Directory.CreateDirectory(Environment.CurrentDirectory + @"\Assets\_DataBox\_MusicFile\");
        }
        path = Environment.CurrentDirectory + @"\Assets\_DataBox\_MusicFile\";
        StartCoroutine(ILoadAllFile(true));
    }
    private IEnumerator ILoadAllFile(bool isStartLoad = false)
    {
        clips = new List<AudioClip>();

        AudioClip newClip;
        var info = new DirectoryInfo(path);

        var fileInfo = info.GetFiles("*.mp3");
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
            if (newClip != null) { clips.Add(newClip); }
        }

        fileInfo = info.GetFiles("*.wav");
        foreach (FileInfo f in fileInfo)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia
                .GetAudioClip(f.FullName, AudioType.WAV))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    newClip = null;
                }
                else { newClip = DownloadHandlerAudioClip.GetContent(www); }

                newClip.name = f.Name;
            }
            if (newClip != null) { clips.Add(newClip); }
        }

        UpdateDropdown(isStartLoad);
    }
    private void UpdateDropdown(bool isStartLoad = false)
    {
        List<TMP_Dropdown.OptionData> Options 
            = new List<TMP_Dropdown.OptionData>();

        Options.Add(new TMP_Dropdown.OptionData("None"));

        for (int i = 0; i < clips.Count; i++)
        {
            Options.Add(new TMP_Dropdown.OptionData(clips[i].name));
        }

        dropdown.options = Options;

        if (isStartLoad) 
        { 
            int _value;
            _value = Options.FindIndex(item => item.text == PlayerPrefs.GetString("MusicValue"));
            dropdown.value = _value == -1 ? 0 : _value;
        }
        else { dropdown.value = 0; }
    }

    public void OnDropdownValueChanged()
    {
        PlayerPrefs.SetString("MusicValue", dropdown.options[dropdown.value].text);
        audioSource.clip = dropdown.value == 0 ? null : clips[dropdown.value - 1];
    }
}
