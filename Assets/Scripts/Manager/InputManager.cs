using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static PlayerInputSystem inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputSystem();
    }
    private void Start()
    {
        #region //$ General Actions
        inputActions.General.SetZero.performed += item =>
        {
            NoteField.s_this.ResetZoom();
        };
        #endregion

        #region //$ Note Edits Actions
        //# Up Arrow
        inputActions.Edit.Up.performed += item =>
        {
            // EditManager.s_this.MoveNote(isUp: true);
        };
        //# Down Arrow
        inputActions.Edit.Down.performed += item =>
        {
            // EditManager.s_this.MoveNote(isDown: true);
        };
        //# Left Arrow
        inputActions.Edit.Left.performed += item =>
        {
            // EditManager.s_this.MoveNote(isLeft: true);
        };
        //# Right Arrow
        inputActions.Edit.Right.performed += item =>
        {
            // EditManager.s_this.MoveNote(/*Auto*/);
        };

        //# Left Tab
        inputActions.Edit.Switch.performed += item =>
        {

        };
        //# Delete
        inputActions.Edit.Delete.performed += item =>
        {

        };
        //# Esc
        inputActions.Edit.Escape.performed += item =>
        {
            EditManager.Escape();
        };
        #endregion

        #region //$ Note Tools Actions
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
        inputActions.Tools.Eraser.performed += item =>
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
