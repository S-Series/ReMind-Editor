using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    [SerializeField] InputAction[] InputAction;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            InputAction[i].performed += item => PrintAction(i);
            InputAction[i].Enable();
        }
    }

    private void PrintAction(int data)
    {
        print(String.Format("Data is {0}", data));
    }
}