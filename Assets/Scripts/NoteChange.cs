using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameNote;

public class NoteChange : MonoBehaviour
{
    private static NoteChange s_this;
    private static readonly string[] noteTag = { "Normal", "Airial", "Bottom" };
    private const string NoteTag = "";

    [SerializeField] TextMeshProUGUI[] InfoTmps;

    [SerializeField] TMP_InputField[] NormalInputs;
    [SerializeField] Toggle[] NormalToggles;

    [SerializeField] TMP_InputField[] FloorInputs;
    [SerializeField] Toggle[] FloorToggles;
    [SerializeField] TMP_Dropdown FloorDropdown;

    [SerializeField] TMP_InputField[] SpeedInputs;
    [SerializeField] TMP_InputField[] EffectInputs;
    [SerializeField] TMP_Dropdown EffectDropdown;

    private void Awake()
    {
        s_this = this;
    }
    public static void UpdateInfoFields()
    {
        if (EditManager.s_isMultyEditing) { return; }

        NoteHolder holder;
        GameObject noteObject;
        holder = EditManager.s_SelectNoteHolder;
        noteObject = EditManager.s_SelectedObject;
        
        if (holder == null || noteObject == null) { return; }

        string selectNoteTag;
        selectNoteTag = noteObject.transform.parent.tag;

        NormalNote normalNote;
        TMP_InputField[] inputFields;

        //$ Normal Note & Airial Note
        if (selectNoteTag == noteTag[0] || selectNoteTag == noteTag[1])
        {
            inputFields = s_this.NormalInputs;
            normalNote = EditManager.s_isAirial
                ? EditManager.s_SelectNoteHolder.airials[EditManager.s_line - 1]
                : EditManager.s_SelectNoteHolder.normals[EditManager.s_line - 1];

            inputFields[0].text = EditManager.s_posY.ToString();
            inputFields[1].text = EditManager.s_page.ToString();
            inputFields[2].text = EditManager.s_length.ToString();
            s_this.NormalToggles[EditManager.s_line - 1].isOn = true;
            s_this.NormalToggles[5].isOn = EditManager.s_isGuideLeft;

            if (EditManager.s_isAirial)
            {
                s_this.InfoTmps[0].text = "Note Heigth";
                s_this.NormalToggles[4].isOn = true;
            }
            else
            {
                s_this.InfoTmps[0].text = "Note Length";
                s_this.NormalToggles[4].isOn = false;
            }
        }
        //$ Floor Note
        else if (selectNoteTag == noteTag[2])
        {
            inputFields = s_this.FloorInputs;
            normalNote = EditManager.s_SelectNoteHolder.bottoms[EditManager.s_line - 1];

            inputFields[0].text = EditManager.s_posY.ToString();
            inputFields[1].text = EditManager.s_page.ToString();
            inputFields[2].text = normalNote.length.ToString();

            s_this.FloorToggles[0].isOn = normalNote.line == 1 ? true : false;
            if (normalNote.SoundIndex == -1)
            {
                s_this.FloorToggles[1].isOn = false;
                s_this.FloorDropdown.interactable = false;
                s_this.FloorDropdown.value = 0;
            }
            else
            {
                s_this.FloorToggles[1].isOn = true;
                s_this.FloorDropdown.interactable = true;
                s_this.FloorDropdown.value = normalNote.SoundIndex;
            }
        }
        else
        {
            //$ Speed Note
            if (noteObject.CompareTag("01"))
            {
                SpeedNote speed;
                speed = EditManager.s_SelectNoteHolder.speedNote;

                inputFields = s_this.SpeedInputs;
                inputFields[0].text = EditManager.s_posY.ToString();
                inputFields[1].text = EditManager.s_page.ToString();
                inputFields[2].text = speed.bpm.ToString();
                inputFields[3].text = speed.multiple.ToString();
                inputFields[4].text = String.Format("{0:F2}", speed.bpm * speed.multiple);
            }
            //$ Effect Note
            else if (noteObject.CompareTag("02"))
            {
                
            }
            //$ System Exception
            else { new Exception("Note Information Update Failed"); }
        }
        InfoField.UpdateInfoField();
    }

    public void InputPos(TMP_InputField input)
    {
        int value;
        try { value = Convert.ToInt32(input.text); }
        catch { input.text = (EditManager.s_SelectNoteHolder.stdPos % 1600).ToString(); return; }
        EditManager.PosNote(value);
    }
    public void InputPage(TMP_InputField input)
    {
        int value;
        try { value = Convert.ToInt32(input.text); }
        catch { input.text = Mathf.FloorToInt(EditManager.s_SelectNoteHolder.stdPos / 1600f).ToString(); return; }
        if (value < 0) { value = 0; input.text = "0"; }
        EditManager.PageNote(value);
    }
    public void InputLegnth(TMP_InputField input)
    {
        int value;
        try { value = Convert.ToInt32(input.text); }
        catch { value = 1; input.text = "1";}
        if (value < 1) { value = 1; input.text = "1"; }
        EditManager.LegnthNote(value);
    }
    public void ToggleLine()
    {
        if (EditManager.s_SelectedObject == null) { return; }
        if (EditManager.s_SelectNoteHolder == null) { return; }

        for (int i = 0; i < 4; i++)
        {
            if (NormalToggles[i].isOn)
            {
                EditManager.LineNote(i + 1);
                break;
            }
        }
    }

    public void ToggleAirial()
    {
        
    }
    public void ToggleGuideColor()
    {

    }
}
