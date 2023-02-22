using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class SaveLoad : MonoBehaviour
{
    private const string c_path = "";

    public static void SaveData()
    {
        NoteClass.SortAll();
    }
}
