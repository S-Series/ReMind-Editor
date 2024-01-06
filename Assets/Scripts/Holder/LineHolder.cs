using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameNote;

public class LineHolder : MonoBehaviour
{
    public static List<LineHolder> s_holders = new List<LineHolder>();
    public int page;
    public TextMeshPro[] texts;
    public List<GuideHolder> holders = new List<GuideHolder>();

    public void UpdateMs()
    {
        texts[1].text = NoteClass.CalMs(page * 1600).ToString();
    }
    public void UpdateScale()
    {
        transform.localScale = new Vector3(300, 1000 / (10f / NoteField.s_Zoom), 300);
    }
}
