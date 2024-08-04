using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;
using System.Runtime.CompilerServices;

public class NoteGenerate : MonoBehaviour
{
    public static NoteGenerate s_this;
    public static bool s_isGenerating = false;

    public static int posX = 0, posY = 0, posZ = 0, s_Line = 0;
    public static Vector3[] InitVec = new Vector3[2];
    public static NoteType s_previewType = NoteType.None;

    private static GameObject[] PreviewObjects;
    [SerializeField] GameObject[] _previews;
    [SerializeField] GameObject previewGuide;
    [SerializeField] GameObject[] GeneratePrefabs;
    [SerializeField] Transform[] GenerateField;

    void Awake()
    {
        s_this = this;
        Escape();

        InitVec[0] = GeneratePrefabs[0].transform.localPosition;
        InitVec[1] = GeneratePrefabs[1].transform.localPosition;

        PreviewObjects = _previews;
        _previews = null;
    }

    public static void ShowPreview(int lineValue, int posValue)
    {
        if (s_previewType == NoteType.None) { return; }

        s_Line = lineValue;
        posY = posValue;
        posX = lineToPosX(lineValue);
        
        GameObject targetObject;
        foreach (GameObject @object in PreviewObjects) { @object.SetActive(false); }
        targetObject = PreviewObjects[(int)s_previewType -1];
        targetObject.SetActive(true);
        targetObject.transform.localPosition = new Vector3(posX, posY, posZ);
    }
    public static void GenerateNote(int posValue)
    {
        if (s_previewType == NoteType.None) { return; }

        NoteHolder targetHolder;
        targetHolder = NoteHolder.s_holders.Find(item => item.stdPos == posValue);

        switch (s_previewType)
        {
            case NoteType.Normal:
                break;

            case NoteType.Airial:
                break;

            case NoteType.Floor:
                break;

            case NoteType.Speed:
                break;
                
            case NoteType.Effect:
                break;

            default: return;
        }
    }
    public static NoteHolder GenerateNoteManual(int pos)
    {
        NoteHolder ret;
        ret = NoteHolder.s_holders.Find(item => item.stdPos == pos);
        if (ret == null) { ret = new NoteHolder(pos); }
        return ret;
    }
    private static void ChangePreview(int index)
    {

    }
    public static void Escape()
    {
    }
    public static void ToolAction(int index)
    {

    }

    private static int lineToPosX(int line)
    {
        switch (s_previewType)
        {
            case NoteType.Normal :
            case NoteType.Airial :
                return -600 + 240 * line;

            case NoteType.Floor :
                return line > 2 ? +240 : -240;

            case NoteType.Effect :
            case NoteType.Speed :
                return -1120;

            case NoteType.None :
            default:
                return 0;
        }
    }
}
