using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;
using System.Runtime.CompilerServices;

public class NoteGenerate : MonoBehaviour
{
    private static NoteGenerate s_this;
    public static bool s_isGenerating = false;

    public static int posX = 0, posY = 0, posZ = 0, s_Line = 0;
    public static Vector3[] InitVec = new Vector3[2];
    public static NoteType s_previewType = NoteType.None;

    [SerializeField] GameObject[] previews;
    [SerializeField] GameObject previewGuide;
    [SerializeField] GameObject[] GeneratePrefabs;
    [SerializeField] Transform[] GenerateField;

    void Awake()
    {
        s_this = this;
        Escape();

        InitVec[0] = GeneratePrefabs[0].transform.localPosition;
        InitVec[1] = GeneratePrefabs[1].transform.localPosition;
    }

    private void Update()
    {
        if (!s_isGenerating) { return; }

        posX = s_Line * 240 - 600;

        if (s_previewType == NoteType.Scratch)
        {
            if (s_Line <= 2) { posX = -480; }
            else { posX = 0; }
        }
        else if (s_previewType == NoteType.Speed || s_previewType == NoteType.Effect) { posX = -1050; }
        else if (s_Line == 0) { posX = -360; }

        previews[(int)s_previewType].transform.localPosition
            = new Vector3(posX, 2 * posY / NoteField.s_Zoom, posZ);
        previewGuide.transform.localPosition = new Vector3(3086, 2 * posY / NoteField.s_Zoom, 0);
    }

    public static void GenerateNote()
    {
        int pos;
        GameObject copyObject;
        NoteHolder holder;
        pos = posY + Mathf.RoundToInt
            (1600f / GuideGenerate.s_guideCount * NoteField.s_Scroll + 1600f * NoteField.s_Page);
        holder = NoteHolder.s_holders.Find(item => item.stdPos == pos);

        if (holder != null)
        {
            if (s_previewType == NoteType.Scratch)
            { if (holder.bottoms[s_Line < 3 ? 0 : 1] != null) { return; } }

            else if (s_previewType == NoteType.Airial)
            { if (holder.airials[s_Line - 1] != null) { return; } }

            else if (s_previewType == NoteType.Speed)
            { if (holder.speedNote != null) { return; } }

            else if (s_previewType == NoteType.Effect)
            { if (holder.effectNote != null) { return; } }

            else if (s_previewType == NoteType.Normal)
            { if (holder.normals[s_Line - 1] != null) { return; } }

            else { throw new System.Exception(""); }
        }
        else
        {
            copyObject = Instantiate(s_this.GeneratePrefabs[0], s_this.GenerateField[0], false);
            //copyObject.transform.localPosition = 
            holder = copyObject.GetComponent<NoteHolder>();
            holder.name = "Pos : " + pos.ToString();
            holder.stdMs = NoteClass.PosToMs(pos);
            holder.stdPos = pos;
            holder.EnableCollider(false);

            copyObject = Instantiate(s_this.GeneratePrefabs[1], s_this.GenerateField[1], false);
            holder.gameNoteHolder = copyObject.GetComponent<GameNoteHolder>();
            holder.gameNoteHolder.name = "Pos : " + pos.ToString();
            holder.ApplyGameMode(GameManager.gameMode);
            NoteHolder.s_holders.Add(holder);
        }

        switch (ToolManager.noteType)
        {
            case NoteType.Normal:
            case NoteType.Airial:
                NormalNote normal;
                normal = new NormalNote(
                    new int[3] { holder.stdPos, s_Line, 1 },
                    false
                );
                if (s_previewType == NoteType.Airial) { holder.airials[s_Line - 1] = normal; }
                else { holder.normals[s_Line - 1] = normal; }
                break;

            case NoteType.Scratch:
                ScratchNote scratch;
                scratch = new ScratchNote(
                    _posY: holder.stdPos,
                    _length: 0,
                    new int[2] {0, 1000},
                    false
                );
                holder.bottoms[s_Line < 3 ? 0 : 1] = scratch;
                break;

            case NoteType.Speed:
                SpeedNote speed;
                speed = new SpeedNote(pos);
                holder.speedNote = speed;
                break;

            case NoteType.Effect:
                EffectNote effect;
                effect = new EffectNote(pos);
                holder.effectNote = effect;
                break;

            default: print("returned"); return;
        }

        holder.UpdateNote();
        holder.UpdateScale();
        holder.EnableNote(false);
        holder.EnableCollider(false);

        NoteField.SortNoteHolder();
        InfoField.UpdateInfoField();
        ObjectCooling.UpdateCooling();
    }
    public static NoteHolder GenerateNoteManual(int pos)
    {
        NoteHolder ret;
        GameObject copyObject;

        ret = NoteHolder.s_holders.Find(item => item.stdPos == pos);

        if (ret != null) { return ret; }

        copyObject = Instantiate(s_this.GeneratePrefabs[0], s_this.GenerateField[0], false);
        ret = copyObject.GetComponent<NoteHolder>();
        ret.name = "Pos : " + pos.ToString();
        ret.stdMs = NoteClass.PosToMs(pos);
        ret.stdPos = pos;
        ret.EnableCollider(false);

        copyObject = Instantiate(s_this.GeneratePrefabs[1], s_this.GenerateField[1], false);
        ret.gameNoteHolder = copyObject.GetComponent<GameNoteHolder>();
        ret.gameNoteHolder.name = "Pos : " + pos.ToString();
        NoteHolder.s_holders.Add(ret);
        return ret;
    }
    public static void ChangePreview(int index)
    {

        foreach (GameObject gameObject in s_this.previews) { gameObject.SetActive(false); }

        s_this.previews[index].SetActive(true);

        GuideGenerate.EnableGuideCollider(true);
        foreach (NoteHolder holder in NoteHolder.s_holders) { holder.EnableCollider(false); }
    }
    public static void Escape()
    {
        s_isGenerating = false;

        foreach (GameObject gameObject in s_this.previews) { gameObject.SetActive(false); }

        GuideGenerate.EnableGuideCollider(false);
        foreach (NoteHolder holder in NoteHolder.s_holders) { holder.EnableCollider(true); }
    }

}
