using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;

public class VersionManager : MonoBehaviour
{
    private static VersionManager s_this;

    public static int s_Season = 1, s_Release = 0, s_Fatch = 0;
    private const string Web_URL = "https://drive.google.com/uc?export=download&id=";
    private const string Version_ID = "14UUDWP3_2zB8jfPkFvvf1xFEx6Y2UbyJ";
    private const string Download_ID = "1OauaizxjzoaJjhOYetpuElSAMTCZ0_W2";

    private void Awake()
    {
        s_this = (s_this == null) ? this : s_this;
    }
    public static void CheckVersion()
    {
        if (Application.internetReachability 
            == NetworkReachability.NotReachable) 
        {
            Updator.s_this.InternetConnection(false);
        }
        else { s_this.StartCoroutine(IDownloadVersionInfo()); }
    }
    public static void DownloadEXE()
    {
        if (Application.internetReachability 
            == NetworkReachability.NotReachable) 
        {
            Updator.s_this.InternetConnection(false);
        }
        else { s_this.StartCoroutine(IStartDownload()); }
    }
    public static bool isVersionMatch(int[] value)
    {
        if (value[0] != s_Season) { return false; }
        else if (value[1] != s_Release) { return false; }
        else if (value[2] != s_Fatch) { return false; }
        else { return true; }
    }
    public static int[] GetVersion()
    {
        int[] ret;
        ret = new int[3] { s_Season, s_Release, s_Fatch };
        return ret;
    }

    private static IEnumerator IDownloadVersionInfo()
    {
        UnityWebRequest www = UnityWebRequest.Get(Web_URL + Version_ID);
        yield return www.SendWebRequest();

        //$ Download Failed
        if (www.result != UnityWebRequest.Result.Success)
        {

        }
        //$ Download Success
        else
        {
            string textData = www.downloadHandler.text;
            print(textData);
            int[] data = new int[3];
            string[] _data = new string[3];
            _data = textData.Split('.', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < 3; i++) { data[i] = Convert.ToInt32(_data[i]); }
        }
    }
    private static IEnumerator IStartDownload()
    {
        string path, URLs;
        UnityWebRequest www = UnityWebRequest.Get(Web_URL + Download_ID);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            yield break;
        }
        else
        {
            URLs = www.downloadHandler.text;
            print(URLs);
        }

        string[] URL;
        URL = URLs.Split("https://drive.google.com/file/d/", StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < URL.Length; i++)
        {
            URL[i] = URL[i].Replace("\n", String.Empty);
            URL[i] = URL[i].Replace("/view?usp=drive_link", String.Empty);
            print(URL[i]);
        }

        for (int i = 0; i < URL.Length; i++)
        {
            www = UnityWebRequest.Get(Web_URL + URL[i]);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                yield break;
            }
            else
            {
                if (i == 0) { path = Application.dataPath + @"/Updator.zip"; }
                else { path = Application.dataPath + String.Format(@"/Updator.z{0:d2}", i); }
                File.WriteAllBytes(path, www.downloadHandler.data);
                print(path);
            }
        }

        ZipFile.ExtractToDirectory(Application.dataPath + @"/Updator.zip", Application.dataPath);
    }
}
