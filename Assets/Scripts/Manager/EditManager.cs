using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameNote;

public class EditManager : MonoBehaviour
{
    private static EditManager s_this;
    

    #region //$ MultyEditing
    private static int s_stdIndex = -1;
    public static bool s_isMultyEditing = false;
    private static List<NoteHolder> s_MultyHolder;
    private static List<GameObject> s_MultyObject;
    private static List<int> s_MultyPage;
    private static List<int> s_MultyPosY;
    private static List<int> s_MultyLine;
    private static List<int> s_MultyLegnth;
    private static List<int> s_MultySoundIndex;
    private static List<bool> s_MultyAirial;

    private static void ResetMultyEdit()
    {
        s_stdIndex = -1;
        s_isMultyEditing = false;
        s_MultyHolder = new List<NoteHolder>();
        s_MultyObject = new List<GameObject>();
        s_MultyPage = new List<int>();
        s_MultyPosY = new List<int>();
        s_MultyLine = new List<int>();
        s_MultyLegnth = new List<int>();
        s_MultySoundIndex = new List<int>();
        s_MultyAirial = new List<bool>();
    }
    private static void AddMultyNote(GameObject @object)
    {
        if (s_MultyObject.Exists(item => item == @object))
        {
            int index;
            int _count;
            index = s_MultyObject.FindIndex(item => item == @object);
            _count = s_MultyObject[index].transform.childCount;

            if (_count == 0)
            {
                s_MultyObject[index].transform.GetComponent<BoxCollider2D>().enabled = true;
                s_MultyObject[index].transform.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                for (int i = 0; i < _count; i++)
                {
                    s_MultyObject[index].transform.GetChild(i)
                        .TryGetComponent<BoxCollider2D>(out var collider2D);
                    s_MultyObject[index].transform.GetChild(i)
                        .GetComponent<SpriteRenderer>().color = Color.white;
                    if (collider2D != null) { collider2D.enabled = true; }
                }
            }

            s_MultyHolder.RemoveAt(index);
            s_MultyObject.RemoveAt(index);
            s_MultyPage.RemoveAt(index);
            s_MultyPosY.RemoveAt(index);
            s_MultyLine.RemoveAt(index);
            s_MultyLegnth.RemoveAt(index);
            s_MultySoundIndex.RemoveAt(index);
            s_MultyAirial.RemoveAt(index);

            if (index == s_stdIndex)
            {
                int _stdIndex = 0, posValue = 2147483647; //# 2 ^ 31 - 1
                for (int i = 0; i < s_MultyHolder.Count; i++)
                {
                    if (s_MultyHolder[i].stdPos < posValue)
                    {
                        _stdIndex = i;
                        posValue = s_MultyHolder[i].stdPos;
                    }
                }
                s_stdIndex = _stdIndex;
            }
        }
        else
        {
            s_isMultyEditing = true;
            NoteHolder objectHolder;
            int _count;
            _count = @object.transform.childCount;

            if (_count == 0)
            {
                @object.transform.GetComponent<BoxCollider2D>().enabled = false;
                @object.transform.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                for (int i = 0; i < _count; i++)
                {
                    @object.transform.GetChild(i).TryGetComponent<BoxCollider2D>(out var collider2D);
                    @object.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.yellow;
                    if (collider2D != null) { collider2D.enabled = false; }
                }
            }

            objectHolder = @object.GetComponentInParent<NoteHolder>();
            s_MultyObject.Add(@object);
            s_MultyHolder.Add(objectHolder);

            s_MultyPosY.Add(objectHolder.stdPos % 1600);
            s_MultyPage.Add(Mathf.FloorToInt(objectHolder.stdPos / 1600f));

            if (@object.transform.parent.CompareTag(noteTag[0]))
            {
                s_MultyAirial.Add(false);
                s_MultyLine.Add(Convert.ToInt32(@object.tag));
                s_MultyLegnth.Add(objectHolder.normals[s_line - 1].length);
                s_MultySoundIndex.Add(objectHolder.normals[s_line - 1].SoundIndex);
            }
            else if (@object.transform.parent.CompareTag(noteTag[1]))
            {
                s_MultyAirial.Add(false);
                s_MultyLine.Add(Convert.ToInt32(@object.tag));
                s_MultyLegnth.Add(objectHolder.bottoms[s_line - 1].length);
                s_MultySoundIndex.Add(objectHolder.bottoms[s_line - 1].SoundIndex);
            }
            else if (@object.transform.parent.CompareTag(noteTag[2]))
            {
                s_MultyAirial.Add(true);
                s_MultyLine.Add(Convert.ToInt32(@object.tag));
                s_MultyLegnth.Add(0);
                s_MultySoundIndex.Add(objectHolder.airials[s_line - 1].SoundIndex);
            }
            else
            {
                s_MultyAirial.Add(false);
                s_MultyLine.Add(0);
                s_MultyLegnth.Add(0);
                s_MultySoundIndex.Add(0);
            }

            int _stdIndex = 0, posValue = 2147483647; //# 2 ^ 31 - 1
            for (int i = 0; i < s_MultyHolder.Count; i++)
            {
                if (s_MultyHolder[i].stdPos < posValue)
                {
                    _stdIndex = i;
                    posValue = s_MultyHolder[i].stdPos;
                }
            }
            s_stdIndex = _stdIndex;
        }
    }
    private static void MultyEscape()
    {
        int _count;
        for (int i = 0; i < s_MultyObject.Count; i++)
        {
            _count = s_MultyObject[i].transform.childCount;
            if (_count == 0)
            {
                s_MultyObject[i].transform.GetComponent<BoxCollider2D>().enabled = true;
                s_MultyObject[i].transform.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                for (int j = 0; j < _count; j++)
                {
                    s_MultyObject[i].transform.GetChild(j)
                        .TryGetComponent<BoxCollider2D>(out var collider2D);
                    s_MultyObject[i].transform.GetChild(j)
                        .GetComponent<SpriteRenderer>().color = Color.white;
                    if (collider2D != null) { collider2D.enabled = true; }
                }
            }
        }
        ResetMultyEdit();
    }

