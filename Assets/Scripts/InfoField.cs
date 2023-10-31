using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameNote;

public class InfoField : MonoBehaviour
{
    private static InfoField s_this;
    //# Sector 01
    [SerializeField] private SpriteRenderer[] renderer_01;
    //# Sector 02
    [SerializeField] private Slider[] slider_02;
    [SerializeField] private TextMeshPro[] tmp_02;
    //# Sector 03
    [SerializeField] private Slider[] slider_03;
    [SerializeField] private TextMeshPro[] tmp_03;
    //# Sector 04

    private void Awake() { s_this = this; }
    private void Start() { UpdateInfoField(); }

    public static void UpdateInfoField() { s_this.UpdateField(); }
    private void UpdateField()
    {
        NoteHolder holder;

        int noteCount = 0;
        int[] normalCount = new int[4] { 0, 0, 0, 0 };
        int[] airialCount = new int[4] { 0, 0, 0, 0 };
        int[] floorCount = new int[2] { 0, 0 };

        #region Sector 01
        for (int i = 0; i < NoteField.s_noteHolders.Count; i++)
        {
            holder = NoteField.s_noteHolders[i];
            for (int j = 0; j < 4; j++)
            {
                normalCount[j] += holder.normals[j] == null ? 0 : holder.normals[j].length;
                airialCount[j] += holder.airials[j] == null ? 0 : 1;
            }
            floorCount[0] += holder.bottoms[0] == null ? 0 : holder.bottoms[0].length;
            floorCount[1] += holder.bottoms[1] == null ? 0 : holder.bottoms[1].length;
        }

        noteCount = floorCount[0] + floorCount[1]
            + normalCount[0] + normalCount[1] + normalCount[2] + normalCount[3]
            + airialCount[0] + airialCount[1] + airialCount[2] + airialCount[3];

        float calCount, maxCount;
        maxCount = normalCount[0] + airialCount[0];
        for (int i = 1; i < 4; i++)
        {
            calCount = normalCount[i] + airialCount[i];
            if (calCount > maxCount) { maxCount = calCount; }
        }

        calCount = floorCount[0];
        if (calCount > maxCount) { maxCount = calCount; }
        calCount = floorCount[1];
        if (calCount > maxCount) { maxCount = calCount; }

        if (maxCount == 0)
        {
            foreach(SpriteRenderer renderer in renderer_01)
            {
                renderer.transform.localScale = new Vector3(0.005f, 0, 1);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                calCount = normalCount[i] + airialCount[i];
                renderer_01[i].transform.localScale = new Vector3
                    (0.005f, (calCount / maxCount) * 0.28f, 1);
                renderer_01[i + 4].transform.localScale = new Vector3
                    (1, normalCount[i] == 0 ? 0 : airialCount[i] / calCount, 1);
            }
            renderer_01[8].transform.localScale
                = new Vector3(0.005f, (floorCount[0] / maxCount) * 0.28f, 1);
            renderer_01[9].transform.localScale
                = new Vector3(0.005f, (floorCount[1] / maxCount) * 0.28f, 1);
        }
        #endregion
        
        #region Sector 02
        for (int i = 0; i < 4; i++)
        {
            float[] noteValue = new float[2];
            noteValue[0] = airialCount[i];
            noteValue[1] = airialCount[i] + normalCount[i];
            if (noteValue[1] == 0)
            {
                slider_02[i].value = 0;
                tmp_02[i].text = "0%";
                tmp_02[i+4].text = "0\n<size=1>total 0";
            }
            else
            {
                slider_02[i].value = noteValue[0] / noteValue[1];
                tmp_02[i].text = String.Format
                    ("{0}%", Mathf.RoundToInt(noteValue[0] / noteValue[1] * 10000) / 100f);
                tmp_02[i + 4].text = String.Format("{0}\n<size=1>total {1}", noteValue[0], noteValue[1]);
            }
        }
        #endregion
        
        #region Sector 03
        if (noteCount == 0)
        {
            slider_03[0].value = 0;
            slider_03[1].value = 0;

            tmp_03[0].text = "- - - -\n<size=1.3>- - - -";
            tmp_03[1].text = "- - - -\n<size=1.3>- - - -";
        }
        else
        {
            float[] value = new float[2];
            value[0] = airialCount[0] + airialCount[1] + airialCount[2] + airialCount[3];
            value[1] = floorCount[0] + floorCount[1];

            slider_03[0].value = value[0] / noteCount;
            slider_03[1].value = value[1] / noteCount;

            tmp_03[0].text = String.Format("{0}%\n<size=1.3>{1} of {2}",
                Mathf.RoundToInt(value[0] / noteCount * 10000) / 100f, value[0], noteCount);
            tmp_03[1].text = String.Format("{0}%\n<size=1.3>{1} of {2}",
                Mathf.RoundToInt(value[1] / noteCount * 10000) / 100f, value[1], noteCount);
        }
        #endregion
    
        SystemInfo.noteCount = noteCount;
    }
}
