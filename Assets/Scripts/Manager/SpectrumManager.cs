using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpectrumManager : MonoBehaviour
{
    private static SpectrumManager s_this;
    private static AudioSource audioSource;
    private static Transform GenerateField;
    private static GameObject SpectrumPrefab;
    [SerializeField] GameObject _SpectrumPrefab;
    [SerializeField] Transform SpectrumLoading;
    [SerializeField] TextMeshPro SpectrumLoadingText;

    public static List<SpectrumData> s_SpectrumDatas = new List<SpectrumData>();

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
        s_SpectrumDatas = new List<SpectrumData>();

        for (int i = 0; i < GenerateField.childCount; i++)
        {
            Destroy(GenerateField.GetChild(i).gameObject);
        }
        ObjectCooling.UpdateCooling(runOnly: 3);

        float length;
        length = audioSource.clip == null ? -1f : audioSource.clip.length;

        if (length == -1f) { return; }

        int indexer = 0;

        while (true)
        {
            float ms, posY;
            ms = indexer * 10;
            posY = NoteClass.MsToPos(ms);

            if (ms / 1000f > length) { break; } //$ End of while();

            GameObject @object;
            @object = Instantiate(SpectrumPrefab, GenerateField, false);
            @object.name = String.Format("{0} : {1}", posY, ms);

            s_SpectrumDatas.Add(new SpectrumData(ms, posY, @object));

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
    public static void UpdateSpectrumPos()
    {
        foreach (SpectrumData data in s_SpectrumDatas)
        {
            data.UpdatePosY(NoteClass.MsToPos(data.ms));
        }
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
        int count;
        count = s_SpectrumDatas.Count;
        GenerateDelays = new List<float>();
        maxScale = 0.0f;
        float[] getData;
        SpectrumData data;

        SpectrumLoading.parent.gameObject.SetActive(true);
        SpectrumLoading.localScale = new Vector3(0.0175f, 0, 1);
        SpectrumLoadingText.text = String.Format("{0} of {1}", 0, count);

        for (int i = 0; i < count;)
        {
            yield return null;

            if (AutoTest.s_isTesting) { SpectrumLoadingText.text = "Paused"; continue; }

            SpectrumLoading.localScale = new Vector3(0.0175f, 0.225f * i / count, 1);
            SpectrumLoadingText.text = String.Format("{0} of {1}", i, count);

            data = s_SpectrumDatas[i];
            getData = GetSpectrumData();
            audioSource.time = data.ms / 1000f;
            data.UpdateScale(getData);
            if (getData[0] > maxScale) { maxScale = getData[0]; }
            if (getData[1] > maxScale) { maxScale = getData[1]; }
            GenerateDelays.Add(Time.deltaTime * 1000);
            SpectrumTransforms[3] = GenerateDelays.Average();
            i++;
        }
        audioSource.Stop();
        isGenerating = false;

        SpectrumLoadingText.text = "Load Complete!";
        yield return new WaitForSeconds(3.0f); 
        SpectrumLoading.parent.gameObject.SetActive(false);
    }
}
