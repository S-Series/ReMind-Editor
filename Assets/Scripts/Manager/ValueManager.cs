using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    //$ UnSave Value
    public static int s_delay = 0;
    public static double s_Bpm = 120.0;

    public static bool s_isTest = false;
    public static bool s_isPause = false;

    //$ Save Value
    public static int s_DrawOffset = 0;     //$ [0]
    public static int s_JudgeOffset = 0;    //$ [1]
    public static int s_GameSpeed = 100;    //$ [2]

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
}

public class UserValue
{
    public List<int> saveInteger = new List<int>();
    public List<bool> saveBoolean = new List<bool>();
    public List<string> saveString = new List<string>();
    public List<double> saveDouble = new List<double>();
}

