using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameNote;
using System.Linq;

public class LineHolder : MonoBehaviour
{
    public static List<LineHolder> s_holders = new List<LineHolder>();
    public static int lastCount = 0;
    public int page;
    [SerializeField] TextMeshPro[] texts;

    public void UpdateMs()
    {
        texts[1].text = NoteClass.PosToMs(page * 1600).ToString();
    }
    public void UpdateScale()
    {
        transform.localScale = new Vector3(1, NoteField.s_Zoom, 1);
    }
    public void EnableHolder(bool isEnable)
    {
        gameObject.SetActive(isEnable);
    }

    public int GetPosValue()
    {
        return (page - 1) * 1600;
    }
}
