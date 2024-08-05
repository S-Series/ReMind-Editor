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
    string[] dataPaths = new string[100];

    private string ReadFile(int index)
    {
        return File.ReadAllText(dataPaths[index]);
    }

    private IEnumerator IFileToData(string files)
    {
        // DoSomething;
        yield return null;
    }

    IEnumerator IReadFile()
    {
        for (int i = 0; i < dataPaths.Length - 1; i++)
        {
            yield return IFileToData(ReadFile(i));
        }
    }
}