using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongJudgeVisualize : MonoBehaviour
{
    public static LongJudgeVisualize[] s_LJV = new LongJudgeVisualize[12];
    public LongJudgeVisualize[] view;
    [SerializeField] private int Line;
    [SerializeField] private bool isGameField;
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
        s_LJV[isGameField ? Line + 5 : Line - 1] = this;
        VisualizeCoroutine = ILongVisualize(0, new int[] { 0, 0 });
    }
    public void StartLongVisualize(int Length, int[] ms)
    {
        StopCoroutine(VisualizeCoroutine);
        sprite[0].enabled = true;
        sprite[1].enabled = true;
        sprite[2].enabled = true;
        transforms[1].localScale = new Vector3(95, Length * 5f * NoteField.s_Zoom, 95);
        transforms[2].localPosition = new Vector3(0, Length * 500f * NoteField.s_Zoom, 0);
        VisualizeCoroutine = ILongVisualize(Length, ms);
        StartCoroutine(VisualizeCoroutine);
    }
    private IEnumerator ILongVisualize(int Length, int[] ms)
    {
        float per, zoom;
        float[] values;
        zoom = NoteField.s_Zoom;
        print(zoom); zoom = 1;
        values = new float[2] { Length * 10, Length * 100 };
        while (true)
        {
            yield return null;
            per = (1.0f - Mathf.Lerp(ms[0], ms[1], AutoTest.s_Ms)) * 100f;
            if (per <= 0) { break; }
            transforms[1].localScale = new Vector3(95, values[0] * per * zoom, 95);
            transforms[2].localPosition = new Vector3(0, values[1] * per * zoom, 0);
        }
        sprite[0].enabled = false;
        sprite[1].enabled = false;
        sprite[2].enabled = false;
    }
}
