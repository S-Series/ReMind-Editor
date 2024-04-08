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
    private static IEnumerator[] ScrollCoroutine = new IEnumerator[3];

    private void Awake()
    {
        if (s_this == null) { s_this = this; }
        playerInput = GetComponent<PlayerInput>();

        ScrollCoroutine[0] = IScroll_Edit();
        ScrollCoroutine[1] = IScroll_Test();
        ScrollCoroutine[2] = IScroll_Play();
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

        playerInput.actions["Quick Tool-1"].performed += T => QuickToolAction(1);
        playerInput.actions["Quick Tool-2"].performed += T => QuickToolAction(2);
        playerInput.actions["Quick Tool-3"].performed += T => QuickToolAction(3);
        playerInput.actions["Quick Tool-4"].performed += T => QuickToolAction(4);
        playerInput.actions["Quick Tool-5"].performed += T => QuickToolAction(5);

        playerInput.actions["A"].performed += T => A_Action();
        playerInput.actions["S"].performed += T => S_Action();
        playerInput.actions["C"].performed += T => C_Action();
        playerInput.actions["V"].performed += T => V_Action();


        // playerInput.SwitchCurrentActionMap(ActionMapName[1]);
    }

    public static void SwitchInputMap(int actionMapIndex)
    {
        s_this.StopAllCoroutines();
        playerInput.DeactivateInput();

        playerInput.SwitchCurrentActionMap(ActionMapName[actionMapIndex]);

        playerInput.ActivateInput();
        s_this.StartCoroutine(ScrollCoroutine[actionMapIndex]);
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

    }

    private void A_Action()
    {

    }
    private void S_Action()
    {

    }
    private void C_Action()
    {

    }
    private void V_Action()
    {

    }

    private void ScrollAction_Edit(bool isPositive)
    {

    }
    private void ScrollAction_Test(bool isPositive)
    {

    }
    private void ScrollAction_Play(bool isPositive)
    {

    }

    private IEnumerator IScroll_Edit()
    {
        var ScrollVector = playerInput.actions["Scroll"]?.ReadValue<Vector2>().y;
        while (true)
        {
            if (ScrollVector >= 1)
            {

            }
            else if (ScrollVector <= -1)
            {

            }
            yield return null;
        }
    }
    private IEnumerator IScroll_Test()
    {
        while (true)
        {
            yield return null;
        }
    }
    private IEnumerator IScroll_Play()
    {
        while (true)
        {
            yield return null;
        }
    }
}
