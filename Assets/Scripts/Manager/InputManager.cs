using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInputSystem inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputSystem();
    }
    private void Start()
    {
        #region //# General Actions
        inputActions.General.SetZero.performed += item =>
        {
            NoteField.ResetZoom();
        };
        inputActions.General.AltZero.performed += item =>
        {
            NoteField.PageToSelect();
        };
        inputActions.General.Save.performed += item =>
        {
            SaveManager.SaveNoteFile();
        };
        #endregion

        #region //# Note Edits Actions
        //# Up Arrow
        inputActions.Edit.Up.performed += item =>
        {
            EditManager.MoveNoteInput(isUp: true);
        };
        //# Down Arrow
        inputActions.Edit.Down.performed += item =>
        {
            EditManager.MoveNoteInput(isUp: false);
        };
        //# Left Arrow
        inputActions.Edit.Left.performed += item =>
        {
            EditManager.LineNoteInput(isLeft: true);
        };
        //# Right Arrow
        inputActions.Edit.Right.performed += item =>
        {
            EditManager.LineNoteInput(isLeft: false);
        };

        //# Left Tab
        inputActions.Edit.Switch.performed += item =>
        {

        };
        //# Delete
        inputActions.Edit.Delete.performed += item =>
        {
            EditManager.Delete();
        };
        //# Esc
        inputActions.Edit.Escape.performed += item =>
        {
            EditManager.Escape();
        };
        #endregion

        #region //# Note Tools Actions
        //# Keycode Q   || 
        inputActions.Tools.NormalNote.performed += item =>
        {
            ToolManager.Tool(0);
        };
        //# Keycode W
        inputActions.Tools.BottomNote.performed += item =>
        {
            ToolManager.Tool(1);
        };
        //# Keycode E
        inputActions.Tools.Airial.performed += item =>
        {
            ToolManager.Tool(2);
        };
        //# Keycode R
        inputActions.Tools.Special.performed += item =>
        {
            ToolManager.Tool(3);
        };
        //# Keycode Tab
        inputActions.Tools.Change.performed += item =>
        {

        };
        inputActions.Tools.Escape.performed += item =>
        {
            NoteGenerate.Escape();
        };
        #endregion
        General(true);
        Editing(false);
    }

    public static void EnableInput(bool isEnable)
    {
        if (isEnable) { inputActions.Enable(); }
        else { inputActions.Disable(); }
    }
    public static void General(bool isEnable)
    {
        if (isEnable) { inputActions.General.Enable(); }
        else { inputActions.General.Disable(); }
    }
    public static void Editing(bool isEnable)
    {
        if (isEnable)
        {
            inputActions.Edit.Enable();
            inputActions.Tools.Disable();
        }
        else
        {
            inputActions.Edit.Disable();
            inputActions.Tools.Enable();
        }
    }
}
