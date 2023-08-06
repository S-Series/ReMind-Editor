using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//# Plugins
using Ookii.Dialogs;
using System.Windows.Forms;

public class test : MonoBehaviour
{
    void Start()
    {
        string fileName = "";

        VistaSaveFileDialog dialog;
        dialog = new VistaSaveFileDialog();
        dialog.Filter = "nd files (*.nd)|*.nd";
        dialog.FilterIndex = 1;
        dialog.Title = "Select Data File";

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            Stream stream;
            if ((stream = dialog.OpenFile()) != null)
            {
                stream.Close();
                fileName = dialog.FileName;
            }
            else { return; }
        }

        if (fileName == null) { return; }

        print(fileName);
    }
}