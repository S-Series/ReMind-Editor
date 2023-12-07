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
    [SerializeField] Transform[] GenerateField;
    /// <summary>
    /// previews[0] = Normal Note
    /// previews[1] = Bottom Note
    /// previews[2] = Airial Note
    /// previews[3] = Speed Note
    /// previews[4] = Effect Note
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
            if (s_Line <= 2) { posX = -480; }
            else { posX = 0; }
        }
        else if (s_previewIndex == 3 || s_previewIndex == 4) { posX = -1050; }
        else if (s_Line == 0) { posX = -360; }

        previews[s_previewIndex].transform.localPosition
            = new Vector3(posX, (posY * 2
            + 1600f / GuideGenerate.s_guideCount * NoteField.s_Scroll * 2
            + 1600f * NoteField.s_Page * 2) / NoteField.s_Zoom, posZ);
    }

    public static void GenerateNote()
    {
        int pos;
        GameObject copyObject;
        NoteHolder holder;
        pos = posY + Mathf.RoundToInt
            (1600f / GuideGenerate.s_guideCount * NoteField.s_Scroll + 1600f * NoteField.s_Page);
        holder = NoteField.s_noteHolders.Find(item => item.stdPos == pos);

        if (holder != null)
        {
            if (s_previewIndex == 1)
            { if (holder.bottoms[s_Line < 3 ? 0 : 1] != null) { return; } }

            else if (s_previewIndex == 2)
            { if (holder.airials[s_Line - 1] != null) { return; } }

            else if (s_previewIndex == 3)
            { if (holder.speedNote != null) { return; } }

            else if (s_previewIndex == 4)
            { if (holder.effectNote != null) { return; } }
            else
            { if (holder.normals[s_Line - 1] != null) { return; } }
        }
        else
        {
            copyObject = Instantiate(s_this.GeneratePrefabs[0], s_this.GenerateField[0], false);
            //copyObject.transform.localPosition = 
            holder = copyObject.GetComponent<NoteHolder>();
            holder.name = "Pos : " + pos.ToString();
            holder.stdMs = NoteClass.CalMs(pos);
            holder.stdPos = pos;
            holder.EnableCollider(false);

            copyObject = Instantiate(s_this.GeneratePrefabs[1], s_this.GenerateField[1], false);
            holder.gameNoteHolder = copyObject.GetComponent<GameNoteHolder>();
            holder.gameNoteHolder.name = "Pos : " + pos.ToString();
            NoteField.s_noteHolders.Add(holder);
        }

        switch (ToolManager.noteType)
        {
            #region //$ Normal Note Generate
            case ToolManager.NoteType.Normal:

                //# Init NormalNote
                NormalNote normal;
                normal = NormalNote.Generate();
                normal.ms = holder.stdMs;
                normal.pos = holder.stdPos;
                normal.line = s_Line;
                normal.isAir = s_previewIndex == 2 ? true : false;
                normal.length = 1;

                if (s_previewIndex == 2)
                {
                    holder.airials[s_Line - 1] = normal;
                }
                else if (s_previewIndex == 1)
                {
                    normal.line = s_Line < 3 ? 1 : 2;
                    holder.bottoms[s_Line < 3 ? 0 : 1] = normal;
                }
                else { holder.normals[s_Line - 1] = normal; }
                break;
            #endregion

            #region //$ Speed Note Generate
            case ToolManager.NoteType.Speed:

                SpeedNote speed;
                //SpeedHolder speedHolder;

                //# Init SpeedNote
                speed = new SpeedNote();
                speed.ms = 0;
                speed.pos = pos;
                speed.bpm = ValueManager.s_Bpm;
                speed.multiple = 1.0;

                //# Init SpeedHolder
                holder.speedNote = speed;
                break;
            #endregion

            #region //$ Effect Note Generate
            case ToolManager.NoteType.Effect:

                EffectNote effect;
                //EffectHolder effectHolder;

                //# Init EffectNote
                effect = new EffectNote();
                effect.ms = 0;
                effect.pos = pos;
                effect.value = 0;
                effect.effectIndex = 0;

                //# Init EffectHolder
                holder.effectNote = effect;
                break;
            #endregion

            default: print("returned"); return;
        }

        holder.UpdateNote();
        holder.UpdateScale();
        holder.EnableCollider(false);

        NoteField.SortNoteHolder();
        InfoField.UpdateInfoField();
    }
    public static NoteHolder GenerateNoteManual(int pos)
    {
        NoteHolder ret;
        GameObject copyObject;

        ret = NoteField.s_noteHolders.Find(item => item.stdPos == pos);

        if (ret != null) { return ret; }

        copyObject = Instantiate(s_this.GeneratePrefabs[0], s_this.GenerateField[0], false);
        ret = copyObject.GetComponent<NoteHolder>();
        ret.name = "Pos : " + pos.ToString();
        ret.stdMs = NoteClass.CalMs(pos);
        ret.stdPos = pos;
        ret.EnableCollider(false);

        copyObject = Instantiate(s_this.GeneratePrefabs[1], s_this.GenerateField[1], false);
        ret.gameNoteHolder = copyObject.GetComponent<GameNoteHolder>();
        ret.gameNoteHolder.name = "Pos : " + pos.ToString();
        NoteField.s_noteHolders.Add(ret);
        return ret;
    }
    public static void ChangePreview(int index)
    {
        foreach (GameObject gameObject in s_this.previews) { gameObject.SetActive(false); }

        if (index == 3 && s_previewIndex == 3) { index = 4; }

        if (index < 0 || index >= s_this.previews.Length)
        {
            s_isGenerating = false;
            ToolManager.noteType = ToolManager.NoteType.Null;
            return;
        }
        else { s_isGenerating = true; }

        if (index < 3) { ToolManager.noteType = ToolManager.NoteType.Normal; }
        else if (index == 3) { ToolManager.noteType = ToolManager.NoteType.Speed; }
        else if (index == 4) { ToolManager.noteType = ToolManager.NoteType.Effect; }
        else { throw new System.Exception("Wrong Note Index"); }

        s_previewIndex = index;
        s_this.previews[index].SetActive(true);

        GuideGenerate.EnableGuideCollider(true);
        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EnableCollider(false); }
    }
    public static void Escape()
    {
        s_isGenerating = false;

        foreach (GameObject gameObject in s_this.previews) { gameObject.SetActive(false); }

        GuideGenerate.EnableGuideCollider(false);
        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EnableCollider(true); }
    }
}
