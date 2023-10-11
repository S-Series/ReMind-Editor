using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePreview : MonoBehaviour
{
    [SerializeField] LineRenderer[] _lineRenderers;
    private static LineRenderer[] lineRenderers;
    private static List<NoteHolder> holders;

    private void Awake()
    {
        lineRenderers = _lineRenderers;
        _lineRenderers = null;
    }

    public static void UpdateLineRenderer()
    {

    }
}
