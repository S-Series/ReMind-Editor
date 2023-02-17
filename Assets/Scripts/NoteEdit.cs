using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class NoteEdit : MonoBehaviour
{
    PlayerInputSystem inputActions;
    private enum EditType { None, Normal, Speed, Effect };
    private static EditType s_editType = EditType.None;
    private static GameObject s_noteObject;
    private static class MultyEdit
    {
        public static bool isAvailable = false;
        public static EditType s_multyType = EditType.None;
        public static List<NormalNote> normals = new List<NormalNote>();
        public static List<SpeedNote> speeds = new List<SpeedNote>();
        public static List<EffectNote> effects = new List<EffectNote>();
        public static void Reset()
        {
            isAvailable = false;
            s_multyType = EditType.None;
            normals = new List<NormalNote>();
            speeds = new List<SpeedNote>();
            effects = new List<EffectNote>();
        }
    }

    private void Awake()
    {
        inputActions = new PlayerInputSystem();
        inputActions.Disable();
        inputActions.General.Up.performed += item => OnUp();
        inputActions.General.Down.performed += item => OnDown();
        inputActions.General.Left.performed += item => OnLeft();
        inputActions.General.Right.performed += item => OnRight();
        inputActions.General.Switch.performed += item => OnSwitch();
        inputActions.General.Escape.performed += item => OnEscape();
        inputActions.General.Delete.performed += item => OnDelete();
    }
    public void NormalNoteSelect(NormalNote note, bool isMulty = false)
    {
        s_editType = EditType.Normal;

        //$ Multy Selecting
        if (isMulty)
        {
            if (MultyEdit.s_multyType == EditType.Normal)
            {
                if (MultyEdit.normals.Exists(item => item == note))
                {
                    MultyEdit.normals.Remove(note);
                }
                else { MultyEdit.normals.Add(note); }
            }
            else
            {
                MultyEdit.Reset();
                MultyEdit.s_multyType = EditType.Normal;
                MultyEdit.normals.Add(note);
            }
            
            MultyEdit.isAvailable = true;
            MultyEdit.normals.OrderBy(item => item.pos).ThenBy(item => item.line);
            s_noteObject = MultyEdit.normals[0].holder.gameObject;
            return;
        }

        //$ Simple Selecting
        
    }
    
    #region //$ Input Actions
    public void OnUp()
    {

    }
    public void OnDown()
    {

    }
    public void OnLeft()
    {

    }
    public void OnRight()
    {

    }
    public void OnEscape()
    {

    }
    public void OnSwitch()
    {

    }
    public void OnDelete()
    {

    }
    #endregion
}
