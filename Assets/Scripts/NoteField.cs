using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameNote;

public class NoteField : MonoBehaviour
{
    public static NoteField s_this;

    public static bool s_isFieldMovable = true;
    public static List<LineHolder> s_holders = new List<LineHolder>();
    public static List<NoteHolder> s_noteHolders = new List<NoteHolder>();
    public static int s_Page = 0;
    public static int s_Scroll = 0;
    public static int s_Zoom = 10;
    public static int s_StartPos = 0;
    private static bool s_isCtrl = false;

    [SerializeField] InputAction[] CtrlAction;
    [SerializeField] InputAction ScrollAction;
    [SerializeField] GameObject LinePrefab;
    [SerializeField] Transform[] DrawField;

    [SerializeField] Material[] noteMaterials;

    private void Awake()
    {
        s_this = this;

        GameObject _copyObject;
        LineHolder _holder;
        for (int i = 0; i < 999; i++)
        {
            _copyObject = Instantiate(LinePrefab, DrawField[0], false);
            _copyObject.transform.localPosition = new Vector3(-480.7692f, 1600 * 2 * i, 0);

            _holder = _copyObject.transform.GetComponent<LineHolder>();
            _holder.page = i;
            _holder.texts[0].text = string.Format("{0:D3}", i + 1);
            _holder.texts[1].text = string.Format("ms\n{0}", NoteClass.CalMs(1600 * i));

            s_holders.Add(_holder);
        }
    }
    private void Start()
    {
        CtrlAction[0].performed += item => { s_isCtrl = true; };
        CtrlAction[1].performed += item => { s_isCtrl = false; };
        CtrlAction[0].Enable();
        CtrlAction[1].Enable();

        //$ Mouse Up & Down
        ScrollAction.performed += item =>
        {
            print("run");
            if (!s_isFieldMovable) { return; }
            
            float z;
            z = ScrollAction.ReadValue<float>();
            
            //$ Mouse Scroll Up
            if (z > 0)
            {
                if (s_isCtrl) { s_Zoom++; }
                else { s_Scroll++; }
                UpdateField();
            }
            //$ Mouse Scroll Down
            else if (z < 0)
            {
                if (s_isCtrl) { s_Zoom--; }
                else { s_Scroll--; }
                UpdateField();
            }
        };
        ScrollAction.Enable();
    }

    public NoteHolder FindMultyHolder(NormalNote note)
    {
        NoteHolder ret = null;
        if (note.isAir)
        {
            for (int i = 0; i < s_noteHolders.Count; i++)
            {
                if (s_noteHolders[i].airials.Contains(note))
                {
                    ret = s_noteHolders[i];
                    break;
                }
            }
        }
        else if (note.line > 4)
        {
            for (int i = 0; i < s_noteHolders.Count; i++)
            {
                if (s_noteHolders[i].bottoms.Contains(note))
                {
                    ret = s_noteHolders[i];
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < s_noteHolders.Count; i++)
            {
                if (s_noteHolders[i].normals.Contains(note))
                {
                    ret = s_noteHolders[i];
                    break;
                }
            }
        }
        return ret;
    }
    public void UpdateField()
    {
        Vector3 _pos;
        Vector3 _scale;
        int _count = GuideGenerate.s_guideCount;

        s_StartPos = s_Page * 1600 + Mathf.RoundToInt(1600f / _count * s_Scroll);

        if (s_Scroll < 0) { s_Page--; s_Scroll += _count; }
        while (s_Scroll > _count) { s_Page++; s_Scroll -= _count; }

        if (s_Page < 0) { s_Page = 0; s_Scroll = 0; }
        else if (s_Page > 999) { s_Page = 999; }

        if (s_Zoom < 02) { s_Zoom = 02; }
        else if (s_Zoom > 40) { s_Zoom = 40; }

        _pos = new Vector3(-0.5f, ((s_Page * -10)
            - (10f / GuideGenerate.s_guideCount * s_Scroll)) * s_Zoom / 10 - 5, 0);
        _scale = new Vector3(0.00312f, s_Zoom * 0.0003125f, 0.00312f);

        DrawField[0].localScale = _scale;
        DrawField[0].localPosition = _pos;

        DrawField[1].localScale = new Vector3(0.00415f, s_Zoom * 0.0003125001f, 0.00415f);
        DrawField[1].localPosition = new Vector3(25, -31.3f, ((s_Page * -10)
            - (10f / GuideGenerate.s_guideCount * s_Scroll)) * s_Zoom / 10 - 19);

        DrawField[2].localScale = _scale;
        DrawField[2].localPosition = _pos;

        DrawField[3].localScale = _scale;
        DrawField[3].localPosition = _pos;

        GuideGenerate.UpdateGuideColor();
        GuideGenerate.GuideFieldSize(_scale, s_Zoom / 10.0f);

        foreach (NoteHolder holder in s_noteHolders) { holder.UpdateScale(); }
        foreach (LineHolder holder in s_holders) { holder.UpdateScale(); }

        EditBox.UpdateRenderer();
    }
    public static void SortNoteHolder()
    {
        s_noteHolders.OrderBy(item => item.stdMs);
    }
    public static IEnumerator IResetHolderList()
    {
        for (int i = 0; i < s_noteHolders.Count; i++)
        {
            s_noteHolders[i].DestroyHolder();
            yield return null;
        }
        s_noteHolders = new List<NoteHolder>();
    }
    public static void ResetZoom()      //$ InputManager SetZero Action
    {
        s_Zoom = 10;
        s_this.UpdateField();
    }
    public static void PageToSelect()   //$ InputManager AltZero Action
    {
        if (EditManager.s_SelectNoteHolder == null) { return; }

        
    }

    public static Material GetNoteMaterial(int index)
    {
        try { return s_this.noteMaterials[index]; }
        catch { return null; }
    }
}
