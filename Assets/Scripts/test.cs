using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    PlayerInputSystem inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputSystem();
        inputActions.Test.test.performed += item => doSomething();
        inputActions.Test.test1.performed += item => printing();
        inputActions.Enable();
    }

    private void doSomething()
    {

    }

    private void printing()
    {

    }
}
