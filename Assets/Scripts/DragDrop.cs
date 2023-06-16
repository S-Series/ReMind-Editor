using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop : MonoBehaviour
{
    [SerializeField] InputAction inputAction;
    [SerializeField] private Camera dragCamera;
    private Vector3 velocity = Vector3.zero;

    private void OnEnable()
    {
        inputAction.performed += MousePressed;
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.performed -= MousePressed;
        inputAction.Disable();
    }
    public void MousePressed(InputAction.CallbackContext context)
    {
        Ray ray = dragCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
        if (hit2D.collider != null && (hit2D.collider.gameObject.CompareTag("Draggable")))
        {
            StartCoroutine(IDragUpdate(hit2D.collider.gameObject));
        }
    }
    private IEnumerator IDragUpdate(GameObject clicked)
    {
        print(clicked.name);
        Ray ray;
        Vector3 pos;
        float initialDistance = Vector3.Distance(clicked.transform.position, dragCamera.transform.position);
        clicked.TryGetComponent<Rigidbody2D>(out var rb);
        while (inputAction.ReadValue<float>() != 0)
        {
            ray = dragCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (rb != null)
            {
                Vector3 direction = ray.GetPoint(initialDistance) - clicked.transform.position;
                rb.velocity = direction * 0;
                yield return null;
            }
            else
            {
                pos = Vector3.SmoothDamp(clicked.transform.position,
                    ray.GetPoint(initialDistance), ref velocity, 0);
                clicked.transform.position = RetouchVector3(pos);
                EditBox.UpdateRenderer();
                yield return null;
            }
        }
    }
    private Vector3 RetouchVector3(Vector3 value)
    {
        value.x = value.x * 9.9f / value.z;
        value.y = value.y * 9.9f / value.z - 1.05f;
        value.z = 9.9f;
        return value;
    }
}
