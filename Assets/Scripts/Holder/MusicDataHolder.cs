using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicDataHolder : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] dataTmps;

    public AudioClip musicFile;

    public void ApplyMusicData(AudioClip audioClip)
    {
        musicFile = audioClip;

        dataTmps[0].text = audioClip.name;
        dataTmps[1].text = string.Format("{0} channels", audioClip.channels);
        dataTmps[2].text = string.Format("Type | {0}", audioClip.GetType());
        dataTmps[3].text = string.Format("{0} HertZ", audioClip.frequency);
        dataTmps[4].text = string.Format("Time | {0:D2}:{1:D2}", audioClip.length / 60, audioClip.length % 60);
    }

    public void OnDataSelected() //# Activate by Button Action
    {

    }
}
