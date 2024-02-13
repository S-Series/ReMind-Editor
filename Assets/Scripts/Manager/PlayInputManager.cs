using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayInputManager : MonoBehaviour
{
    public static PlayInputManager s_PlayInputManager;
    public static bool s_isRebindable = false;

    private PlayerInput playerInput;
    private readonly string[] ActionMap = { "Preset01", "Preset02", "User" };
    [SerializeField] InputPlay test;

    private void Awake()
    {
        s_PlayInputManager = this;
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        SetupInputAction();
    }
    private void SetupInputAction()
    {
        playerInput.DeactivateInput();
        for (int i = 0; i < ActionMap.Length; i++)
        {
            playerInput.actions["Type A-01"].performed += item => { JudgeAction(1); };
            playerInput.actions["Type B-01"].performed += item => { JudgeAction(1); };
            playerInput.actions["Type A-02"].performed += item => { JudgeAction(2); };
            playerInput.actions["Type B-02"].performed += item => { JudgeAction(2); };
            playerInput.actions["Type A-03"].performed += item => { JudgeAction(3); };
            playerInput.actions["Type B-03"].performed += item => { JudgeAction(3); };
            playerInput.actions["Type A-04"].performed += item => { JudgeAction(4); };
            playerInput.actions["Type B-04"].performed += item => { JudgeAction(4); };
            playerInput.actions["Type A-05"].performed += item => { JudgeAction(5); };
            playerInput.actions["Type B-05"].performed += item => { JudgeAction(5); };
            playerInput.actions["Side - L"].performed += item => { JudgeAction(-1); };
            playerInput.actions["Side - R"].performed += item => { JudgeAction(-2); };
        }
        playerInput.SwitchCurrentActionMap(ActionMap[0]);
        playerInput.ActivateInput();
    }
    private void JudgeAction(int line)
    {
        print(line);
    }
    public void DropdownPreset(TMP_Dropdown dropdown)
    {
        int index;
        index = dropdown.value;
        playerInput.DeactivateInput();
        playerInput.SwitchCurrentActionMap(ActionMap[index]);
        playerInput.ActivateInput();
        s_isRebindable = ActionMap[index] == "User" ? true : false;
    }
}