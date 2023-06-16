using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSelectHelper : MonoBehaviour
{
    public void Resize(int x, int y)
    {
        GetComponent<BoxCollider2D>().size = new Vector3(x, y);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        DragSelect.AddObject(EditManager.s_SelectedObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        DragSelect.RemoveObject(EditManager.s_SelectedObject);
    }
}
