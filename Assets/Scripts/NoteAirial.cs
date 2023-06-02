using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteAirial : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    [SerializeField] SpriteRenderer[] renderers;

    public void UpdatePosition(int _value)
    {
        if (_value < 001) { return; }
        if (_value > 100) { return; }
    }
}
