using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicDataHolder : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] dataTmps;

    private FileSelector.MusicData musicData;

    public void ApplyMusicData(FileSelector.MusicData data)
    {
        musicData = data;

        UpdateMusicData();
    }
    public void UpdateMusicData()
    {
        var musicClip = musicData.AudioClip;

        dataTmps[0].text = musicClip.name;
        dataTmps[1].text = string.Format("Time | {0:D2}:{1:D2}", 
            Mathf.FloorToInt(Mathf.FloorToInt(musicClip.length) / 60f),
            Mathf.FloorToInt(musicClip.length) % 60);
        dataTmps[2].text = string.Format("Type | {0}", musicData.isWav ? "Wav" : "Mp3");
        dataTmps[3].text = string.Format("{0} Hz", musicClip.frequency);
        dataTmps[4].text = string.Format("{0} Channels", musicClip.channels);
    }

    public void OnDataSelected() //# Activate by Button Action
    {
        FileSelector.s_this.ApplyMusicFile(this.gameObject, musicData);
    }
}
