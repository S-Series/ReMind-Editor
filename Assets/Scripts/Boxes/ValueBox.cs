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

    public void BtnBpm()
    {
        float data;
        try { data = System.Convert.ToSingle(inputFields[0].text); }
        catch { inputFields[0].text = ValueManager.s_Bpm.ToString(); return; }
        data = Mathf.CeilToInt(data * 100) / 100f; //$ 소수점 두자리로 변환
        inputFields[0].text = data.ToString();
        ValueManager.s_Bpm = (double)data;
    }
    public void BtnDelay()
    {
        int data;
        try { data = System.Convert.ToInt32(inputFields[1].text); }
        catch { inputFields[1].text = ValueManager.s_Delay.ToString(); return; }
        ValueManager.s_Delay = data;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBoxOpened) { animator.SetTrigger("Close"); }
        else { animator.SetTrigger("Open"); }
        isBoxOpened = !isBoxOpened;
    }
}
