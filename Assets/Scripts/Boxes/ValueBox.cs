using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ValueBox : MonoBehaviour, IPointerClickHandler
{
    private static ValueBox s_this;
    private bool isBoxOpened = false;
    private Animator animator;
    [SerializeField] TMP_InputField[] inputFields;

    void Awake()
    {
        s_this = this;
        animator = GetComponent<Animator>();
    }
    public void GetDataFromOrigin()
    {

    }

    public void InputBpm()
    {
        float data;
        data = System.Convert.ToSingle(inputFields[0].text);
        inputFields[0].text = data.ToString();
        ValueManager.s_Bpm = data;
    }
    public void InputDelay()
    {
        int data;
        data = System.Convert.ToInt32(inputFields[1].text);
        ValueManager.s_Delay = data;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBoxOpened) { animator.SetTrigger("Close"); }
        else { animator.SetTrigger("Open"); }
        isBoxOpened = !isBoxOpened;
    }
}
