using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteChange : MonoBehaviour
{
    [SerializeField] TMP_InputField[] noteInputFields;
    [SerializeField] TMP_Dropdown[] noteDropdowns;
    [SerializeField] Toggle[] noteToggles;

    public void UpdateInfoFields()
    {
        if (EditManager.s_SelectNoteHolder == null) { return; }

        NoteHolder holder;
        holder = EditManager.s_SelectNoteHolder;

        //# Pos Value
        noteInputFields[0].text = (EditManager.s_SelectNoteHolder.stdPos % 1600).ToString();
        //# Page Value
        noteInputFields[1].text = Mathf.FloorToInt(EditManager.s_SelectNoteHolder.stdPos / 1600f).ToString();
        //# Legnth Value?
        if (noteInputFields[2] != null) { noteInputFields[2].text = EditManager.s_legnth.ToString(); }
        //# Bpm Value?
        if (noteInputFields[3] != null) { noteInputFields[3].text = holder.speedNote.bpm.ToString(); }
        //# Multiply Value?
        if (noteInputFields[4] != null) { noteInputFields[4].text = holder.speedNote.multiple.ToString(); }
        //# Effect Value?
        if (noteInputFields[5] != null) { noteInputFields[5].text = holder.effectNote.value.ToString(); }

        //# Sound Index?
        if (noteDropdowns[0] != null) { noteDropdowns[0].value = EditManager.s_soundIndex; }
        //# Effect Index?
        if (noteDropdowns[1] != null) { noteDropdowns[1].value = holder.effectNote.effectIndex; }

        if (noteToggles.Length == 5)
        {
            for (int i = 0; i < 4; i++) { noteToggles[i].isOn = false; }

            noteToggles[EditManager.s_line - 1].isOn = true;
            noteToggles[4].isOn = EditManager.s_isAirial;
        }
    }

    public void BtnPos()
    {
        int pos;
        try { pos = System.Convert.ToInt32(noteInputFields[0].text); }
        catch
        {
            noteInputFields[0].text =
                (EditManager.s_SelectNoteHolder.stdPos % 1600).ToString();
            return;
        }
        EditManager.PosNote(pos);
    }
    public void BtnPage()
    {
        int page;
        try { page = System.Convert.ToInt32(noteInputFields[1].text); }
        catch
        {
            noteInputFields[1].text = Mathf.FloorToInt
                (EditManager.s_SelectNoteHolder.stdPos / 1600f).ToString();
            return;
        }
        EditManager.PageNote(page);
    }
}
