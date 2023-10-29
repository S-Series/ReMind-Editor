using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteAirial : MonoBehaviour
{
    [SerializeField] GameObject Shadow;
    [SerializeField] LineRenderer renderers;

    public void UpdatePosition(int _value)
    {
        float posX = transform.localPosition.x;
        transform.localPosition = new Vector3(posX, 0, -3.6f * _value);
        Shadow.transform.localPosition = new Vector3(0, 0, 3.6f * _value);
    }
}
