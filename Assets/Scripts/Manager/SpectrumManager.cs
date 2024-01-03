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
    private static float[] SpectrumTransforms = new float[4];
    private static List<float> GenerateDelays;
    private static float maxScale;

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
        if (@clip != null) { audioSource.clip = @clip; }
        if (audioSource.clip == null) { return; }
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
            @object.name = String.Format("{0} : {1}", posY, ms);
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
    private float[] GetSpectrumData()
    {
        float[] ret, data;
        ret = new float[2];
        data = new float[1024];
        audioSource.GetSpectrumData(data, 0, FFTWindow.Rectangular);
        ret[0] = data.Max() * 1024;
        ret[1] = data.Average() * 1024 * 10;
        return ret;
    }
    public static void UpdateMusicDelay(int? DelayMs = null)
    {
        if (DelayMs.HasValue) { ValueManager.s_Delay = DelayMs.Value; }
        SpectrumTransforms[0] = ValueManager.s_Bpm * ValueManager.s_Delay / 150f;
        UpdateField();
    }
    public static void UpdatePosY(float PosY)
    {
        SpectrumTransforms[1] = PosY;
        UpdateField();
    }
    public static void UpdateScale(float ScaleY)
    {
        SpectrumTransforms[2] = ScaleY;
        UpdateField();
    }
    private static void UpdateField()
    {
        float posY;
        posY = SpectrumTransforms[1] 
            - SpectrumTransforms[0] * SpectrumTransforms[2]
            - ValueManager.s_Bpm * SpectrumTransforms[3] / 150;
        GenerateField.localScale = new Vector3(
            maxScale == 0 ? 1f : 1f / maxScale, SpectrumTransforms[2], 1);
        GenerateField.localPosition = new Vector3(0, posY * 2f / NoteField.s_Zoom, 0);
    }
    private IEnumerator IGenerating()
    {
        print("Started");
        GenerateDelays = new List<float>();
        maxScale = 0.0f;
        float[] getData;
        SpectrumData data;
        for (int i = 0; i < spectrumDatas.Count; i++)
        {
            yield return null;

            data = spectrumDatas[i];
            getData = GetSpectrumData();
            audioSource.time = data.ms / 1000f;
            data.SpectrumObject.transform.GetChild(0)
                .localScale = new Vector3(getData[0], .5f, 1);
            audioSource.time = data.ms / 1000f;
            data.SpectrumObject.transform.GetChild(1)
                .localScale = new Vector3(getData[1], 1, 1);
            if (getData[0] > maxScale) { maxScale = getData[0]; }
            if (getData[1] > maxScale) { maxScale = getData[1]; }
            GenerateDelays.Add(Time.deltaTime * 1000);
            SpectrumTransforms[3] = GenerateDelays.Average();
        }
        audioSource.Stop();
        isGenerating = false;
    }
}
