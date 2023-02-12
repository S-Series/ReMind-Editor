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
