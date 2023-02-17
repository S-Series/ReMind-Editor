using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicHolder : MonoBehaviour, IPointerClickHandler
{
    private static MusicHolder s_this;
    private bool isUp = false;
    [SerializeField] AudioClip clip;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isUp) { GetComponent<Animator>().SetTrigger("PlayDown"); }
        else { GetComponent<Animator>().SetTrigger("PlayUp"); }
        isUp = !isUp;
    }
}
