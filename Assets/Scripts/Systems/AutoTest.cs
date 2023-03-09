using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTest : MonoBehaviour
{
    private static AutoTest s_this;
    public static int s_Index;
    public static NoteHolder s_TargetHolder;
    public static bool s_isTesting = false;
    private void Update()
    {
        if (!s_isTesting) { return; }

        if (s_TargetHolder.stdMs <= ValueManager.s_testMs)
        {
            JudgeApply(s_TargetHolder);
            
        }
    }
    public static void StartTest()
    {

    }
    private static void JudgeApply(NoteHolder holder)
    {
        
    }
}
