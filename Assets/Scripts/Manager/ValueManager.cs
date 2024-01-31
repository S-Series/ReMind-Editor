using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueManager : MonoBehaviour
{
    private static ValueManager s_this;
    [SerializeField] TMP_InputField[] ValueInputField;

    //$ UnSave Value
    public static int s_Delay = 0;
    public static float s_Bpm = 120.0f;
    public static bool s_isTest = false;
    public static bool s_isPause = false;

    //$ Save Value
    public static int s_DrawOffset = 0;     //$ [0]
    public static int s_JudgeOffset = 0;    //$ [1]
    public static int s_GameSpeed = 100;    //$ [2]

    private void Awake()
    {
        s_this = this;
    }
    public static void SaveValue()
    {
        UserValue userValue = new UserValue();

        userValue.saveInteger.Add(s_DrawOffset);
        userValue.saveInteger.Add(s_JudgeOffset);
        userValue.saveInteger.Add(s_GameSpeed);

        string path, jsonData;
        path = Application.dataPath + "/_DataBox/" + "_.json";
        jsonData = JsonUtility.ToJson(userValue, true);

        File.WriteAllText(path, jsonData);
    }
    public static void LoadValue()
    {
        UserValue userValue = new UserValue();

        string path, jsonData;
        path = Application.dataPath + "/_DataBox/" + "_.json";
        jsonData = JsonUtility.ToJson(userValue, true);

        if (!File.Exists(path))
        {
            s_DrawOffset = 0;
            s_JudgeOffset = 0;
            s_GameSpeed = 100;
            return;
        }
    }
    public static void UpdateInputField()
    {
        s_this.ValueInputField[0].text = s_Bpm.ToString();
        s_this.ValueInputField[1].text = s_Delay.ToString();
    }

    public void SubmitBpm(float value)
    {
        NoteClass.InitSpeedMs();
        foreach (LineHolder holder in LineHolder.s_holders) { holder.UpdateMs(); }
        SpectrumManager.UpdateSpectrumPos();
    }
    public void Input_Delay()
    {
        try { s_Delay = Convert.ToInt32(ValueInputField[1].text); }
        catch { s_Delay = 0; }
        ValueInputField[1].text = s_Delay.ToString();
    }
}

public class UserValue
{
    public List<int> saveInteger = new List<int>();
    public List<bool> saveBoolean = new List<bool>();
    public List<string> saveString = new List<string>();
    public List<double> saveDouble = new List<double>();
}
