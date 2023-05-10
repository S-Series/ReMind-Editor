using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideGenerate : MonoBehaviour
{
    private static GuideGenerate s_this;
    public static int s_guideCount = 1;
    public static int[] s_guidePos = new int[1] { 0 };
    private static List<GuideHolder> holders = new List<GuideHolder>();

    [SerializeField] GameObject ColliderPrefab;
    [SerializeField] Transform ColliderField;
    [SerializeField] TMP_InputField InputGuideCount;

    private void Awake() { s_this = this; }
    private void Start() { Generate(8); }

    public static void Generate(int count)
    {
        GameObject copyObject;
        GuideHolder copyHolder;
        
        foreach (GuideHolder holder in holders)
        {
            Destroy(holder.gameObject);
        }

        holders = new List<GuideHolder>();

        if (count < 01) { count = 01; }
        if (count > 33) { count = 32; }

        NoteField.s_Scroll = Mathf.FloorToInt(1.0f * NoteField.s_Scroll * count / s_guideCount);
        NoteField.s_this.UpdateField();

        s_guideCount = count;

        s_guidePos = new int[count + 1];
        for (int i = 0; i < count; i++) { s_guidePos[i] = System.Convert.ToInt32(1600f / count * i); }
        s_guidePos[count] = 1600;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < count; j++)
            {
                copyObject = Instantiate(s_this.ColliderPrefab, s_this.ColliderField, false);
                copyHolder = copyObject.GetComponent<GuideHolder>();

                copyHolder.index = j;
                copyHolder.posY = 1600 * i + Mathf.RoundToInt(1600.0f / count * j);
                copyHolder.ReSizeCollider(count, j);
                copyHolder.EnableCollider(NoteGenerate.s_isGenerating);
                copyObject.transform.localPosition = new Vector3(0, copyHolder.posY * 2, 0);
                holders.Add(copyHolder);
            }
        }
        UpdateGuideColor();
    }

    public static void EnableGuideCollider(bool isEnable)
    {
        foreach (GuideHolder holder in holders)
        {
            holder.EnableCollider(isEnable);
        }
        foreach (NoteHolder holder in NoteField.s_noteHolders)
        {
            holder.EditMode(true);
        }
    }
    public static void UpdateGuideColor()
    {
        foreach(GuideHolder holder in holders)
        {
            holder.UpdateLineColor(s_guideCount, NoteField.s_Scroll);
        }
    }
    public static void GuideFieldSize(Vector3 scale, float invertScale)
    {
        s_this.ColliderField.transform.localScale = scale;
        foreach(GuideHolder holder in holders)
        {
            holder.ReSizeLineRenderer(invertScale);
        }
    }

    public void Input_Guide()
    {
        int _count;
        try { _count = System.Convert.ToInt32(InputGuideCount.text); }
        catch { _count = 1; }
        if (_count <= 0) { _count = 1; }
        Generate(_count);
    }
}
