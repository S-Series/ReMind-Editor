using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using TMPro;

public class MusicBox : MonoBehaviour, IPointerClickHandler
{
    private static MusicBox s_this;
    private bool isUp = false;
    public static AudioSource audioSource;
    [SerializeField] Toggle isWav;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button loadBtn;
    [SerializeField] Slider slider;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!Directory.Exists(Application.dataPath + "/_DataBox/_MusicFile/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/_DataBox/_MusicFile/");
        }
        else { inputField.text = PlayerPrefs.GetString("musicName"); StartCoroutine(ILoadMusic()); }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isUp) { GetComponent<Animator>().SetTrigger("PlayDown"); }
        else { GetComponent<Animator>().SetTrigger("PlayUp"); }
        isUp = !isUp;
    }
    public void OnLoadBtn()
    {
        isWav.interactable = false;
        inputField.interactable = false;
        loadBtn.interactable = false;
        StartCoroutine(ILoadMusic());
    }
    public void OnVolumeChange()
    {
        audioSource.volume = slider.value / 10.0f;
    }
    public IEnumerator ILoadMusic()
    {
        string path = "";
        string name = inputField.text;

        if (name == "")
        {
            isWav.interactable = true;
            inputField.interactable = true;
            loadBtn.interactable = true;
            yield break;
        }
        if (isWav.isOn) { path = Application.dataPath + "/_DataBox/_MusicFile/" + name + ".wav"; }
        else { path = Application.dataPath + "/_DataBox/_MusicFile/" + name + ".mp3"; }

        print(path);

        using (UnityWebRequest www = UnityWebRequestMultimedia
            .GetAudioClip(path, isWav.isOn ? AudioType.WAV : AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                audioSource.clip = null;
                PlayerPrefs.SetString("SongName", "");
                isWav.interactable = true;
                inputField.interactable = true;
                loadBtn.interactable = true;
                yield break;
            }
            else
            {
                try
                {
                    audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                    PlayerPrefs.SetString("SongName", name);
                }
                catch { }
                isWav.interactable = true;
                inputField.interactable = true;
                loadBtn.interactable = true;
            }
        }
    }
}
