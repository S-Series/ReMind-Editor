using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragSelect : MonoBehaviour
{
    private static bool isDrag = false;
    public static List<GameObject> s_DragSelectObject;

    private bool isShift = false;
    [SerializeField] private InputAction[] inputAction;
    [SerializeField] private InputAction[] shiftAction;
    [SerializeField] private Camera dragCamera;
    [SerializeField] private Transform dragObject;

    private Vector3[] posValue = new Vector3[2];

    private void Awake()
    {
        inputAction[0].performed += item =>
        {
            isDrag = true;
            if (!isShift) { s_DragSelectObject = new List<GameObject>(); }
            posValue[0] = RetouceVector(Input.mousePosition);
            StartCoroutine(IDragBox());
        };
        inputAction[1].performed += item =>
        {
            isDrag = false;
            StopAllCoroutines();
        };
        shiftAction[0].performed += item => { isShift = true; };
        shiftAction[1].performed += item => { isShift = false; };

        s_DragSelectObject = new List<GameObject>();
    }

    private IEnumerator IDragBox()
    {
        while(true)
        {
            posValue[1] = RetouceVector(Input.mousePosition);

            dragObject.localPosition = new Vector3(
                Mathf.Lerp(posValue[0].x, posValue[1].x, 0.5f),
                Mathf.Lerp(posValue[0].y, posValue[1].y, 0.5f), 0);

            yield return null;
        }
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
