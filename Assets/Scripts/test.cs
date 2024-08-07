using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using System.Threading;

public class test : MonoBehaviour
{
    string[] dataPaths = new string[100];
    float timer = 0f;

    void Start()
    {
        FileToData();
    }
    void Update()
    {
        timer += Time.deltaTime;
    }
    private string ReadFile(int index)
    {
        return index.ToString();
    }

    private async void FileToData()
    {
        await Task.Run(() => 
        {
            for (int i = 0; i < dataPaths.Length - 1; i++)
            {
                ReadFile(i);
            }
        });
    }
}