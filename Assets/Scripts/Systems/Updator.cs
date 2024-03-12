using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Updator : MonoBehaviour
{
    public static Updator s_this;
    void Awake()
    {
        if (s_this == null) { s_this = this; }
    }
    public void InternetConnection(bool isSuccess)
    {
        
    }
}
