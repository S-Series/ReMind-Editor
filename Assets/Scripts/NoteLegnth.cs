using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLegnth : MonoBehaviour
{
    [SerializeField] private Transform[] LongNotes;

    public void Legnth(int legnth)
    {
        if (legnth <= 1)
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

            LongNotes[1].localScale = new Vector3(95, legnth * 10, 95);
            LongNotes[3].localPosition = new Vector3(0, 100 * legnth, 0);
        }
    }

    public Transform GetTransform(bool isSingle)
    {
        if (isSingle) { return LongNotes[0]; }
        else { return LongNotes[1]; }
    }
}
