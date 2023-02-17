using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTool : MonoBehaviour
{
    PlayerInputSystem inputActions;

    void Awake()
    {
        inputActions = new PlayerInputSystem();
        inputActions.Tools.Disable();
        inputActions.General.Disable();
        inputActions.Tools.NormalNote.performed += item => OnNormalNote();
    }

    //$ Input Actions
    private void OnNormalNote()
    {
        
    }
}
