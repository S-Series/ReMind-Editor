using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongJudgeVisualize : MonoBehaviour
{
    public static LongJudgeVisualize[][] s_LJV =
    {
        new LongJudgeVisualize[5],
        new LongJudgeVisualize[2],
        new LongJudgeVisualize[5],
        new LongJudgeVisualize[2]
    };
    [SerializeField] private int Line;
    [SerializeField] private bool isGameField;
    [SerializeField] private bool isBottomLine;
    [SerializeField] private Transform[] transforms;
    [SerializeField] private Animator animator;
    private SpriteRenderer[] sprite;
    private IEnumerator VisualizeCoroutine;

    private void Start()
    {
        sprite = new SpriteRenderer[3];
        sprite[0] = transforms[0].GetComponent<SpriteRenderer>();
        sprite[1] = transforms[1].GetComponent<SpriteRenderer>();
        sprite[2] = transforms[2].GetComponent<SpriteRenderer>();
        s_LJV[isGameField ? (isBottomLine ? 3 : 2) : (isBottomLine ? 1 : 0)][Line - 1] = this;
        VisualizeCoroutine = ILongVisualize(0, new float[] { 0, 0 });
    }
    public void StartLongVisualize(int Length, float[] ms)
    {
        StopAllCoroutines();
        sprite[0].enabled = true;
        sprite[1].enabled = true;
        sprite[2].enabled = true;
        transforms[1].localScale = new Vector3(95, Length * 5f * NoteField.s_Zoom, 95);
        transforms[2].localPosition = new Vector3(0, Length * 500f * NoteField.s_Zoom, 0);
        VisualizeCoroutine = ILongVisualize(Length, ms);
        StartCoroutine(VisualizeCoroutine);
    }
    private IEnumerator ILongVisualize(int Length, float[] ms)
    {
        float per, zoom;
        float[] values;
        zoom = NoteField.s_Zoom / 2f;
        values = new float[2] { Length * 10, Length * 100 };
        while (true)
        {
            yield return null;
            if (AutoTest.s_Ms > ms[1]) { break; }
            per = 1.0f - Mathf.InverseLerp(ms[0], ms[1], AutoTest.s_Ms);
            transforms[1].localScale = new Vector3(95, values[0] * per * zoom, 95);
            transforms[2].localPosition = new Vector3(0, values[1] * per * zoom, 0);
        }
        sprite[0].enabled = false;
        sprite[1].enabled = false;
        sprite[2].enabled = false;
    }
}
