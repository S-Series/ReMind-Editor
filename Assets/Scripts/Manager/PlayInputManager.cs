using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayInputManager : MonoBehaviour
{
    public static PlayInputManager s_PlayInputManager;

    public bool[] Line_A { get; private set; } = new bool[4];
    public bool[] Line_B { get; private set; } = new bool[4];
    public bool[] Side { get; private set; } = new bool[2];

    private PlayerInput _playerInput;

    private InputAction[] A_LineAction = new InputAction[4];
    private InputAction[] B_LineAction = new InputAction[4];
    private InputAction[] SideAction = new InputAction[2];

    private void Awake()
    {
        s_PlayInputManager = this;
        _playerInput = GetComponent<PlayerInput>();
        SetupInputAction();
    }
    private void Update()
    {
        UpdateInputAction();
    }

    private void SetupInputAction()
    {
        for (int i = 0; i < 4; i++)
        {
            A_LineAction[i] = _playerInput.actions[string.Format("Line A 0{0}", i + 1)];
            B_LineAction[i] = _playerInput.actions[string.Format("Line B 0{0}", i + 1)];
        }
        SideAction[0] = _playerInput.actions["Side - L"];
        SideAction[1] = _playerInput.actions["Side - R"];
    }
    private void UpdateInputAction()
    {
        for (int i = 0; i < 4; i++)
        {
            Line_A[i] = A_LineAction[i].WasPressedThisFrame();
            Line_B[i] = B_LineAction[i].WasPressedThisFrame();
        }
        Side[0] = SideAction[0].WasPressedThisFrame();
        Side[1] = SideAction[1].WasPressedThisFrame();
    }
}