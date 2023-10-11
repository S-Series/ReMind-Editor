using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemInfo : MonoBehaviour
{
    [SerializeField] TextMeshPro[] tmps;
    float timer = 0.0f, frameRate;
    public static int noteCount = 0;
    public static bool isSaved = false;

    private void Start()
    {
        StartCoroutine(tmpUpdate());
    }
    private void Update()
    {
        timer += (Time.unscaledDeltaTime - timer) * 0.1f;
        frameRate = 1.0f / timer;
    }
    private IEnumerator tmpUpdate()
    {
        yield return null;

        while (true)
        {
            tmps[0].text = string.Format    //$ Format index 0 to 9
                ("FPS : {0:f1}\nOffset : [ {1:d2} | {2:d2} ]\nMusic Length : [ {3:d2}:{4:d2} ]\nNote Count : [ {5:d4} ]\nSaved : <color={6}</color>\n<size=1>Version : {7}.{8}.{9}"
                , frameRate, ValueManager.s_DrawOffset, ValueManager.s_JudgeOffset          //# 0 1 2
                , Mathf.FloorToInt(MusicLoader.audioSource.clip.length / 60.0f)             //# 3
                , Mathf.FloorToInt(MusicLoader.audioSource.clip.length % 60.0f)             //# 4
                , noteCount, isSaved ? "#09ff00> Saved" : "#FF0000> UnSaved"                //# 5 6
                , VersionManager.s_Season, VersionManager.s_Release, VersionManager.s_Fatch);  //# 7 8 9
            
            tmps[1].text = string.Format
                ("Max FPS : {0}\n\n\n\n\n<size=1>Login : <color={1}</color>"
                , Application.targetFrameRate == -1 ? "Unlimited" : Application.targetFrameRate
                , LoginManager.isLogin ? "#09ff00> " + LoginManager.UserName : "#FF0000> Not Logined");
            yield return null;
        }
    }
}
