using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] AudioSource _MainMusic;
    private static AudioSource MainMusic;
    [SerializeField] AudioSource[] _SystemAudios;
    private static AudioSource[] SystemAudios;
    [SerializeField] AudioSource[] _FxAudios;
    private static AudioSource[] FxAudios;
    [SerializeField] AudioSource[] _GameAudios;
    private static AudioSource[] GameAudios;

    private void Start()
    {
        MainMusic = _MainMusic;
        SystemAudios = _SystemAudios;
        FxAudios = _FxAudios;
        GameAudios = _GameAudios;
    }

    /// 
    public static void PlaySound(int typeIndex, int index)
    {
        AudioSource source;
        if (typeIndex == 1) { source = SystemAudios[index]; }
        else if (typeIndex == 2) { source = FxAudios[index]; }
        else if (typeIndex == 3) { source = GameAudios[index]; }
        else { source = MainMusic; }

        if (source.isPlaying) { source.Stop(); }
        else { source.Play(); }
    }

    public void MasterVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }
    public void MusicVolume(Slider slider)
    {
        MainMusic.volume = slider.value;
    }
    public void FxVolume(Slider slider)
    {
        foreach(AudioSource audioSource in FxAudios)
        {
            audioSource.volume = slider.value;
        }
    }
    public void SystemVolume(Slider slider)
    {
        foreach(AudioSource audioSource in SystemAudios)
        {
            audioSource.volume = slider.value;
        }
    }
}
