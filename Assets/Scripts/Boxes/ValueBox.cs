using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
using GameNote;

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
        NoteClass.InitSpeedMs();
        SpectrumManager.GenerateSpectrum(null);
    }
    public void InputDelay()
    {
        int data;
        data = System.Convert.ToInt32(inputFields[1].text);
        if (data < 0)
        {
            data = 0;
            inputFields[1].text = "0";
        }
        ValueManager.s_Delay = data;
        SpectrumManager.UpdateMusicDelay();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBoxOpened) { animator.SetTrigger("Close"); }
        else { animator.SetTrigger("Open"); }
        isBoxOpened = !isBoxOpened;
    }
}
