using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHolder : MonoBehaviour
{
    public int page = 0, index = 0;

    public void ColliderEnable(bool isEnable)
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).GetComponent<MouseOver>().enabled = isEnable;
            transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = isEnable;
        }
    }
}
