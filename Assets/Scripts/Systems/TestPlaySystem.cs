using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameNote;
using GameSystem;

public class TestPlaySystem : MonoBehaviour
{
    private static TestPlaySystem s_this;
    public static List<HolderPackage> holderPackages;
    [SerializeField] TextMeshPro[] InfoTexts;
    [SerializeField] Transform[] GameNoteFields;
    [SerializeField] GameObject GameNoteHolderPrefab;

    private void Awake()
    {
        if (s_this == null) { s_this = this; }
        InfoTexts[0].text = "";
        InfoTexts[1].text = "";
        InfoTexts[2].text = "";
    }
    public static void LoadGameScene()
    {
        List<NoteHolder> holders;
        holders = new List<NoteHolder>();
        holders = NoteField.s_noteHolders;
        holderPackages = new List<HolderPackage>();

        for (int i = 0; i < holders.Count; i++)
        {
            GameObject copyObject;
            GameNoteHolder copyHolder;
            
            copyObject = Instantiate(s_this.GameNoteHolderPrefab, s_this.GameNoteFields[0], false);
            copyHolder = copyObject.GetComponent<GameNoteHolder>();

            copyHolder.UpdateNote(holders[i]);
            holderPackages.Add(new HolderPackage(holders[i], copyHolder));
        }
    }
}
