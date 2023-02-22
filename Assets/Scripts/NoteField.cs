using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class NoteField : MonoBehaviour
{
    public static NoteField s_this;

    public static bool s_isFieldMovable = true;

    [SerializeField] GameObject LinePrefab;
    [SerializeField] Transform DrawField;
    List<LineHolder> holders = new List<LineHolder>();

    [SerializeField] private int scroll = 0;
    [SerializeField] private int Zoom = 10;

    private void Awake()
    {
        s_this = this;

        GameObject _copyObject;
        LineHolder _holder;
        for (int i = 0; i < 999; i++)
        {
            _copyObject = Instantiate(LinePrefab, DrawField, false);
            _copyObject.transform.localPosition = new Vector3(0, 1600 * i, 0);
            _holder = _copyObject.transform.GetComponent<LineHolder>();
            _holder.texts[0].text = string.Format("{0:D3}", i + 1);
            _holder.texts[1].text = string.Format("ms\n{0}", NoteClass.CalMs(1600 * i));
            holders.Add(_holder);
        }
    }

    private void Update()
    {
        if (!s_isFieldMovable) { return; }

        //$ Mouse Up
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Input.GetKey(KeyCode.LeftControl)) { Zoom++; }
            else { scroll--; }
            UpdateField();
        }
        //$ Mouse Down
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Input.GetKey(KeyCode.LeftControl)) { Zoom--; }
            else { scroll++; }
            UpdateField();
        }
        

    }

    public void ResetZoom()
    {
        print("A");
        s_this.Zoom = 10;
        s_this.UpdateField();
    }

    public void UpdateField()
    {
        if (scroll > 0) { scroll = 0; }
        if (scroll < -999 * 5) { scroll = -999 * 5; }

        if (Zoom < 02) { Zoom = 02; }
        if (Zoom > 40) { Zoom = 40; }

        transform.localPosition = new Vector3(0, scroll * (Zoom / 10.0f) , 10);
        DrawField.localScale = new Vector3(0.00312f, Zoom * 0.0003125f, 0.00312f); 
    }
}
