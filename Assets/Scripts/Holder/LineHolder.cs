using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineHolder : MonoBehaviour
{
    public int page;
    public TextMeshPro[] texts;
    public List<GuideHolder> holders = new List<GuideHolder>();

    public void UpdateScale()
    {
        transform.localScale = new Vector3(300, 100 / (NoteField.s_Zoom / 10f), 300);
    }
}
