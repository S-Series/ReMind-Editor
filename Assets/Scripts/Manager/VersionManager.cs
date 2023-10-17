using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VersionManager : MonoBehaviour
{
    private static VersionManager s_this;

    public static int s_Season = 1, s_Release = 0, s_Fatch = 0;
    private const string URL = "https://drive.google.com/uc?export=download&id=14UUDWP3_2zB8jfPkFvvf1xFEx6Y2UbyJ";

    private void Awake()
    {
        s_this = (s_this == null) ? this : s_this;
        CheckVersion();
    }
    public static void CheckVersion()
    {
        if (Application.internetReachability 
            == NetworkReachability.NotReachable) 
        {
            Updator.CheckUpdate(new int[3] { -1, 1, 0 });
        }
        else { s_this.StartCoroutine(s_this.IDownloadVersionInfo()); }
    }

    private IEnumerator IDownloadVersionInfo()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Updator.CheckUpdate(new int[3] {-1, 0, 1 });
        }
        else    //$ Download Success
        {
            string textData = www.downloadHandler.text;
            string[] _data = new string[3];
            int[] data = new int[3];
            _data = (textData.Split('.', StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < 3; i++) { data[i] = Convert.ToInt32(_data[i]); }
        }
    }
}
