using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameNote;
using GameData;
using System;

public class NoteHolder : MonoBehaviour
{
    public static List<NoteHolder> s_holders = new List<NoteHolder>();
    public static List<NoteHolder> errorHolders = new List<NoteHolder>();

    private const float lineValue = 0.02632813f;

    public int stdPos, stdEndPos;
    public float stdMs;
    public GameNoteHolder gameNoteHolder;

    public int[][] longMs = 
    { 
        new int[0], new int[0], new int[0],
        new int[0], new int[0], new int[0],
        new int[0], new int[0]
    };
    public NormalNote[] normals = new NormalNote[6] { null, null, null, null, null, null };
    public NormalNote[] airials = new NormalNote[6] { null, null, null, null, null, null };
    public ScratchNote[] bottoms = new ScratchNote[2] { null, null };
    public SpeedNote speedNote;
    public EffectNote effectNote;

    [SerializeField] private GameObject[] normalObjects;
    [SerializeField] private GameObject[] airialObjects;
    [SerializeField] private GameObject[] bottomObjects;
    [SerializeField] private GameObject[] AlertObjects;
    [SerializeField] private GameObject speedObject;
    [SerializeField] private GameObject effectObject;

    [SerializeField] private LineRenderer[] lineRenderers;

    [SerializeField] private GameObject[] ParentObjects;
    [SerializeField] private TextMeshPro[] InfoTmps;

    public static void UpdateVisualPos(float pos)
    {
        foreach (NoteHolder holder in s_holders)
        {
            if (holder.stdPos < pos - 400) { holder.gameObject.SetActive(false); }
            else if (holder.stdPos > pos + 1600 * NoteField.s_Zoom + 400) { holder.EnableNote(false); }
            else { holder.EnableNote(true); }
        }
    }
    public static void GameModeHolderUpdate(GameMode mode)
    {
        foreach (NoteHolder holder in s_holders)
        {
            holder.ApplyGameMode(mode);
        }
    }

    public void UpdateNote()
    {
        transform.localPosition = new Vector3(0, stdPos * 2, 0);

        int LineCount;
        if (GameManager.gameMode == GameMode.Line_4) { LineCount = 4; }
        else if (GameManager.gameMode == GameMode.Line_5) { LineCount = 5; }
        else { throw new Exception("GameMode Error"); }

        for (int i = 0; i < LineCount + 1; i++)
        {
            if (normals[i] == null) { normalObjects[i].SetActive(false); }
            else
            {
                normalObjects[i].SetActive(true);
                normalObjects[i].TryGetComponent<NoteData>(out var normalLength);
                if (normalLength == null) { throw new System.Exception("Notelength Operation is not Exist!"); }
                normalLength.Length(normals[i].length);
            }

            if (airials[i] == null) { airialObjects[i].SetActive(false); }
            else
            {
                airialObjects[i].SetActive(true);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            if (bottoms[i] == null)
            {
                bottomObjects[i].SetActive(false);
                lineRenderers[i].enabled = false;
                continue;
            }
            bottomObjects[i].SetActive(true);
            lineRenderers[i].enabled = true;
            lineRenderers[i].positionCount = 3;
            lineRenderers[i].SetPosition(0, new Vector3(bottoms[i].startX * 4.8f, 0, 0));
            lineRenderers[i].SetPosition(0, new Vector3((bottoms[i].startX + bottoms[i].powerX) * 4.8f, 0, 0));
            lineRenderers[i].SetPosition(0, new Vector3(bottoms[i].endX * 4.8f, bottoms[i].length, 0));
        }

        if (speedNote == null) { speedObject.SetActive(false); }
        else { speedObject.SetActive(true); }

        if (effectNote == null) { effectObject.SetActive(false); }
        else { effectObject.SetActive(true); }

        UpdateTextInfo();
        gameNoteHolder.UpdateNote(this);
    }
    public void UpdateScale()
    {
        transform.localScale = new Vector3(1, 10f / (10f / NoteField.s_Zoom), 1);
        gameNoteHolder.UpdateScale();
    }
    public void NoteSelected(NoteType type, int line)
    {

    }
    public void EnableCollider(bool isTrue)
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

        speedObject.GetComponent<BoxCollider2D>().enabled = isTrue;
        effectObject.GetComponent<BoxCollider2D>().enabled = isTrue;
    }
    
    public void CheckDestroy()
    {
        if (isNull())
        {
            s_holders.RemoveAll(item => item == this);
            Destroy(gameNoteHolder.gameObject);
            Destroy(this.gameObject);
        }
    }
    public void DestroyHolder()
    {
        Destroy(gameNoteHolder.gameObject);
        Destroy(this.gameObject);
    }
    
    public void EnableNote(bool isEnable)
    {
        gameObject.SetActive(isEnable);
    }
    public void UpdateTextInfo()
    {
        InfoTmps[0].text = speedNote == null ? "" : String.Format("{0:F2} X {1:F2} = {2:F2}",
            speedNote.bpm, speedNote.multiple, speedNote.bpm * speedNote.multiple);
        InfoTmps[1].text = effectNote == null ? "" : String.Format("{0} || {1:D4}",
            effectNote.GetEffectName(), effectNote.value);
    }
    public void LineScale(float lengthValue)
    {
        gameNoteHolder.UpdateLineTransform(0, lengthValue);
    }
    public void ApplyLine(int posX, float lineLength)
    {
        
    }

