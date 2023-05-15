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

    [SerializeField] TMP_InputField[] NormalInputs;
    [SerializeField] Toggle[] NormalToggles;

    [SerializeField] TMP_InputField[] FloorInputs;
    [SerializeField] Toggle[] FloorToggles;
    [SerializeField] TMP_Dropdown FloorDropdown;

    [SerializeField] TMP_InputField[] SpeedInputs;
    [SerializeField] TMP_InputField[] EffectInputs;

    [SerializeField] 
    private void Awake()
    {
        s_this = this;
    }
    private void Update()
    {   

    }
    public static void UpdateInfoFields()
    {
        NoteHolder holder;
        GameObject noteObject;
        holder = EditManager.s_SelectNoteHolder;
        noteObject = EditManager.s_SelectedObject;
        
        string selectNoteTag;
        selectNoteTag = noteObject.transform.parent.tag;


        if (holder == null || noteObject == null) { return; }

        TMP_InputField[] inputFields;

        print(noteObject.tag);

        if (selectNoteTag == noteTag[0] || selectNoteTag == noteTag[1])
        {
            inputFields = s_this.NormalInputs;

            inputFields[0].text = EditManager.s_posY.ToString();
            inputFields[1].text = EditManager.s_page.ToString();

            s_this.NormalToggles[EditManager.s_line - 1].isOn = true;
            if (EditManager.s_isAirial)
            {
                inputFields[2].text = "- - -";
                s_this.NormalToggles[4].isOn = true;
            }
            else
            {
                inputFields[2].text = EditManager.s_legnth.ToString();
                s_this.NormalToggles[4].isOn = false;
            }
        }
        else if (selectNoteTag == noteTag[2])
        {
            
        }
        else
        {
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
    public void ToggleLine(Toggle toggle, int line)
    {
        if (!toggle.isOn) { return; }

    }
}
