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

        NoteType type;
        NoteHolder holder;
        type = EditManager.s_noteType;
        holder = EditManager.s_SelectNoteHolder;
        
        if (holder == null || type == NoteType.None) { return; }

        NormalNote normalNote;
        TMP_InputField[] inputFields;

        if (type == NoteType.Normal || type == NoteType.Airial)
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
                s_this.InfoTmps[0].text = "Note Heigth(%)";
                s_this.NormalToggles[4].isOn = true;
            }
            else
            {
                s_this.InfoTmps[0].text = "Note Length";
                s_this.NormalToggles[4].isOn = false;
            }
        }
        else if (type == NoteType.Scratch)
        {
            //ToDo : asd;fkljasdf;lkjasdflk;jasdflkj;sadfl;kjasdflk;jsadfl;kj
        }
        else
        {
            if (type == NoteType.Speed)
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
            else if (type == NoteType.Effect)
            {
                
            }
            //$ System Exception
            else { new Exception("Note Information Update Failed"); }
        }
        InfoField.UpdateInfoField();
    }

    public void InputPage(TMP_InputField input)
    {
        int value;
        try { value = Convert.ToInt32(input.text); }
        catch { input.text = Mathf.FloorToInt(EditManager.s_SelectNoteHolder.stdPos / 1600f).ToString(); return; }
        if (value < 0) { value = 0; input.text = "0"; }
        EditManager.EditNote(page: value);
    }
    public void InputPos(TMP_InputField input)
    {
        int value;
        try { value = Convert.ToInt32(input.text); }
        catch { input.text = (EditManager.s_SelectNoteHolder.stdPos % 1600).ToString(); return; }
        EditManager.EditNote(pos: value);
    }
    public void InputLegnth(TMP_InputField input)
    {
        int value;
        try { value = Convert.ToInt32(input.text); }
        catch { value = 1; input.text = "1";}
        print(value);
        if (value < 1) { value = 1; input.text = "1"; }
        EditManager.LengthNote(value);
    }
    
    public void ToggleLine()
    {
        if (EditManager.s_noteType == NoteType.None) { return; }
        if (EditManager.s_SelectNoteHolder == null) { return; }

        for (int i = 0; i < 5; i++)
        {
            if (NormalToggles[i].isOn)
            {
                EditManager.EditNote(line: i + 1);
                break;
            }
        }
    }
    public void ToggleAirial(Toggle toggle)
    {
        if (EditManager.s_isMultyEditing) { return; }

        NoteHolder holder;
        holder = EditManager.s_SelectNoteHolder;
    }
    
    //$ ScratchNote Only
    public void TogglePowered(Toggle toggle)
    {
        bool isOn;
        isOn = toggle.isOn;
        EditManager.EditScratch();
    }
    public void ToggleInversed(Toggle toggle)
    {
        bool isOn;
        isOn = toggle.isOn;
    }
    public void ToggleSlide(Toggle toggle)
    {
        bool isOn;
        isOn = toggle.isOn;
    }
    
    //$ SpeedNote Only
    public void InputBpm(TMP_InputField input)
    {
        float value;
        try {value = Convert.ToSingle(input.text); }
        catch { return; }
        EditManager.BpmNote(value);
    }
    public void InputMultiply(TMP_InputField input)
    {
        float value;
        try {value = Convert.ToSingle(input.text); }
        catch { return; }
        EditManager.MultiplyNote(value);
    }
}