    public int[][] ApplyJudge(NoteType type = NoteType.None, int line = 0)
    {
        int[][] ret;
        ret = new int[][]
        {
            new int[]{0, 0, 0, 0, 0, 0},   /// Normal
            new int[]{0, 0, 0, 0, 0, 0},   //@ Airial
            new int[]{0, 0}             //$ Bottom
        };

        if (type == NoteType.None || line == 0)
        {
            int max;
            max = (int)GameManager.gameMode;
            
            for (int i = 0; i < max; i++)
            {
                if (normals[i] != null) { ret[0][i] = normals[i].length; }
                normalObjects[i].SetActive(false);

                if (airials[i] != null) { ret[1][i] = airials[i].length; }
                airialObjects[i].SetActive(false);

                if (i > 1) { continue; }
                
                if (bottoms[i] != null) { ret[2][i] = bottoms[i].length; }
                bottomObjects[i].SetActive(false);
            }
        }
        else
        {
            if (line < 1) { return ret; }
            if (line > (int)GameManager.gameMode) { return ret; }

            int index;
            index = line - 1;

            switch (type)
            {
                case NoteType.Normal:
                    normalObjects[index].SetActive(false);
                    ret[0][index] = normals[index].length;
                    break;

                case NoteType.Airial:
                    airialObjects[index].SetActive(false);
                    ret[1][index] = airials[index].length;
                    break;

                case NoteType.Scratch:
                    if (index > 1) { break; }
                    airialObjects[index].SetActive(false);
                    ret[2][index] = bottoms[index].length;
                    break;

                default: break;
            }
        }
        return ret;
    }
    public void NoteAlert()
    {

    }
    public void GeneratingInit()
    {
        normals = new NormalNote[] { null, null, null, null, null, null };
        airials = new NormalNote[] { null, null, null, null, null, null };
        bottoms = new ScratchNote[] { null, null };
    }
    public void ApplyGameMode(GameMode mode)
    {
        int lineCount, startPosX;
        float scaleX, posX = 0;
        bool isActive;
        lineCount = (int)mode;
        startPosX = -120 * (lineCount - 1);
        scaleX = Mathf.Pow(0.815f, lineCount - 4);

        for (int i = 0; i < 6; i++)
        {
            isActive = i < lineCount ? true : false;
            posX = scaleX * (startPosX + 240 * i);

            normalObjects[i].transform.localScale = new Vector3(scaleX, 1, 1);
            normalObjects[i].transform.localPosition = new Vector3(posX, 0, 0);
            airialObjects[i].transform.localScale = new Vector3(scaleX * 110, 110, 110);
            airialObjects[i].transform.localPosition = new Vector3(posX, 0, 0);

            normalObjects[i].SetActive(isActive);
            airialObjects[i].SetActive(isActive);
        }

        gameNoteHolder.ApplyGameMode(ints: new int[] { lineCount, startPosX }, mode: mode);
    }
    public int NoteMaxLength(bool isPos = false)
    {
        int ret = 0;
        for (int i = 0; i < 4; i++)
        {
            if (normals[i] != null) if (normals[i].length > ret) ret = normals[i].length;
            if (i > 1) continue;
            if (bottoms[i] != null) if (bottoms[i].length > ret) ret = bottoms[i].length;
        }
        return isPos ? ret * 100 : ret;
    }

    public GameObject getNormal(int index)
    {
        return normalObjects[index];
    }
    public GameObject getAirial(int index)
    {
        return airialObjects[index];
    }
    public GameObject getBottom(int index)
    {
        return bottomObjects[index];
    }
    public GameObject getSpeed()
    {
        return speedObject;
    }
    public GameObject getEffect()
    {
        return effectObject;
    }

    private bool isNull()
    {
        for (int i = 0; i < 6; i++)
        {
            if (normals[i] == null) { return false; }
            if (airials[i] == null) { return false; }
        }
        if (bottoms[0] == null) { return false; }
        if (bottoms[1] == null) { return false; }
        if (speedNote == null) { return false; }
        if (effectNote == null) { return false; }

        return true;
    }

    public NoteHolder(int pos)
    {
        stdPos = pos;
    }
    public NoteHolder(string data)
    {
        string[] holderData, noteData;
        holderData = data.Split('#', StringSplitOptions.RemoveEmptyEntries);

        stdPos = Convert.ToInt32(holderData[0]);

        noteData = holderData[1].Split('|', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < 6; i++)
        {
            normals[i] = new NormalNote(
                values: new int[3]
                {
                    stdPos, i,
                    SaveManager.StringToLength(noteData[i])
                },
                isAirial: false
            );
        }

        noteData = holderData[2].Split('|', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < 6; i++)
        {
            airials[i] = new NormalNote(
                values: new int[3]
                {
                    stdPos, i,
                    SaveManager.StringToLength(noteData[i])
                },
                isAirial: true
            );
        }

        for (int i = 0; i < 2; i++)
        {
            noteData = holderData[i + 3].Split('|', StringSplitOptions.RemoveEmptyEntries);
            bottoms[i] = new ScratchNote(
                _posY: stdPos,
                _length: SaveManager.StringToLength(noteData[0]),
                valueXs: new int[]{
                    Convert.ToInt32(noteData[1]),
                    Convert.ToInt32(noteData[2]),
                    Convert.ToInt32(noteData[3]),
                },
                _isPower: Convert.ToInt32(noteData[0]) > 0 ? true : false
            );
        }

        noteData = holderData[5].Split('|', StringSplitOptions.RemoveEmptyEntries);
        speedNote = new SpeedNote(
            posY: stdPos,
            bpm: Convert.ToInt32(noteData[0]) / 100d,
            multiple: Convert.ToInt32(noteData[1]) / 10000d
        );

        noteData = holderData[6].Split('|', StringSplitOptions.RemoveEmptyEntries);
        effectNote = new EffectNote(
            posY: stdPos,
            index: Convert.ToInt32(noteData[0]),
            value: Convert.ToInt32(noteData[1])
        );
    }
}
