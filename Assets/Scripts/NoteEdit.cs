using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class NoteEdit : MonoBehaviour
{
    public static bool isEditing = false;
    public static bool isMultyEditing = false;
    private static NormalNote editNote;
    public static List<NormalNote> notes = new List<NormalNote>();

    PlayerInputSystem inputActions;

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

    public static void NoteSelect(NormalNote note, bool isMultiple = false)
    {
        isEditing = true;
        if (!isMultiple) { notes = new List<NormalNote>(); }
        notes.Add(note);
        if (notes.Count >= 2) { isMultyEditing = true; }
        else { isMultyEditing = false; }

        notes = notes.OrderBy(item => item.ms).ThenBy(item => item.line).ToList();
        editNote = notes[0];
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
