using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideHolder : MonoBehaviour
{
    public int index = 0;

    [SerializeField] SpriteRenderer guideLineRenderer;
    [SerializeField] BoxCollider2D[] guideColliders;

    public void ReSizeCollider(int guideCount, int index)
    {
        int pos;
        pos = Mathf.RoundToInt(16.0f / guideCount * index);

        transform.localScale = new Vector3(1, 1.0f / guideCount, 1);
        transform.localPosition = new Vector3(0, pos, 0);
        print(pos);

        if (pos == 0)
        {
            guideLineRenderer.color = new Color32(255, 255, 255, 255);
            //guideLineRenderer.transform.localScale = new Vector3(1, 1.25f, 1);
        }
        else if (pos % 4 == 0)
        {
            guideLineRenderer.color = new Color32(255, 200, 200, 50);
            //guideLineRenderer.transform.localScale = new Vector3(1, 1.25f, 1);
        }
        else
        {
            guideLineRenderer.color = new Color32(200, 200, 255, 50);
            //guideLineRenderer.transform.localScale = new Vector3(1, 1.25f, 1);
        }
    }

    public void EnableCollider(bool isEnable)
    {
        foreach(BoxCollider2D collider2D in guideColliders)
        {
            collider2D.enabled = isEnable;
            collider2D.GetComponent<MouseOver>().enabled = isEnable;
        }
    }
}
