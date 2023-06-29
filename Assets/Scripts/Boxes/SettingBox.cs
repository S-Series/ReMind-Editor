using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingBox : MonoBehaviour
{
    [SerializeField] GameObject[] SettingObjects;
    [SerializeField] InputAction[] Actions;
    [SerializeField] OffsetSetting offset;

    private static int offsetMs = 0;
    private static bool isSetting = false;

    private void Awake()
    {
        //# Space
        Actions[0].performed += item =>
        {
            StartCoroutine(IOffsetRunning());
        };
        //# Escape
        Actions[1].performed += item =>
        {
            if (!isSetting) { return; }


        };

        Actions[0].Enable();
        Actions[1].Disable();
    }
    private void FixedUpdate()
    {
        offsetMs++;
    }

    public void EnableSetting(bool isGameSetting)
    {

    }

    private IEnumerator IOffsetRunning()
    {
        float testMs = 0;
        offsetMs = 0;
        while (true)
        {
            testMs += Time.deltaTime * 1000;
            print(string.Format("{0} : {1}", testMs, offsetMs));
            yield return null;
        }
    }
}
