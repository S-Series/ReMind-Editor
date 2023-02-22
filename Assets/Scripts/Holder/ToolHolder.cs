using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolHolder : MonoBehaviour, IPointerClickHandler
{
    Animator animator;
    private bool isUp = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isUp) { animator.SetTrigger("Down"); }
        else { animator.SetTrigger("Up"); }
        isUp = !isUp;
    }
}