    #endregion //$ 

    public static NoteHolder s_SelectNoteHolder;
    public static GameObject s_SelectedObject;
    public static int s_page, s_posY, s_line, s_legnth, s_soundIndex;
    public static bool s_isAirial = false;
    private static bool s_shift = false, s_ctrl = false;

    private static readonly string[] noteTag = { "Normal", "Bottom", "Airial" };

    [SerializeField] InputAction[] actions;

    private void Awake()
    {
        s_this = this;
        actions[0].performed += item => { s_shift = true; };
        actions[1].performed += item => { s_shift = false; };
        actions[2].performed += item => { s_ctrl = true; };
        actions[3].performed += item => { s_ctrl = false; };
        ResetMultyEdit();
        InputEnable(true);
    }

    public static void InputEnable(bool isEnable)
    {
        foreach (InputAction action in s_this.actions)
        {
            if (isEnable) { action.Enable(); }
            else { action.Disable(); }
        }
    }
    public static void Select(GameObject obj)
    {
        int _count;
        if (s_SelectedObject != null)
        {
            _count = s_SelectedObject.transform.childCount;
            if (_count == 0)
            {
                s_SelectedObject.transform.GetComponent<BoxCollider2D>().enabled = true;
                if (s_ctrl) { AddMultyNote(s_SelectedObject); }
                else { s_SelectedObject.transform.GetComponent<SpriteRenderer>().color = Color.white; }
            }
            else
            {
                for (int i = 0; i < _count; i++)
                {
                    s_SelectedObject.transform.GetChild(i)
                        .TryGetComponent<BoxCollider2D>(out var collider2D);
                    s_SelectedObject.transform.GetChild(i)
                        .GetComponent<SpriteRenderer>().color = Color.white;
                    if (collider2D != null) { collider2D.enabled = true; }
                }
            }
        }

        _count = obj.transform.childCount;
        if (_count == 0)
        {
            obj.transform.GetComponent<BoxCollider2D>().enabled = false;
            obj.transform.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            for (int i = 0; i < _count; i++)
            {
                obj.transform.GetChild(i).TryGetComponent<BoxCollider2D>(out var collider2D);
                obj.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.yellow;
                if (collider2D != null) { collider2D.enabled = false; }
            }
        }

        s_SelectNoteHolder = obj.GetComponentInParent<NoteHolder>();
        s_SelectedObject = obj;

        s_posY = s_SelectNoteHolder.stdPos % 1600;
        s_page = Mathf.FloorToInt(s_SelectNoteHolder.stdPos / 1600f);

        if (obj.transform.parent.CompareTag(noteTag[0]))
        {
            s_isAirial = false;
            s_line = Convert.ToInt32(obj.tag);
            s_legnth = s_SelectNoteHolder.normals[s_line - 1].length;
            s_soundIndex = s_SelectNoteHolder.normals[s_line - 1].SoundIndex;
        }
        else if (obj.transform.parent.CompareTag(noteTag[1]))
        {
            s_isAirial = false;
            s_line = Convert.ToInt32(obj.tag);
            s_legnth = s_SelectNoteHolder.bottoms[s_line - 1].length;
            s_soundIndex = s_SelectNoteHolder.bottoms[s_line - 1].SoundIndex;
        }
        else if (obj.transform.parent.CompareTag(noteTag[2]))
        {
            s_isAirial = true;
            s_line = Convert.ToInt32(obj.tag);
            s_legnth = 0;
            s_soundIndex = s_SelectNoteHolder.airials[s_line - 1].SoundIndex;
            // obj.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 000, 255);
        }
        else
        {
            s_line = 0;
            s_legnth = 0;
            // obj.GetComponent<SpriteRenderer>().color = new Color32(100, 255, 100, 255);
        }

        InputManager.Editing(true);
        EditBox.PopUpBox(obj);

        int pagePos, startPos, endPos;
        startPos = NoteField.s_StartPos;
        endPos = NoteField.s_StartPos + Mathf.FloorToInt(1600 * (NoteField.s_Zoom / 10f));
        pagePos = s_SelectNoteHolder.stdPos;

        while (pagePos < startPos)
        {
            NoteField.s_Scroll--;
            startPos = NoteField.s_Page * 1600
                + Mathf.RoundToInt(1600f / GuideGenerate.s_guideCount * NoteField.s_Scroll);
        }
        while (pagePos > endPos)
        {
            NoteField.s_Scroll++;
            endPos = NoteField.s_Page * 1600
               + Mathf.RoundToInt(1600f / GuideGenerate.s_guideCount * NoteField.s_Scroll)
               + Mathf.FloorToInt(1600f * (10f / NoteField.s_Zoom));
            print(NoteField.s_Zoom);
        }
        NoteField.s_this.UpdateField();
    }
    public static void MultySelect(GameObject[] objects)
    {
        Escape();
        for (int i = 0; i < objects.Length; i++)
        {
            AddMultyNote(objects[i]);
        }
        Select(s_MultyObject[s_stdIndex]);
    }
    public static void Escape()
    {
        if (s_SelectedObject != null)
        {
            int _count;
            _count = s_SelectedObject.transform.childCount;
            if (_count == 0)
            {
                s_SelectedObject.transform.GetComponent<BoxCollider2D>().enabled = true;
                s_SelectedObject.transform.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                for (int i = 0; i < _count; i++)
                {
                    s_SelectedObject.transform.GetChild(i)
                        .TryGetComponent<BoxCollider2D>(out var collider2D);
                    s_SelectedObject.transform.GetChild(i)
                        .GetComponent<SpriteRenderer>().color = Color.white;
                    if (collider2D != null) { collider2D.enabled = true; }
                }
            }
        }

        foreach (NoteHolder holder in NoteField.s_noteHolders) { holder.EditMode(true); }
        MultyEscape();
        InputManager.Editing(false);
        EditBox.Deselect();
    }
    public static void Delete()
    {
        if (s_SelectNoteHolder == null) { return; }
        string selectNoteTag;
        selectNoteTag = s_SelectedObject.tag;
        if (selectNoteTag == noteTag[0])
        {
            NormalNote note;
            note = s_SelectNoteHolder.normals[s_line - 1];
            NoteClass.s_NormalNotes.RemoveAll(item => item == note);
            s_SelectNoteHolder.normals[s_line - 1] = null;
        }
        else if (selectNoteTag == noteTag[1])
        {
            NormalNote note;
            note = s_SelectNoteHolder.bottoms[s_line - 1];
            NoteClass.s_NormalNotes.RemoveAll(item => item == note);
            s_SelectNoteHolder.bottoms[s_line - 1] = null;
        }
        else if (selectNoteTag == noteTag[2])
        {
            NormalNote note;
            note = s_SelectNoteHolder.airials[s_line - 1];
            NoteClass.s_NormalNotes.RemoveAll(item => item == note);
            s_SelectNoteHolder.airials[s_line - 1] = null;
        }
        else
        {
            s_SelectNoteHolder.TryGetComponent<SpeedNote>(out var speed);
            if (speed != null)
            {
                NoteClass.s_SpeedNotes.RemoveAll(item => item == speed);
                s_SelectNoteHolder.speedNote = null;
            }
            else
            {
                s_SelectNoteHolder.TryGetComponent<EffectNote>(out var effect);
                NoteClass.s_EffectNotes.RemoveAll(item => item == effect);
                s_SelectNoteHolder.effectNote = null;
            }
        }
        NoteClass.SortAll();
        s_SelectNoteHolder.UpdateNote();
        Escape();
    }

