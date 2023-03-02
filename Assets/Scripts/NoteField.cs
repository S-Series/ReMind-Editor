using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class NoteField : MonoBehaviour
{
    public static NoteField s_this;

    public static bool s_isFieldMovable = true;
    public static List<LineHolder> s_holders = new List<LineHolder>();
    public static List<MultyNoteHolder> s_multyHolders = new List<MultyNoteHolder>();

    [SerializeField] GameObject LinePrefab;
    [SerializeField] Transform[] DrawField;

    [SerializeField] private int scroll = 0;
    [SerializeField] private int Zoom = 10;

    private void Awake()
    {
        s_this = this;

        GameObject _copyObject;
        LineHolder _holder;
        for (int i = 0; i < 999; i++)
        {
            _copyObject = Instantiate(LinePrefab, DrawField[0], false);
            _copyObject.transform.localPosition = new Vector3(-480.7692f, 1600 * i, 0);

            _holder = _copyObject.transform.GetComponent<LineHolder>();
            _holder.page = i;
            _holder.texts[0].text = string.Format("{0:D3}", i + 1);
            _holder.texts[1].text = string.Format("ms\n{0}", NoteClass.CalMs(1600 * i));

            s_holders.Add(_holder);
        }
    }

    private void Start()
    {
        GuideGenerate.Generate(8);
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

    public MultyNoteHolder FindMultyHolder(NormalNote note)
    {
        MultyNoteHolder ret = null;
        if (note.isAir)
        {
            for (int i = 0; i < s_multyHolders.Count; i++)
            {
                if (s_multyHolders[i].airials.Contains(note))
                {
                    ret = s_multyHolders[i];
                    break;
                }
            }
        }
        else if (note.line > 4)
        {
            for (int i = 0; i < s_multyHolders.Count; i++)
            {
                if (s_multyHolders[i].bottoms.Contains(note))
                {
                    ret = s_multyHolders[i];
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < s_multyHolders.Count; i++)
            {
                if (s_multyHolders[i].normals.Contains(note))
                {
                    ret = s_multyHolders[i];
                    break;
                }
            }
        }
        return ret;
    }
    
    public void ResetZoom()
    {
        print("A");
        s_this.Zoom = 10;
        s_this.UpdateField();
    }

    public void UpdateField()
    {
        Vector3 _pos;
        Vector3 _scale;

        if (scroll > 0) { scroll = 0; }
        if (scroll < -999 * 5) { scroll = -999 * 5; }

        if (Zoom < 02) { Zoom = 02; }
        if (Zoom > 40) { Zoom = 40; }

        _pos = new Vector3(-0.5f, scroll * (Zoom / 10.0f) - 5, 0);
        _scale = new Vector3(0.00312f, Zoom * 0.0003125f, 0.00312f);

        DrawField[0].localScale = _scale;
        DrawField[0].localPosition = _pos;

        DrawField[1].localScale = new Vector3(0.00415f, Zoom * 0.000415f, 0.00415f);
        DrawField[1].localPosition = new Vector3(25, -31.3f, -19 + scroll * (Zoom / 10.0f));

        DrawField[2].localScale = _scale;
        DrawField[2].localPosition = _pos;
    }

    public void AddNote(NormalNote normal = null, SpeedNote speed = null, EffectNote effect = null)
    {
        int _pos;
        MultyNoteHolder targetHolder;

        if (normal != null) 
        {
            _pos = normal.pos;
            targetHolder = s_multyHolders.Find(item => item.stdPos == normal.pos); 
        }
        else if (speed != null) 
        {
            _pos = speed.pos; 
            targetHolder = s_multyHolders.Find(item => item.stdPos == speed.pos); 
        }
        else if (effect != null) 
        {
            _pos = effect.pos;
            targetHolder = s_multyHolders.Find(item => item.stdPos == effect.pos); 
        }
        else { return; }

        if (targetHolder == null)
        {
            targetHolder = new MultyNoteHolder();
            targetHolder.stdMs = NoteClass.CalMs(_pos);
            targetHolder.stdPos = _pos;
            s_multyHolders.Add(targetHolder);
        }

        if (normal != null)
        {

        }
        else if (speed != null)
        {

        }
        else if (effect != null)
        {

        }
    }
}
