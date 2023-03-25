using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditBox : MonoBehaviour
{
    private static EditBox s_this;
    private static LineRenderer lineRenderer = null;
    private static int nowIndex = -1, pos = 0;
    private static Vector3[] vector = new Vector3[3]
        {new Vector3(), new Vector3(), new Vector3()};

    [SerializeField] GameObject[] editBoxes;

    private void Awake()
    {
        s_this = this;
    }

    public static void PopUpBox(GameObject gameObject)
    {
        pos = gameObject.GetComponentInParent<NoteHolder>().stdPos;

        foreach (GameObject obj in s_this.editBoxes)
        {
            obj.SetActive(false);
            obj.GetComponent<LineRenderer>().enabled = false;
        }

        if (gameObject.transform.parent.CompareTag("Normal")
            || gameObject.transform.parent.CompareTag("Airial"))
        {
            s_this.UpdateBox(0);
        }
        else if (gameObject.transform.parent.CompareTag("Bottom"))
        {
            s_this.UpdateBox(1);
        }
        else
        {
            gameObject.TryGetComponent<SpeedHolder>(out var holder);

            if (holder != null)
            {
                s_this.UpdateBox(2);
            }
            else
            {
                s_this.UpdateBox(3);
            }
        }

        vector[0] = gameObject.transform.position;
        UpdateRenderer();
    }

    public static void Deselect()
    {
        if (nowIndex == -1) { return; }
        s_this.editBoxes[nowIndex].SetActive(false);
        s_this.editBoxes[nowIndex].GetComponent<LineRenderer>().enabled = false;
        nowIndex = -1;
    }

    private void UpdateBox(int index)
    {
        editBoxes[index].SetActive(true);

        lineRenderer = editBoxes[index].GetComponent<LineRenderer>();
        lineRenderer.enabled = true;

        Vector3 position;
        position = editBoxes[index].transform.localPosition;
        if (Mathf.Abs(position.x) > 10 || Mathf.Abs(position.y) > 4.5)
        { editBoxes[index].transform.localPosition = new Vector3(-3.75f, 0, 0); }

        nowIndex = index;
    }

    public static void UpdateRenderer()
    {
        if (nowIndex == -1) { return; }
        if (lineRenderer == null) { return; }

        vector[1] = s_this.editBoxes[nowIndex].transform.localPosition;
        vector[2] = (vector[0] - vector[1]) / 1.1f;
        vector[2].z = 0;

        lineRenderer.SetPosition(1, vector[2]);
    }
}
