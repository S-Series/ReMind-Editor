using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

[System.Serializable]
public class NoteHolder : MonoBehaviour
{
    public static List<NoteHolder> holders = new List<NoteHolder>();

    public int stdMs, stdPos;
    public GameNoteHolder gameNoteHolder;

    public NormalNote[] normals = new NormalNote[4] { null, null, null, null };
    public NormalNote[] airials = new NormalNote[4] { null, null, null, null };
    public NormalNote[] bottoms = new NormalNote[2] { null, null };
    public SpeedNote speedNote;
    public EffectNote effectNote;

    [SerializeField] private GameObject[] normalObjects;
    [SerializeField] private GameObject[] airialObjects;
    [SerializeField] private GameObject[] bottomObjects;
    [SerializeField] private GameObject speedObject;
    [SerializeField] private GameObject effectObject;

    [SerializeField] private GameObject[] ParentObjects;

    public void UpdateNote()
    {
        transform.localPosition = new Vector3(0, stdPos * 2, 0);

        for (int i = 0; i < 4; i++)
        {
            if (normals[i] == null) { normalObjects[i].SetActive(false); }
            else
            {
                normalObjects[i].SetActive(true);
                normalObjects[i].TryGetComponent<NoteLength>(out var notelength);
                if (notelength == null) { throw new System.Exception("Notelength Operation is not Exist!"); }
                notelength.Length(normals[i].length);
            }

            if (airials[i] == null) { airialObjects[i].SetActive(false); }
            else { airialObjects[i].SetActive(true); }
        }

        for (int i = 0; i < 2; i++)
        {
            if (bottoms[i] == null) { bottomObjects[i].SetActive(false); }
            else
            {
                bottomObjects[i].SetActive(true);
                bottomObjects[i].TryGetComponent<NoteLength>(out var notelength);
                if (notelength == null) { throw new System.Exception("Notelength Operation is not Exist!"); }
                notelength.Length(bottoms[i].length);
            }
        }

        if (speedNote == null) { speedObject.SetActive(false); }
        else { speedObject.SetActive(true); }

        if (effectNote == null) { effectObject.SetActive(false); }
        else { effectObject.SetActive(true); }

        gameNoteHolder.UpdateNote(this);
    }
    public void UpdateScale()
    {
        transform.localScale = new Vector3(1, 1f / (NoteField.s_Zoom / 10f), 1);
        gameNoteHolder.UpdateScale();
    }
    public void EditMode(bool isTrue)
    {
        foreach (GameObject obj in normalObjects)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                obj.transform.GetChild(i).TryGetComponent<BoxCollider2D>(out var collider);
                if (collider != null) { collider.enabled = isTrue; }

            }
        }
        foreach (GameObject obj in bottomObjects)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                obj.transform.GetChild(i).TryGetComponent<BoxCollider2D>(out var collider);
                if (collider != null) { collider.enabled = isTrue; }
            }
        }
        foreach (GameObject obj in airialObjects) { obj.GetComponent<BoxCollider2D>().enabled = isTrue; }

        speedObject.GetComponent<BoxCollider2D>().enabled = false;
        effectObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void CheckDestroy()
    {
        if (normals == new NormalNote[4] { null, null, null, null}
            && airials == new NormalNote[4] { null, null, null, null}
            && bottoms == new NormalNote[2] { null, null }
            && speedNote == null && effectNote == null)
        {
            NoteField.s_noteHolders.RemoveAll(item => item == this);
            Destroy(gameNoteHolder.gameObject);
            Destroy(this.gameObject);
        }
    }
    public void DestroyHolder()
    {
        foreach(NormalNote note in normals) { NoteClass.s_NormalNotes.RemoveAll(item => item == note); }
        foreach(NormalNote note in airials) { NoteClass.s_NormalNotes.RemoveAll(item => item == note); }
        foreach(NormalNote note in bottoms) { NoteClass.s_NormalNotes.RemoveAll(item => item == note); }
        NoteClass.s_SpeedNotes.RemoveAll(item => item == speedNote);
        NoteClass.s_EffectNotes.RemoveAll(item => item == effectNote);
        NoteClass.InitAll();

        Destroy(gameNoteHolder.gameObject);
        Destroy(this.gameObject);
    }
    public void EnableNote(bool isEnable)
    {
        foreach (GameObject gameObject in ParentObjects) { gameObject.SetActive(isEnable); }
    }   
    
    public GameObject getNormal(int index) { return normalObjects[index]; }
    public GameObject getAirial(int index) { return airialObjects[index]; }
    public GameObject getBottom(int index) { return bottomObjects[index]; }
    public GameObject getSpeed() { return speedObject; }
    public GameObject getEffect() { return effectObject; }
}
