using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    AudioSource[] Audios;
    AudioSource[] BuildAudios;
    AudioSource[] GameAudios;

    void Start()
    {
        //$ Music
        Audios[0] = MusicLoader.audioSource;
        //$ Fx
        //BuildAudios[0] = ;
        //$ Effect
        //BuildAudios[1] = ;
    }

    public void MasterVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }
    public void MusicVolume(Slider slider)
    {
        Audios[0].volume = slider.value;
    }
    public void FxVolume(Slider slider)
    {
        BuildAudios[1].volume = slider.value;
    }
    public void EffectVolume(Slider slider)
    {
        BuildAudios[2].volume = slider.value;
    }
}
