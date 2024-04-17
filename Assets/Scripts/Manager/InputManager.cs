using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager s_this;
    private static PlayerInput playerInput;
    private static readonly string[] ActionMapName = new string[] { 
        "Editing",
        "Testing",
        "Playing"
    };
    private bool isAlt = false, isShift = false, isControl = false;
    private InputAction ScrollInputAction;


    private void Awake()
    {
        if (s_this == null) { s_this = this; }
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        playerInput.SwitchCurrentActionMap(ActionMapName[0]);

        playerInput.actions["Alt"].performed += T => AltAction(true);
        playerInput.actions["AltRelease"].performed += T => AltAction(false);

        playerInput.actions["Shift"].performed += T => ShiftAction(true);
        playerInput.actions["ShiftRelease"].performed += T => ShiftAction(false);

        playerInput.actions["Control"].performed += T => ControlAction(true);
        playerInput.actions["ControlRelease"].performed += T => ControlAction(false);
        
        playerInput.actions["Tab"].performed += T => DeleteAction();
        playerInput.actions["Delete"].performed += T => DeleteAction();

        playerInput.actions["UpArrow"].performed += T => UpArrowAction();
        playerInput.actions["DownArrow"].performed += T => DownArrowAction();
        playerInput.actions["RightArrow"].performed += T => RightArrowAction();
        playerInput.actions["LeftArrow"].performed += T => LeftArrowAction();

        playerInput.actions["QuickTool-1"].performed += T => QuickToolAction(1);
        playerInput.actions["QuickTool-2"].performed += T => QuickToolAction(2);
        playerInput.actions["QuickTool-3"].performed += T => QuickToolAction(3);
        playerInput.actions["QuickTool-4"].performed += T => QuickToolAction(4);
        playerInput.actions["QuickTool-5"].performed += T => QuickToolAction(5);

        playerInput.actions["A"].performed += T => A_Action();
        playerInput.actions["S"].performed += T => S_Action();
        playerInput.actions["C"].performed += T => C_Action();
        playerInput.actions["V"].performed += T => V_Action();

        ScrollInputAction = playerInput.actions["Scroll"];
        ScrollInputAction.performed += T => ScrollAction();

        // playerInput.SwitchCurrentActionMap(ActionMapName[1]);

        playerInput.ActivateInput();
    }

    public static void SwitchInputMap(int actionMapIndex)
    {
        playerInput.SwitchCurrentActionMap(ActionMapName[actionMapIndex]);
    }   

    private void AltAction(bool isInput)
    {
        isAlt = isInput;
    }
    private void ShiftAction(bool isInput)
    {
        isShift = isInput;
    }
    private void ControlAction(bool isInput)
    {
        isControl = isInput;
    }
    
    private void TabAction()
    {

    }
    private void DeleteAction()
    {
        EditManager.Delete();
    }

    private void UpArrowAction()
    {
        if (isControl)
        {
            EditManager.MoveNoteInput(true);
        }
        else if (isShift)
        {
            
        }
        else if (isAlt)
        {

        }
        else
        {
                
        }
    }
    private void DownArrowAction()
    {
        if (isControl)
        {

        }
        else if (isShift)
        {

        }
        else if (isAlt)
        {

        }
        else
        {

        }
    }
    private void LeftArrowAction()
    {
        if (isControl)
        {

        }
        else if (isShift)
        {

        }
        else if (isAlt)
        {

        }
        else
        {

        }
    }
    private void RightArrowAction()
    {
        if (isControl)
        {

        }
        else if (isShift)
        {

        }
        else if (isAlt)
        {

        }
        else
        {

        }
    }

    private void QuickToolAction(int toolIndex)
    {
        print(toolIndex);
    }

    private void A_Action()
    {
        if (isControl)
        {

        }
    }
    private void S_Action()
    {
        if (isControl)
        {
            if (isShift || isAlt)
            {

            }
            else { }
        }
    }
    private void C_Action()
    {
        if (isControl)
        {

        }
    }
    private void V_Action()
    {
        if (isControl)
        {

        }
    }

    private void ScrollAction()
    {
        float z;
        z = ScrollInputAction.ReadValue<float>();

        //$ Mouse Scroll Up
        if (z > 0)
        {
            if (isControl) { NoteField.ZoomIn(); }
            else { NoteField.ScrollUp(); }
        }
        //$ Mouse Scroll Down
        else if (z < 0)
        {
            if (isControl) { NoteField.ZoomOut(); }
            else { NoteField.ScrollDown(); }
        }
    }
}
