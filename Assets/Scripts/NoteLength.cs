using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLength : MonoBehaviour
{
    [SerializeField] private bool isGameNote;
    [SerializeField] private Transform[] LongNotes;

    public void Length(int length)
    {
        if (length <= 1)
        {
            LongNotes[0].gameObject.SetActive(true);

            LongNotes[1].gameObject.SetActive(false);
            LongNotes[2].gameObject.SetActive(false);
            LongNotes[3].gameObject.SetActive(false);
        }
        else
        {
            LongNotes[0].gameObject.SetActive(false);

            LongNotes[1].gameObject.SetActive(true);
            LongNotes[2].gameObject.SetActive(true);
            LongNotes[3].gameObject.SetActive(true);

            if (isGameNote)
            {
                if (transform.parent.CompareTag("Normal"))
                {
                    LongNotes[1].localScale = new Vector3(1, length * 0.05683594f, 1);
                    LongNotes[3].localPosition = new Vector3(0, length * 0.5681819f, 0);
                }
                else
                {
                    LongNotes[1].localScale = new Vector3(1, length * 0.05209961f, 1);
                    LongNotes[3].localPosition = new Vector3(0, length * 0.5208334f, 0);
                }

            }
            else
            {
                LongNotes[1].localScale = new Vector3(95, length * 10, 95);
                LongNotes[3].localPosition = new Vector3(0, 100 * length, 0);
            }
        }
    }

    public Transform GetTransform(bool isSingle)
    {
        if (isSingle) { return LongNotes[0]; }
        else { return LongNotes[1]; }
    }
}
