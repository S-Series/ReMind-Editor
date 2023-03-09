using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNoteHolder : MonoBehaviour
{
    [SerializeField] GameObject[] normalObjects;
    [SerializeField] GameObject[] airialObjects;
    [SerializeField] GameObject[] bottomObjects;

    public void UpdateNote(NoteHolder holder)
    {
        transform.localPosition = new Vector3(0, holder.stdPos * 2, 0);

        for (int i = 0; i < 4; i++)
        {
            if (holder.normals[i] == null) { normalObjects[i].SetActive(false); }
            else { normalObjects[i].SetActive(true); }

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
                        lineRenderer.SetPosition(index, new Vector3(240f * (i - j), 0, 350));
                        lineRenderer.SetPosition(index + 1, new Vector3(0, 0, 0));
                        index += 2;
                    }
                }
                if (lineRenderer.positionCount == 1) { lineRenderer.enabled = false; }
            }
        }

        for (int i = 0; i < 2; i++)
        {
            if (holder.bottoms[i] == null) { bottomObjects[i].SetActive(false); }
            else { bottomObjects[i].SetActive(true); }
        }
    }
    public void UpdateScale()
    {
        transform.localScale = new Vector3(1, 2f / (NoteField.s_Zoom / 10f), 1);
    }
}
