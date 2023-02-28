using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;

public class ToolManager : MonoBehaviour
{
    public enum NoteType { Null, Normal, Speed, Effect };
    public static NoteType noteType = NoteType.Null;

    public static class EraseManager
    {
        public static bool isErasing = false;
        public static GameObject EraseObject = null;
        public static void MouseOveredObject(GameObject gameObject, NoteType type)
        {
            if (EraseObject == gameObject) { return; }

            //$ Release Note
            switch (noteType)
            {
                case NoteType.Normal:

                    break;

                case NoteType.Speed:

                    break;

                case NoteType.Effect:

                    break;

                default: return;
            }

            //$ Init New Note
            switch (type)
            {
                case NoteType.Normal:

                    break;

                case NoteType.Speed:

                    break;

                case NoteType.Effect:

                    break;

                default: return;
            }
        }
        public static void DeleteObject(GameObject gameObject, NoteType type)
        {
            EraseObject = null;
            noteType = NoteType.Null;

            if (type == NoteType.Normal)
            {
                NoteClass.s_NormalNotes.RemoveAll(item =>
                item == gameObject.GetComponent<NoteHolder>().noteClass);
                Destroy(gameObject.GetComponent<NoteHolder>().linkedHolder.gameObject);
            }
            else if (type == NoteType.Speed)
            {
                NoteClass.s_SpeedNotes.RemoveAll(item =>
                item == gameObject.GetComponent<SpeedHolder>().noteClass);
            }
            else if (type == NoteType.Effect)
            {
                NoteClass.s_EffectNotes.RemoveAll(item =>
                item == gameObject.GetComponent<EffectHolder>().noteClass);
            }

            Destroy(gameObject);
        }
    }

    public void ToolButton(int index)
    {
        NoteGenerate.ChangePreview(index);
    }
}
