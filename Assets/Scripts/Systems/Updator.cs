using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Updator : MonoBehaviour
{
    public static void CheckUpdate(int[] result)
    {
        //$ Failed
        if (result[0] == -1)
        {
            if (result[1] == 1)         //# Networking Failed
            {

            }
            else if (result[2] == 1)    //# Downloading Failed
            {

            }
            else
            {
                new System.Exception("Checking Update Error");
            }
            return;
        }
        else
        {

        }
    }

    public void CheckButton()
    {

    }
    public void UpdateButton()
    {

    }
}
