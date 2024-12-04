using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSequence : MonoBehaviour
{
    public static LoadingSequence s_this;
    public static bool s_isLoading = false;
    private static IEnumerator LoadingCoroutine;

    void Awake()
    {
        if (s_this == null) s_this = this;
        else throw new System.Exception("Single Monotone Error");
        LoadingCoroutine = IStartLoading(null);
    }

    public static void StartLoading(Camera LoadingExitCam)
    {
        s_isLoading = false;
        LoadingCoroutine = IStartLoading(LoadingExitCam);
    }
    private static IEnumerator IStartLoading(Camera cam)
    {
        if (cam == null) { yield break; }

        while (!s_isLoading) { yield return null; }

        
    }
}
