using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameNote;
using TMPro;
using System;

public class NoteField : MonoBehaviour
{
    public static NoteField s_this;

    public static bool s_isFieldMovable = true;
    public static int s_Page = 0;
    public static int s_Scroll = 0;
    public static int s_Zoom = 2;
    public static int s_StartPos = 0;
    private static bool s_isCtrl = false;

    [SerializeField] GameObject LinePrefab;
    [SerializeField] Transform PreviewNoteParent;
    [SerializeField] Transform[] DrawField;
    [SerializeField] GameObject[] LineObjects;

    private void Awake()
    {
        s_this = this;

        GameObject _copyObject;
        LineHolder _holder;
        for (int i = 0; i < 999; i++)
        {
            _copyObject = Instantiate(LinePrefab, DrawField[0], false);
            _copyObject.transform.localPosition = new Vector3(-465f, 1600 * 2 * i, 0);

            _holder = _copyObject.transform.GetComponent<LineHolder>();
            _holder.page = i;
            _holder.texts[0].text = string.Format("{0:D3}", i + 1);
            _holder.texts[1].text = string.Format("{0}", NoteClass.PosToMs(1600 * i));
            _holder.EnableHolder(false);

            LineHolder.s_holders.Add(_holder);
        }
    }
    public NoteHolder FindMultyHolder(NormalNote note)
    {
        NoteHolder ret = null;
        if (note.isAirial)
        {
            for (int i = 0; i < NoteHolder.s_holders.Count; i++)
            {
                if (NoteHolder.s_holders[i].airials.Contains(note))
                {
                    ret = NoteHolder.s_holders[i];
                    break;
                }
            }
        }
        else if (note.line > 4)
        {
            return null;
        }
        else
        {
            for (int i = 0; i < NoteHolder.s_holders.Count; i++)
            {
                if (NoteHolder.s_holders[i].normals.Contains(note))
                {
                    ret = NoteHolder.s_holders[i];
                    break;
                }
            }
        }
        return ret;
    }
    public void UpdateField()
    {
        Vector3 _pos;
        Vector3 _scale;
        int _count = GuideGenerate.s_guideCount;
        float zoomValue;

        s_StartPos = s_Page * 1600 + Mathf.RoundToInt(1600f / _count * s_Scroll);
        ObjectCooling.UpdateCooling(s_StartPos);

        if (s_Scroll < 0) { s_Page--; s_Scroll += _count; }
        while (s_Scroll > _count) { s_Page++; s_Scroll -= _count; }

        if (s_Page < 0) { s_Page = 0; s_Scroll = 0; }
        else if (s_Page > 999) { s_Page = 999; }

        if (s_Zoom < 01) { s_Zoom = 01; }
        else if (s_Zoom > 5) { s_Zoom = 5; }

        zoomValue = 10.0f / s_Zoom;

        _pos = new Vector3(-0.5f, ((s_Page * -10)
            - (10f / _count * s_Scroll)) * zoomValue / 10 - 5, 0);
        _scale = new Vector3(0.00312f, zoomValue * 0.0003125f, 0.00312f);

        DrawField[0].localScale = _scale;
        DrawField[0].localPosition = _pos;

        DrawField[1].localScale = new Vector3(0.00415f, 2f * zoomValue * 0.0003125001f, 0.00415f);
        DrawField[1].localPosition = new Vector3(25, -31.3f, 2f *
            (((s_Page * -10) - (10f / _count * s_Scroll)) * zoomValue / 10) - 19f);

        DrawField[2].localScale = _scale;
        DrawField[2].localPosition = _pos;

        SpectrumManager.UpdatePosY((s_Page * -1600f) - (1600f * s_Scroll / _count));
        SpectrumManager.UpdateScale(zoomValue / 5f);
        PreviewNoteParent.localScale = new Vector3(0.00312f, 0.00312f, 0.00312f);

        GuideGenerate.UpdateGuideColor();
        GuideGenerate.GuideFieldSize(_scale, zoomValue);

        foreach (NoteHolder holder in NoteHolder.s_holders) { holder.UpdateScale(); }
        foreach (LineHolder holder in LineHolder.s_holders) { holder.UpdateScale(); }

        EditBox.UpdateRenderer();
    }
    public static void SortNoteHolder()
    {
        NoteHolder.s_holders.OrderBy(item => item.stdPos).ToList();
    }
    public static void UpdateFieldByGameMode()
    {
        foreach (GameObject obj in s_this.LineObjects) { obj.SetActive(false); }
        s_this.LineObjects[(int)GameManager.gameMode - 4].SetActive(true);
        NoteHolder.GameModeHolderUpdate(GameManager.gameMode);
    }
    public static IEnumerator IResetHolderList()
    {
        for (int i = 0; i < NoteHolder.s_holders.Count; i++)
        {
            NoteHolder.s_holders[i].DestroyHolder();
            yield return null;
        }
        NoteHolder.s_holders = new List<NoteHolder>();
        ObjectCooling.s_noteHolders = new List<NoteHolder>();
    }
    public static void ResetZoom()      //$ InputManager SetZero Action
    {
        s_Zoom = 2;
        s_this.UpdateField();
    }
    public static void PageToSelect()   //$ InputManager AltZero Action
    {
        if (EditManager.s_SelectNoteHolder == null) { return; }
        
    }
    
    public static void ZoomIn()
    {
        s_Zoom--;
        s_this.UpdateField();
    }
    public static void ZoomOut()
    {
        s_Zoom++;
        s_this.UpdateField();
    }
    public static void ScrollUp()
    {
        s_Scroll++;
        s_this.UpdateField();
    }
    public static void ScrollDown()
    {
        s_Scroll--;
        s_this.UpdateField();
    }

    public void InputPage(TMP_InputField inputField)
    {
        int value;
        try { value = Convert.ToInt32(inputField.text); }
        catch { value = s_Page; }

        if (value < 1) { value = 1; }
        if (value > 1000) { value = 1000; }
        s_Page = value - 1;
        s_Scroll = 0;

        inputField.text = String.Format("{0:d3}", value);
        UpdateField();
    }
}
