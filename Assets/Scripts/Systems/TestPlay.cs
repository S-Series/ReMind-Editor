using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Judge
{
    public enum Judgetype { 
        Perfect = 0,
        Pure = 1,
        Near = 2,
        Lost = 3,
        None = 4
    };
}

public class TestPlay : MonoBehaviour
{
    private static TestPlay s_this;
    public static float s_TestMs = 0.0f;
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
        Transform copyField;
        GameObject copyObject;
        List<NoteHolder> holders;

        NoteField.SortNoteHolder();
        copyField = Instantiate(new GameObject(""), GameNoteGenerateField, false).transform;
        holders = NoteField.s_noteHolders;
        for (int i = 0; i < holders.Count; i++)
        {
            copyObject = Instantiate(GameNoteHolderPrefab, GameNoteGenerateField, false);
        }
    }
}
