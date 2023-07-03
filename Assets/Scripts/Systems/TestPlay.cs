using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlay : MonoBehaviour
{
    private static TestPlay s_this;
    private static int s_score, Perfect;
    private static int[] Pures = new int[2], Nears = new int[2], Losts = new int[2];
    private static float s_bpm, s_testMs;
    [SerializeField] Animator ChangeAnimator;
    [SerializeField] Transform GameNoteGenerateField;
    [SerializeField] GameObject GameNoteHolderPrefab;

    private void Awake()
    {
        s_this = this;
    }

    public static void GameMode(bool isTrue)
    {
        
    }

    public void LoadMusicFile()
    {
        GameObject copyObject;
        List<NoteHolder> holders;
        NoteField.SortNoteHolder();
        holders = NoteField.s_noteHolders;
        for (int i = 0; i < holders.Count; i++)
        {
            copyObject = Instantiate(GameNoteHolderPrefab, GameNoteGenerateField, false);
        }
    }
    public static void ResetGamePlay()
    {
        s_score = 0;
        Perfect = 0;
        Pures = new int[2] { 0, 0 };
        Nears = new int[2] { 0, 0 };
        Losts = new int[2] { 0, 0 };
    }
}
