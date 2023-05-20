using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DragSelect : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerMoveHandler
{
    public static bool s_isMultySelect = false;

    [SerializeField] private InputAction inputAction;
    [SerializeField] private Camera dragCamera;
    [SerializeField] private GameObject dragObject;
    private Vector3[] posValue = new Vector3[2];

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 start;
        start = RetouceVector(Input.mousePosition);
        print(start);
        posValue[0] = start;
    }
    public void OnPointerMove(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    private Vector3 RetouceVector(Vector3 _value)
    {
        Vector3 ret;
        ret = _value;
        ret.x = ret.x / 10 * ret.z;
        ret.y = ret.y / 10 * ret.z;
        ret.z = 10;
        return ret;
    }
}