    public static void PosNote(int editPos)
    {
        if (s_isMultyEditing) { return; }

        editPos = 1600 * s_page + editPos;

        NoteHolder targetHolder;
        GameObject targetObject;
        targetHolder = NoteField.s_noteHolders.Find(item => item.stdPos == editPos);

        if (targetHolder == null) { targetHolder = NoteGenerate.GenerateNoteManual(editPos); }

        if (s_SelectedObject.transform.parent.CompareTag(noteTag[0]))
        {
            if (targetHolder.normals[s_line - 1] != null)
            { Select(targetHolder.getNormal(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.normals[s_line - 1];

                targetHolder.normals[note.line - 1] = note;
                s_SelectNoteHolder.normals[note.line - 1] = null;
                note.pos = editPos;

                targetObject = targetHolder.getNormal(note.line - 1);
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[1]))
        {
            if (targetHolder.airials[s_line - 1] != null)
            { Select(targetHolder.getAirial(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.airials[Convert.ToInt32(s_SelectedObject.tag) - 1];

                targetHolder.airials[note.line - 1] = note;
                s_SelectNoteHolder.airials[note.line - 1] = null;
                note.pos = editPos;

                targetObject = targetHolder.getAirial(note.line - 1);
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[2]))
        {
            if (targetHolder.bottoms[s_line - 1] != null)
            { Select(targetHolder.getBottom(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

                targetHolder.bottoms[note.line - 5] = note;
                s_SelectNoteHolder.bottoms[note.line - 5] = null;
                note.pos = editPos;

                targetObject = targetHolder.getBottom(note.line - 1);
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }
        else if (s_SelectedObject.CompareTag("01"))
        {
            if (targetHolder.speedNote != null)
            { Select(targetHolder.getSpeed()); }
            else
            {
                SpeedNote note;
                note = s_SelectNoteHolder.speedNote;

                targetHolder.speedNote = note;
                s_SelectNoteHolder.speedNote = null;

                targetObject = targetHolder.getSpeed();
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }
        else
        {
            if (targetHolder.effectNote != null)
            { Select(targetHolder.getEffect()); }
            else
            {
                EffectNote note;
                note = s_SelectNoteHolder.effectNote;

                targetHolder.effectNote = note;
                s_SelectNoteHolder.effectNote = null;

                targetObject = targetHolder.getEffect();
                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetObject);
            }
        }

        targetHolder.UpdateNote();
        s_SelectNoteHolder.UpdateNote();
        s_SelectNoteHolder.CheckDestroy();
    }
    public static void PageNote(int editPage)
    {
        if (s_isMultyEditing) { return; }
        
        int editPos;
        editPos = editPage * 1600 + s_SelectNoteHolder.stdPos % 1600;

        NoteHolder targetHolder;
        targetHolder = NoteField.s_noteHolders.Find(item => item.stdPos == editPos);
        if (targetHolder == null) { targetHolder = NoteGenerate.GenerateNoteManual(editPos); }

        if (s_SelectedObject.transform.parent.CompareTag(noteTag[0]))
        {
            if (targetHolder.normals[s_line - 1] != null)
            { Select(targetHolder.getNormal(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.normals[s_line - 1];

                targetHolder.normals[note.line - 1] = note;
                s_SelectNoteHolder.normals[note.line - 1] = null;
                note.pos = editPos;

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.getNormal(note.line - 1));
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[1]))
        {
            if (targetHolder.airials[s_line - 1] != null)
            { Select(targetHolder.getAirial(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.airials[Convert.ToInt32(s_SelectedObject.tag) - 1];

                targetHolder.airials[note.line - 1] = note;
                s_SelectNoteHolder.airials[note.line - 1] = null;
                note.pos = editPos;

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.getAirial(note.line - 1));
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[2]))
        {
            if (targetHolder.bottoms[s_line - 1] != null)
            { Select(targetHolder.getBottom(s_line - 1)); }
            else
            {
                NormalNote note;
                note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

                targetHolder.bottoms[note.line - 5] = note;
                s_SelectNoteHolder.bottoms[note.line - 5] = null;
                note.pos = editPos;

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.getBottom(note.line - 4 - 1));
            }
        }
        else if (s_SelectedObject.CompareTag("01"))
        {
            if (targetHolder.speedNote != null)
            { Select(targetHolder.getSpeed()); }
            else
            {
                SpeedNote note;
                note = s_SelectNoteHolder.speedNote;

                targetHolder.speedNote = note;
                s_SelectNoteHolder.speedNote = null;
                Select(targetHolder.getSpeed());

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.getSpeed());
            }
        }
        else
        {
            if (targetHolder.effectNote != null)
            { Select(targetHolder.getEffect()); }
            else
            {
                EffectNote note;
                note = s_SelectNoteHolder.effectNote;

                targetHolder.effectNote = note;
                s_SelectNoteHolder.effectNote = null;
                Select(targetHolder.getEffect());

                targetHolder.UpdateNote();
                s_SelectNoteHolder.UpdateNote();
                Select(targetHolder.getEffect());
            }
        }
    }
    public static void LineNote(int editLine)
    {
        if (s_isMultyEditing) { return; }
        
        NormalNote editNormal;

        if (editLine < 1 || editLine > 4) { return; }

        if (s_SelectedObject.transform.parent.CompareTag(noteTag[0]))
        {
            if (s_SelectNoteHolder.normals[editLine - 1] != null)
            { Select(s_SelectNoteHolder.getNormal(editLine - 1)); }
            else
            {
                editNormal = s_SelectNoteHolder.normals[s_line - 1];
                editNormal.line = editLine;

                s_SelectNoteHolder.normals[s_line - 1] = null;
                s_SelectNoteHolder.normals[editLine - 1] = editNormal;

                s_SelectNoteHolder.UpdateNote();
                Select(s_SelectNoteHolder.getNormal(editLine - 1));
            }
        }
        else if (s_SelectedObject.transform.parent.CompareTag(noteTag[1]))
        {
            if (s_SelectNoteHolder.airials[editLine - 1] != null)
            { Select(s_SelectNoteHolder.getAirial(editLine - 1)); }
            else
            {
                editNormal = s_SelectNoteHolder.airials[s_line - 1];
                editNormal.line = editLine;

                s_SelectNoteHolder.airials[s_line - 1] = null;
                s_SelectNoteHolder.airials[editLine - 1] = editNormal;

                s_SelectNoteHolder.UpdateNote();
                Select(s_SelectNoteHolder.getAirial(editLine - 1));
            }
        }
        else
        {
            if (editLine > 2) { return; }

            if (s_SelectNoteHolder.bottoms[editLine - 1] != null)
            { Select(s_SelectNoteHolder.getBottom(editLine - 1)); }
            else
            {
                editNormal = s_SelectNoteHolder.bottoms[s_line - 4 - 1];
                editNormal.line = editLine + 4;

                s_SelectNoteHolder.bottoms[s_line - 4 - 1] = null;
                s_SelectNoteHolder.bottoms[editLine - 1] = editNormal;

                s_SelectNoteHolder.UpdateNote();
                Select(s_SelectNoteHolder.getBottom(editLine - 1));
            }
        }
    }
    public static void LegnthNote(int editLegnth)
    {
        if (s_isMultyEditing) { return; }
       
        if (editLegnth < 1) { editLegnth = 1; }


        if (s_SelectedObject.transform.parent.CompareTag("Normal"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.normals[Convert.ToInt32(s_SelectedObject.tag) - 1];

            note.length = editLegnth;
            s_SelectNoteHolder.UpdateNote();
        }
        else if (s_SelectNoteHolder.transform.parent.CompareTag("Bottom"))
        {
            NormalNote note;
            note = s_SelectNoteHolder.bottoms[Convert.ToInt32(s_SelectedObject.tag) - 1];

            note.length = editLegnth;
            s_SelectNoteHolder.UpdateNote();
        }
        InfoField.UpdateInfoField();
    }

    public static void MoveNoteInput(bool isUp)
    {
        if (s_shift && s_ctrl) { return; }

        //$ Page Movement
        if (s_ctrl)
        {
            s_page = isUp ? s_page + 1 : s_page - 1;

            if (s_page < 0) { s_page = 0; }
            if (s_page > 998) { s_page = 998; }

            PageNote(s_page);
        }
        //$ Legnth Change
        else if (s_shift)
        {
            if (s_legnth == 0) { return; }

            s_legnth = isUp ? s_legnth + 1 : s_legnth - 1;

            if (s_legnth < 01) { s_legnth = 01; }
            if (s_legnth > 99) { s_legnth = 99; }

            LegnthNote(s_legnth);
        }
        //$ Pos Movement
        else
        {
            int indexer;
            int[] copyArr;
            copyArr = GuideGenerate.s_guidePos;

            if (copyArr.Contains(s_posY % 1600))
            {
                indexer = Array.IndexOf(copyArr, s_posY % 1600);
                if (isUp)
                {
                    s_posY = s_posY - copyArr[indexer] + copyArr[indexer + 1];
                    if (s_posY == 1600)
                    {
                        s_page++;
                        s_posY = 0;
                    }
                }
                else
                {
                    if (indexer == 0)
                    {
                        if (s_page == 0) { s_posY = 0; }
                        else
                        {
                            s_page--;
                            s_posY = copyArr[copyArr.Length - 2];
                        }
                    }
                    else { s_posY = s_posY - copyArr[indexer] + copyArr[indexer - 1]; }
                }
            }
            else
            {
                if (isUp)
                {
                    for (int i = 0; i < copyArr.Length; i++)
                    {
                        if (copyArr[i] > s_posY) { s_posY = copyArr[i]; break; }
                    }
                    if (s_posY == 1600)
                    {
                        s_page++;
                        s_posY = 0;
                    }
                }
                else
                {
                    for (int i = 1; i < copyArr.Length; i++)
                    {
                        if (copyArr[i] > s_posY) { s_posY = copyArr[i - 1]; break; }
                    }
                }
            }
            PosNote(s_posY);
        }
        InfoField.UpdateInfoField();
    }
    public static void LineNoteInput(bool isLeft)
    {
        if (s_line == 0) { return; }
        if (s_SelectNoteHolder == null || s_SelectedObject == null) { return; }

        int editLine;
        editLine = s_line;

        if (isLeft) { editLine--; }
        else { editLine++; }

        LineNote(editLine);
        InfoField.UpdateInfoField();
    }
}