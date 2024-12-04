using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideGenerate : MonoBehaviour
{
    private static GuideGenerate s_this;
    public static int s_guideCount = 1;
    public static int[] s_guidePos = new int[1] { 0 };
    public static bool[] s_Accent = new bool[2] { false, true };
    public static GameObject ColliderPrefab;
    private static Transform ColliderField;
    private static bool[] s_HasAccent = new bool[2] { false, true };

    [SerializeField] GameObject _ColliderPrefab;
    [SerializeField] Transform _ColliderField;
    [SerializeField] TMP_InputField InputGuideCount;

    private void Awake() 
    {
        s_this = this;
        ColliderPrefab = _ColliderPrefab;
        ColliderField = _ColliderField;
    }
    private void Start() { Generate(8); }

    public static void Generate(int count)
    {
        if (count < 01) { count = 01; }
        if (count > 33) { count = 32; }

        NoteField.s_Scroll = Mathf.FloorToInt(1.0f * NoteField.s_Scroll * count / s_guideCount);
        NoteField.s_this.UpdateField();

        //s_HasAccent[0] = count % 3 == 0 ? true : false;
        //s_HasAccent[1] = count % 4 == 0 ? true : false;

        GameObject copyObject;

        s_guideCount = count;
        s_guidePos = new int[count + 1];

        for (int i = 0; i < count; i++)
        {
            s_guidePos[i] = System.Convert.ToInt32(1600f / count * i);
            for (int j = 0; j < 300; j++)
            {
                copyObject = Instantiate(ColliderPrefab, ColliderField, false);
                copyObject.transform.localPosition = new Vector3(0, j * 1600 + (1600f / count * i), 0);
            }
        }
        s_guidePos[count] = 1600;
    }
    public static void UpdateGuideColor()
    {

    }
    public static void EnableGuideCollider(bool isEnable)
    {
        foreach (NoteHolder holder in NoteHolder.s_holders)
        {
            holder.EnableCollider(true);
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

    public void AccentToggle(UnityEngine.UI.Toggle toggle)
    {
        if (toggle.gameObject.CompareTag("01")) { s_Accent[1] = toggle.isOn; }
        else { s_Accent[0] = toggle.isOn; }
        UpdateGuideColor();
    }
}
