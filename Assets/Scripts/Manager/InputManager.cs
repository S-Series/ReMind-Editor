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

        playerInput.actions["UpArrow"].performed += T => UpArrowAction();
        playerInput.actions["DownArrow"].performed += T => DownArrowAction();
        playerInput.actions["RightArrow"].performed += T => RightArrowAction();
        playerInput.actions["LeftArrow"].performed += T => LeftArrowAction();

        playerInput.SwitchCurrentActionMap(ActionMapName[1]);
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
    private void UpArrowAction()
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
    private void RightArrowAction(bool isControl = false, bool isShift = false, bool isAlt = false)
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
}
