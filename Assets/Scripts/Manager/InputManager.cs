using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static PlayerInputSystem inputActions;

    void Awake()
    {
        inputActions = new PlayerInputSystem();
    }
}
