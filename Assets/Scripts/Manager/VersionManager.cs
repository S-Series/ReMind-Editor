using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : MonoBehaviour
{
    public static int s_Season = 1, s_Release = 0, s_Fatch = 0;

    public static void CheckVersion()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) 
        { 
            return;
        }


    }
}
