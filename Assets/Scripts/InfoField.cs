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
    [SerializeField] private Slider slider_03;
    [SerializeField] private TextMeshPro tmp_03;
    //# Sector 04

    void Awake() { s_this = this; }
    public static void UpdateInfoField() { s_this.UpdateField(); }
    private void UpdateField()
    {
        int noteCount = 0;
        int[] normalCount = new int[4] { 0, 0, 0, 0 };
        int[] airialCount = new int[4] { 0, 0, 0, 0 };
        int[] floorCount = new int[2] { 0, 0 };

        NoteHolder holder;
        for (int i = 0; i < NoteField.s_noteHolders.Count; i++)
        {
            holder = NoteField.s_noteHolders[i];
            for (int j = 0; j < 4; j++)
            {
                normalCount[j] += holder.normals[j] == null ? 0 : holder.normals[j].length;
                airialCount[j] += holder.airials[j] == null ? 0 : holder.normals[j].length;
            }
            floorCount[0] += holder.bottoms[0] == null ? 0 : holder.bottoms[0].length;
            floorCount[1] += holder.bottoms[1] == null ? 0 : holder.bottoms[1].length;
        }

        noteCount = floorCount[0] + floorCount[1]
            + normalCount[0] + normalCount[1] + normalCount[2] + normalCount[3]
            + airialCount[0] + airialCount[1] + airialCount[2] + airialCount[3];

        int maxCount, calCount;
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

        for (int i = 0; i < 4; i++)
        {
            calCount = normalCount[i] + airialCount[i];
            renderer_01[i].transform.localScale 
                = new Vector3(0.005f, (calCount / maxCount) * 0.28f, 1);
            renderer_01[i + 4].transform.localScale = new Vector3(0.005f, 
                normalCount[i] == 0 ? 0 : airialCount[i] / normalCount[i], 1);
        }
    }
}
