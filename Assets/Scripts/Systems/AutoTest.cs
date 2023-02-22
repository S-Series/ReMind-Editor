using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTest : MonoBehaviour
{
    private static AutoTest s_this;

    public static bool s_isTesting = false;
    public static List<MultyNoteHolder> s_holders;

    private int holderIndex;
    private MultyNoteHolder targetHolder;

    private void Awake()
    {
        s_this = this;
    }

    private void Update()
    {
        if (!s_isTesting) { return; }

        if (targetHolder.stdMs <= ValueManager.s_testMs)
        {
            holderIndex++;
            AutoJudge(targetHolder);

            if (s_holders.Count == holderIndex) { s_isTesting = false; TestEnd(); }
            else { targetHolder = s_holders[holderIndex]; }
        }
    }

    private void AutoJudge(MultyNoteHolder holder)
    {

    }

    private void TestEnd()
    {
        
    }

    private void ResetTest()
    {
        if (s_holders.Count != 0) { targetHolder = s_holders[0]; }
        holderIndex = 0;
    }

    public static void GetHolders(List<MultyNoteHolder> holderList)
    {
        s_holders = holderList;
        s_this.ResetTest();
    }
}
