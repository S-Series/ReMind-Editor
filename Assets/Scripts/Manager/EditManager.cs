using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class EditManager : MonoBehaviour
{
    public static EditManager s_this;

    private enum EditType { None, Normal, Speed, Effect };
    private static EditType s_editType = EditType.None;

    public static List<NormalNote> s_EditNormal = new List<NormalNote>();
    public static List<SpeedNote> s_EditSpeed = new List<SpeedNote>();
    public static List<EffectNote> s_EditEffect = new List<EffectNote>();

    private void Awake()
    {
        s_this = this;
    }

    public static void SelectNote(GameObject gameObject, bool isMulty = false)
    {
        if (!isMulty) { ResetLists(); }

        NormalNote editNormal = null;
        SpeedNote editSpeed = null;
        EffectNote editEffect = null;

        try { editNormal = gameObject.GetComponent<NoteHolder>().noteClass; }
        catch
        {
            try { editSpeed = gameObject.GetComponent<SpeedHolder>().noteClass; }
            catch 
            {
                try { editEffect = gameObject.GetComponent<EffectHolder>().noteClass; }
                catch { throw new System.Exception("Wrong GameObject Sended"); }
            }
        }

        if (editNormal != null) 
        {
            if (s_EditNormal.Contains(editNormal)) { s_EditNormal.Remove(editNormal); }
            else { s_EditNormal.Add(editNormal); }
        }
        else if (editSpeed != null) {; }
        else {; }
    }
    public static void ResetLists()
    {
        s_EditNormal = new List<NormalNote>();
        s_EditSpeed = new List<SpeedNote>();
        s_EditEffect = new List<EffectNote>();
    }
    
    public void MoveNote(bool isUp = false, bool isDown = false, bool isLeft = false)
    {
        if (Input.GetKey(KeyCode.LeftControl)) { LegnthNote(isUp, isDown); }

        int stdPos = 2147483647;
        if (s_EditNormal.Count != 0) { stdPos = s_EditNormal[0].pos; }
        if (s_EditSpeed.Count != 0) { stdPos = stdPos < s_EditSpeed[0].pos ? stdPos : s_EditSpeed[0].pos; }
        if (s_EditEffect.Count != 0) { stdPos = stdPos < s_EditEffect[0].pos ? stdPos : s_EditEffect[0].pos; }
        if (stdPos == 2147483647) { return; }

        if (isUp)
        {

        }
        else if (isDown)
        {

        }
        else if (isLeft)
        {

        }
        else
        {

        }
    }
    private void LegnthNote(bool isUp, bool isDown)
    {
        if (isUp == isDown) { return; }

        if (isUp)
        {

        }
        else
        {

        }
    }
}