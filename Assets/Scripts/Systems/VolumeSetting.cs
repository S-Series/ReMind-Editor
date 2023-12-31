using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioSource[] Audios;
    [SerializeField] private AudioSource[] BuildAudios;
    [SerializeField] private AudioSource[] BuildJudgeAudios;
    [SerializeField] private AudioSource[] GameAudios;

    void Start()
    {
        //$ Music
        Audios = new AudioSource[]
        {
            MusicLoader.audioSource,
        };
        //$ Fx
        BuildJudgeAudios = AutoTest.GetJudgeAudioSource(); 
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
        
    }
    public void EffectVolume(Slider slider)
    {
        foreach(AudioSource audioSource in BuildJudgeAudios)
        {
            audioSource.volume = slider.value;
        }
    }
}
