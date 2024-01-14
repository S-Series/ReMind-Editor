using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragSelect : MonoBehaviour
{
    public static bool isDraggable = true;
    private static bool isDrag = false;
    public static List<GameObject> s_DragSelectObject;

    private bool isShift = false;
    [SerializeField] private InputAction[] inputAction;
    [SerializeField] private InputAction[] shiftAction;
    [SerializeField] private Transform dragObject;
    private LineRenderer lineRenderer;
    private IEnumerator boxCoroutine;

    private Vector3[] posValue = new Vector3[2];

    private void Awake()
    {
        boxCoroutine = IDragBox();
        s_DragSelectObject = new List<GameObject>();
        lineRenderer = dragObject.GetComponent<LineRenderer>();
        
        //$ Mouse Input
        inputAction[0].performed += item =>
        {
            if (!isDraggable) { return; }

            isDrag = true;
            s_DragSelectObject = new List<GameObject>();
            
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.nearClipPlane;
            posValue[0] = RetouceVector(Camera.main.ScreenToWorldPoint(mousePos));

            StartCoroutine(boxCoroutine);
            dragObject.gameObject.SetActive(true);
        };

        //$ Mouse Output
        inputAction[1].performed += item =>
        {
            if (!isDraggable) { return; }
            
            isDrag = false;
            bool isNull;
            isNull = EditManager.s_SelectNoteHolder == null ? true : false;

            StopCoroutine(boxCoroutine);

            if (s_DragSelectObject.Count != 0)
                { EditManager.MultySelect(s_DragSelectObject.ToArray(), !isShift); }
            dragObject.gameObject.SetActive(false);
        };

        shiftAction[0].performed += item => { isShift = true; };
        shiftAction[1].performed += item => { isShift = false; };

        inputAction[0].Enable();
        inputAction[1].Enable();
        shiftAction[0].Enable();
        shiftAction[1].Enable();
    }
    public static void AddObject(GameObject @object)
    {
        if (s_DragSelectObject.Contains(@object)) { return; }
        else { s_DragSelectObject.Add(@object); }
    }
    public static void RemoveObject(GameObject @object)
    {
        s_DragSelectObject.RemoveAll(item => item == @object);
    }
    private IEnumerator IDragBox()
    {
        while (true)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.nearClipPlane;
            posValue[1] = RetouceVector(Camera.main.ScreenToWorldPoint(mousePos));

            dragObject.localPosition = new Vector3(
                Mathf.Lerp(posValue[0].x, posValue[1].x, 0.5f),
                Mathf.Lerp(posValue[0].y, posValue[1].y, 0.5f), 0);
            dragObject.localScale = new Vector3(
                0.155f * (posValue[1].x - posValue[0].x),
                0.155f * (posValue[1].y - posValue[0].y), 1);

            lineRenderer.SetPosition(0, new Vector3(posValue[0].x, posValue[0].y, 10));
            lineRenderer.SetPosition(1, new Vector3(posValue[1].x, posValue[0].y, 10));
            lineRenderer.SetPosition(2, new Vector3(posValue[1].x, posValue[1].y, 10));
            lineRenderer.SetPosition(3, new Vector3(posValue[0].x, posValue[1].y, 10));

            yield return null;
        }
    }
    private Vector3 RetouceVector(Vector3 _value)
    {
        Vector3 ret;
        ret = _value;
        ret.x *= 32.5f;
        ret.y *= 32.5f;
        ret.z = 10;
        return ret;
    }
}
