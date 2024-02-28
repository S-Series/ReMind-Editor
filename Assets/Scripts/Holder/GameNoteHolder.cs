using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class GameNoteHolder : MonoBehaviour
{
    private NoteHolder holder = null;
    [SerializeField] GameObject[] normalObjects;
    [SerializeField] GameObject[] airialObjects;
    [SerializeField] GameObject[] bottomObjects;
    [SerializeField] Transform lineTransform;

    public void UpdateNote(NoteHolder noteHolder = null)
    {
        if (noteHolder != null) { holder = noteHolder; }
        if (holder == null) { return; }
        transform.localPosition = new Vector3(0, holder.stdPos * 2, NoteGenerate.InitVec[1].z);

        for (int i = 0; i < 4; i++)
        {
            if (holder.normals[i] == null) { normalObjects[i].SetActive(false); }
            else
            {
                normalObjects[i].SetActive(true);
                normalObjects[i].GetComponent<NoteData>().Length(holder.normals[i].length);
            }

            if (holder.airials[i] == null) { airialObjects[i].SetActive(false); }
            else
            {
                LineRenderer lineRenderer;
                airialObjects[i].SetActive(true);
                lineRenderer = airialObjects[i].GetComponentInChildren<LineRenderer>();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, new Vector3(0, 0, 0) );
                int index = 1;
                for (int j = 0; j < 4; j++)
                {
                    if (holder.normals[j] != null)
                    {
                        lineRenderer.positionCount += 2;
                        lineRenderer.SetPosition(index, 
                            new Vector3(240f * (j - i), 0, 5f * holder.airials[i].length));
                        lineRenderer.SetPosition(index + 1, new Vector3(0, 0, 0));
                        index += 2;
                    }
                }
                if (lineRenderer.positionCount == 1) { lineRenderer.enabled = false; }

                NoteAirial airial;
                airial = airialObjects[i].GetComponent<NoteAirial>();
                airial.UpdatePosition(holder.airials[i].length);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            if (holder.bottoms[i] == null) { bottomObjects[i].SetActive(false); }
            else
            {
                bottomObjects[i].SetActive(true);
                bottomObjects[i].GetComponent<NoteData>().Length(holder.bottoms[i].length);
            }
        }
    }
    public void UpdateScale()
    {
        transform.localScale = new Vector3(1, 10f / (10f / NoteField.s_Zoom), 1);
    }
    public void JudgeVisual(int line = -1)
    {
        if (line == -1)
        {
            /*/$ case 01
            for (int i = 0; i < 4; i++)
            {
                normalObjects[i].SetActive(false);
                airialObjects[i].SetActive(false);
                if (i > 1) { continue; }
                bottomObjects[i].SetActive(false);
            }
            */
            //$ caes 02
            for (int i = 0; i < 4; i++)
            {
                normalObjects[i].SetActive(false);
                airialObjects[i].SetActive(false);
                if (i < 2) { bottomObjects[i].SetActive(false); }
            }
        }
        else
        {
            if (line < 0) { throw new System.Exception("Judge Line Error (line < 0)"); }
            else if (line < 4) { normalObjects[line].SetActive(false); }
            else if (line < 6) { bottomObjects[line - 4].SetActive(false); }
            else if (line < 10) { airialObjects[line - 6].SetActive(false); }
            else { throw new System.Exception("Judge Line Error (line >= 10)"); }
        }
    }
    public void ApplyGameMode(int[] ints, GameMode mode)
    {
        float posX;
        bool isActive;
        Vector3 vec3;

        for (int i = 0; i < 6; i++)
        {
            isActive = i < ints[0] ? true : false;
            posX = ints[1] + 240 * i;
            normalObjects[i].transform.localPosition = new Vector3(posX, 0, 0);
            airialObjects[i].transform.localPosition = new Vector3(posX, 0, 0);
            normalObjects[i].SetActive(isActive);
            airialObjects[i].SetActive(isActive);
        }

        vec3 = new Vector3(96f / Mathf.Pow(0.815f, (int)mode - 4), 96f, 96f);
        bottomObjects[0].transform.localScale = vec3;
        bottomObjects[1].transform.localScale = vec3;
    }
    public void UpdateLineTransform(float posX, float lengthValue)
    {
        
        lineTransform.localScale = new Vector3(80.72289f, 0.02632963f * lengthValue, 240.9639f);
    }
}
