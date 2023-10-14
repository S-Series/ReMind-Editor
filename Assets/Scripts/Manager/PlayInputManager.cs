using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayInputManager : MonoBehaviour
{
    public static PlayInputManager s_PlayInputManager;

    private PlayerInput playerInput;
    private readonly string[] ActionMap 
        = {"Preset01", "Preset02", "Preset03", "Preset04", "User"};
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
    private void Update()
    {
        UpdateInputAction();
    }

    private void SetupInputAction()
    {
        playerInput.DeactivateInput();
        for (int i = 0; i < ActionMap.Length; i++)
        {
            playerInput.SwitchCurrentActionMap(ActionMap[i]);
            for (int j = 0; j < 4; j++)
            {
                playerInput.actions[string.Format("Line A 0{0}", j + 1)].performed
                    += item => { JudgeAction(j + 1); };
                playerInput.actions[string.Format("Line B 0{0}", j + 1)].performed
                    += item => { JudgeAction(j + 1); };
            }
            playerInput.actions["Side - L"].performed += item => { JudgeAction(5); };
            playerInput.actions["Side - R"].performed += item => { JudgeAction(6); };
        }
        playerInput.ActivateInput();
    }
    private void UpdateInputAction()
    {

    }

    private void JudgeAction(int line)
    {
        print(line);
    }
}