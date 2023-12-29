using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameNote;
using UnityEngine;

public class SpectrumManager : MonoBehaviour
{
    private static SpectrumManager s_this;
    public struct SpectrumData
    {
        public float ms { get; }
        public float posY { get; }
        public GameObject SpectrumObject { get; }
        public SpectrumData(float x, float y, GameObject z)
        {
            ms = x;
            posY = y;
            SpectrumObject = z;
        }
    }
    private static AudioSource audioSource;
    private static Transform GenerateField;
    private static GameObject SpectrumPrefab;
    [SerializeField] GameObject _SpectrumPrefab;

    public static List<SpectrumData> spectrumDatas;

    public static bool isGenerating = false;

    private void Awake()
    {
        s_this = this;
        GenerateField = transform.GetChild(0);
        SpectrumPrefab = _SpectrumPrefab;
        audioSource = GetComponent<AudioSource>();
    }
    public static void GenerateSpectrum(AudioClip @clip)
    {
        s_this.StopAllCoroutines();
        audioSource.clip = @clip;
        audioSource.Play();
        spectrumDatas = new List<SpectrumData>();

        for (int i = 0; i < GenerateField.childCount; i++)
        {
            Destroy(GenerateField.GetChild(i).gameObject);
        }

        float length;
        length = audioSource.clip == null ? -1f : audioSource.clip.length;

        if (length == -1f) { return; }

        int indexer = 0;

        while (true)
        {
            int posY;
            posY = indexer * 16;

            float ms;
            ms = NoteClass.CalMs(posY);

            if (ms / 1000f > length) { break; } //$ End of while();

            GameObject @object;
            @object = Instantiate(SpectrumPrefab, GenerateField, false);
            @object.name = String.Format("Pos : {0}", posY);
            @object.transform.localPosition = new Vector3(0, posY, 0);

            spectrumDatas.Add(new SpectrumData(ms, posY, @object));

            indexer++;
        }
        s_this.StartGenerateCoroutine();
    }
    private void StartGenerateCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(IGenerating());
    }
    private float GetSpectrumData(bool isMax)
    {
        float[] data;
        data = new float[256];
        audioSource.GetSpectrumData(data, 0, FFTWindow.Rectangular);
        return isMax ? data.Max() * 1000 : data.Average() * 10000;
    }
    private IEnumerator IGenerating()
    {
        SpectrumData data;
        for (int i = 0; i < spectrumDatas.Count; i++)
        {
            yield return null;

            data = spectrumDatas[i];
            audioSource.time = data.ms / 1000f;
            data.SpectrumObject.transform.GetChild(0)
                .localScale = new Vector3(GetSpectrumData(true), .5f, 1);
            data.SpectrumObject.transform.GetChild(1)
                .localScale = new Vector3(GetSpectrumData(false), 1, 1);
        }
        audioSource.Stop();
        isGenerating = false;
    }
}
