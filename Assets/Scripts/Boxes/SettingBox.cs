using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SettingBox : MonoBehaviour
{
    private static SettingBox s_this;
    public static bool[] isEnabled = new bool[3] { false, false, false };
    [SerializeField] InputAction action;
    [SerializeField] GameObject[] _SettingObjects;
    private static GameObject[] SettingObjects;
    
    private void Awake()
    {
        s_this = this;
        SettingObjects = _SettingObjects;

        action.performed += item =>
        {
            DisableSetting();
        };
    }
    public static void EnableSettingBox(int index)
    {
        if (index < 0 || index > 2) { return; }

        isEnabled[index] = true;
        SettingObjects[0].SetActive(false);
        SettingObjects[1].SetActive(false);
        SettingObjects[2].SetActive(false);
        SettingObjects[index].SetActive(true);
        
        s_this.EnableAction(false);
        DragSelect.isDraggable = false;
    }
    public static void DisableSetting()
    {
        SettingObjects[0].SetActive(false);
        SettingObjects[1].SetActive(false);
        SettingObjects[2].SetActive(false);

        isEnabled = new bool[3] { false, false, false };

        s_this.EnableAction(true);
        DragSelect.isDraggable = true;
    }
    private void EnableAction(bool isEnable)
    {
        if (isEnable) { action.Disable();}
        else { action.Enable(); }
    }
}
