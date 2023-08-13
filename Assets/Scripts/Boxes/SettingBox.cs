using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingBox : MonoBehaviour
{
    public static bool[] isEnabled = new bool[3] { false, false, false };
    [SerializeField] GameObject[] _SettingObjects;
    private static GameObject[] SettingObjects;
    
    private void Awake() { SettingObjects = _SettingObjects; }
    public static void EnableSettingBox(int index)
    {
        if (isEnabled.Contains(true)) { return; }
        if (index < 0 || index > 2) { return; }

        isEnabled[index] = true;
        SettingObjects[index].SetActive(true);
    }
    public static void DisableSetting()
    {
        SettingObjects[0].SetActive(false);
        SettingObjects[1].SetActive(false);
        SettingObjects[2].SetActive(false);
        isEnabled = new bool[3] { false, false, false };
    }
}
