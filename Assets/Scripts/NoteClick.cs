using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        NoteHolder parent;
        parent = transform.GetComponentInParent<NoteHolder>();
        EditManager.SelectNote(NoteField.s_this
            .FindMultyHolder(parent.noteClass), EditManager.EditType.Normal, parent.noteClass);
    }

    public void ChangeColor(bool isSelected)
    {
        if (isSelected) { GetComponent<SpriteRenderer>().color = new Color32(100, 255, 100, 255); }
        else { GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255); }
    }
}
