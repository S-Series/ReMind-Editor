using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteField : MonoBehaviour
{
    private static NoteField s_this;
    [SerializeField] GameObject LinePrefab;
    List<LineHolder> holders = new List<LineHolder>();

    private void Awake()
    {
        s_this = this;
        GameObject _copyObject;
        LineHolder _holder;
        for (int i = 0; i < 999; i++)
        {
            _copyObject = Instantiate(LinePrefab, this.transform, false);
            _copyObject.transform.localPosition = new Vector3(0, 1600 * i, 0);
            _holder = _copyObject.transform.GetComponent<LineHolder>();
            holders.Add(_holder);
        }
    }

    public static void UpdateField()
    {
        for (int i = 0; i < s_this.holders.Count - 1; i++)
        {

        }
    }
}
