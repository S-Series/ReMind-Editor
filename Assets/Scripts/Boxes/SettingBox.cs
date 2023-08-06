using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingBox : MonoBehaviour
{
    public static bool[] isEnabled = new bool[2] { false, false };
    [SerializeField] GameObject[] _SettingObjects;
    private static GameObject[] SettingObjects;
    
    private void Awake() { SettingObjects = _SettingObjects; }
    public static void EnableSettingBox(int index)
    {
        if (isEnabled[0] || isEnabled[1]) { return; }
        if (index != 0 && index != 1) { return; }

        isEnabled[index] = true;
        SettingObjects[index].SetActive(true);
    }
    public static void Disable()
    {
        SettingObjects[0].SetActive(false);
        SettingObjects[1].SetActive(false);
        isEnabled = new bool[2] { false, false };
    }
}
