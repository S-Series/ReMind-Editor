using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class NoteGenerate : MonoBehaviour
{
    private static NoteGenerate s_this;
    public static bool s_isGenerating = false;

    public static int posX = 0, posY = 0, posZ = 0, s_Line = 0, s_previewIndex = 0;

    [SerializeField] GameObject[] previews;
    [SerializeField] GameObject[] GeneratePrefabs;
    [SerializeField] Transform GenerateField;
    /// <summary>
    /// previews[0] = Normal Note   || GeneratePrefabs[0]
    /// previews[1] = Bottom Note   || GeneratePrefabs[0]
    /// previews[2] = Eraser        || 
    /// previews[3] = Airial Note   || GeneratePrefabs[0]
    /// previews[4] = Speed Note    || GeneratePrefabs[1]
    /// previews[5] = Effect Note   || GeneratePrefabs[2]
    /// </summary>

    void Awake()
    {
        s_this = this;
        ChangePreview(-1);
    }

    private void Update()
    {
        if (!s_isGenerating) { return; }

        posX = s_Line * 240 - 600;

        if (s_previewIndex == 1)
        {
            if (s_Line <= 2) { posX = -240; }
            else { posX = 240; }
        }
        else if (s_previewIndex == 3 || s_previewIndex == 4) { posX = -1121; }
        else if (s_Line == 0) { posX = -360; }

        previews[s_previewIndex].transform.localPosition
            = new Vector3(posX, posY * 2
            + 1600f / GuideGenerate.s_guideCount * NoteField.s_Scroll * 2
            + 1600f * NoteField.s_Page * 2, posZ);
    }

    public static void GenerateNote()
    {
        int pos;
        GameObject copyObject;
        MultyNoteHolder multyHolder;
        pos = posY + Mathf.RoundToInt
            (1600f / GuideGenerate.s_guideCount * NoteField.s_Scroll + 1600f * NoteField.s_Page);
        multyHolder = NoteField.s_multyHolders.Find(item => item.stdPos == pos);

        switch (ToolManager.noteType)
        {
            #region //$ Normal Note Generate
            case ToolManager.NoteType.Normal:

                if (multyHolder != null)
                {
                    if (s_previewIndex == 3)
                        { if (multyHolder.airials[s_Line - 1] != null) { return; } }
                    else
                        { if (multyHolder.normals[s_Line - 1] != null) { return; } }
                }

                NormalNote normal;
                NoteHolder normalHolder;

                //$ Init NormalNote
                normal = new NormalNote();
                normal.pos = pos;
                normal.ms = NoteClass.CalMs(normal.pos);
                normal.line = s_Line;
                normal.isAir = s_previewIndex == 3 ? true : false;

                //$ Init NoteHolder
                copyObject = Instantiate(s_this.GeneratePrefabs[0], s_this.GenerateField, false);
                normalHolder = copyObject.GetComponent<NoteHolder>();
                normalHolder.is2D = true;
                normalHolder.type = s_previewIndex == 0 ? NoteHolder.NoteType.Normal :
                    s_previewIndex == 1 ? NoteHolder.NoteType.Bottom : NoteHolder.NoteType.Air;

                NoteClass.s_NormalNotes.Add(normal);
                normal.holder = normalHolder;
                normalHolder.noteClass = normal;
                normalHolder.GenerateTwin();
                normalHolder.UpdateNote(true);

                if (multyHolder == null)
                {
                    multyHolder = new MultyNoteHolder();
                    multyHolder.stdMs = normal.ms;
                    multyHolder.stdPos = normal.pos;
                    NoteField.s_multyHolders.Add(multyHolder);
                    NoteField.SortMultyHolder();
                }
                if (normal.isAir) { multyHolder.airials[normal.line - 1] = normal; }
                else { multyHolder.normals[normal.line - 1] = normal; }


                break;
            #endregion

            #region //$ Speed Note Generate
            case ToolManager.NoteType.Speed:

                SpeedNote speed;
                SpeedHolder speedHolder;

                //$ Init SpeedNote
                speed = new SpeedNote();
                speed.ms = 0;
                speed.pos = pos;
                speed.bpm = ValueManager.s_Bpm;
                speed.multiple = 1.0;

                //$ Init SpeedHolder
                copyObject = Instantiate(s_this.GeneratePrefabs[1], s_this.GenerateField, false);
                speedHolder = copyObject.GetComponent<SpeedHolder>();

                NoteClass.s_SpeedNotes.Add(speed);
                NoteClass.InitAll();
                speed.holder = speedHolder;
                speedHolder.noteClass = speed;

                break;
            #endregion

            #region //$ Effect Note Generate || Not Definded
            case ToolManager.NoteType.Effect:
                break;
            #endregion

            default: print("returned"); return;
        }
    }

    public static void ChangePreview(int index)
    {
        foreach (GameObject gameObject in s_this.previews) { gameObject.SetActive(false); }

        if (index < 0 || index >= s_this.previews.Length)
        {
            s_isGenerating = false;
            ToolManager.noteType = ToolManager.NoteType.Null;
            return;
        }
        else { s_isGenerating = true; }

        if (index == 2) { ToolManager.noteType = ToolManager.NoteType.Null; }
        else if (index < 4) { ToolManager.noteType = ToolManager.NoteType.Normal; }
        else if (index == 4) { ToolManager.noteType = ToolManager.NoteType.Speed; }
        else if (index == 5) { ToolManager.noteType = ToolManager.NoteType.Effect; }
        else { throw new System.Exception("Wrong Note Index"); }

        s_previewIndex = index;
        s_this.previews[index].SetActive(true);

        GuideGenerate.EnableGuideCollider(true);
    }

    public static void Escape()
    {
        print("Escaped");
        s_isGenerating = false;

        foreach (GameObject gameObject in s_this.previews) { gameObject.SetActive(false); }

        GuideGenerate.EnableGuideCollider(false);
    }
}
