using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditBox : MonoBehaviour
{
    private static EditBox s_this;
    private static LineRenderer s_lineRenderer = null;
    private static int nowIndex = -1, pos = 0;
    private static Vector3[] vector = new Vector3[3]
        {new Vector3(), new Vector3(), new Vector3()};

    [SerializeField] private GameObject[] editBoxes;
    [SerializeField] private LineRenderer _lineRenderer;

    private void Awake()
    {
        s_this = this;
        s_lineRenderer = _lineRenderer;
    }

    public static void PopUpBox(GameObject gameObject)
    {
        pos = gameObject.GetComponentInParent<NoteHolder>().stdPos;

        foreach (GameObject obj in s_this.editBoxes)
        {
            obj.SetActive(false);
        }

        if (gameObject.transform.parent.CompareTag("Normal")
            || gameObject.transform.parent.CompareTag("Airial"))
        {
            s_this.UpdateBox(1);
        }
        else if (gameObject.transform.parent.CompareTag("Bottom"))
        {
            s_this.UpdateBox(2);
        }
        else
        {
            gameObject.TryGetComponent<SpeedHolder>(out var holder);

            if (holder != null)
            {
                s_this.UpdateBox(3);
            }
            else
            {
                s_this.UpdateBox(4);
            }
        }

        UpdateRenderer();
    }

    public static void Deselect()
    {
        if (nowIndex == -1) { return; }
        foreach (GameObject obj in s_this.editBoxes)
        {
            obj.SetActive(false);
        }
        s_this.editBoxes[0].SetActive(true);
        nowIndex = -1;
        UpdateRenderer();
    }

    private void UpdateBox(int index)
    {
        editBoxes[index].SetActive(true);
        editBoxes[index].GetComponent<NoteChange>().UpdateInfoFields();

        /*Vector3 position;
        position = editBoxes[index].transform.localPosition;
         if (Mathf.Abs(position.x) > 10 || Mathf.Abs(position.y) > 4.5)
        { editBoxes[index].transform.localPosition = new Vector3(-5.75f, 0, 0); }*/

        nowIndex = index;
    }

    public static void UpdateRenderer()
    {
        if (nowIndex == -1) { s_lineRenderer.SetPosition(1, new Vector3(-5.25f, 2.7875f, 10f)); }
        else { s_lineRenderer.SetPosition(1, EditManager.s_SelectedObject.transform.position); }
    }
}
