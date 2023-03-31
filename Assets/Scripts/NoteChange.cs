using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteChange : MonoBehaviour
{
    [SerializeField] TMP_InputField[] inputFields;

    public void UpdateInputFields()
    {
        if (EditManager.s_SelectNoteHolder == null) { return; }

        if (inputFields[0] != null) 
            { inputFields[0].text = (EditManager.s_SelectNoteHolder.stdPos % 1600).ToString(); }
        if (inputFields[1] != null)
            { inputFields[1].text = Mathf.FloorToInt(EditManager.s_SelectNoteHolder.stdPos / 1600f).ToString(); }
    }

    public void BtnPos()
    {
        int pos;
        try { pos = System.Convert.ToInt32(inputFields[0].text); }
        catch
        {
            inputFields[0].text =
                (EditManager.s_SelectNoteHolder.stdPos % 1600).ToString();
            return;
        }
        EditManager.PosNote(pos);
    }
    public void BtnPage()
    {
        int page;
        try { page = System.Convert.ToInt32(inputFields[1].text); }
        catch
        {
            inputFields[1].text = Mathf.FloorToInt
                (EditManager.s_SelectNoteHolder.stdPos / 1600f).ToString();
            return;
        }
        EditManager.PageNote(page);
    }
}
