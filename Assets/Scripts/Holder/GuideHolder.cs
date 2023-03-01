using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideHolder : MonoBehaviour
{
    public int page = 1, index = 0;

    [SerializeField] SpriteRenderer guideLineRenderer;
    [SerializeField] BoxCollider2D[] guideColliders;

    public void ReSizeCollider(int guideCount, int _page, int _index)
    {
        int pos;
        pos = Mathf.RoundToInt(1600.0f / guideCount * _index);

        transform.localScale = new Vector3(1, 1.0f / guideCount, 1);
        transform.localPosition = new Vector3(0, pos, 0);

        if (pos % 400 == 0) { guideLineRenderer.color = new Color32(255, 255, 255, 255); }
        else { guideLineRenderer.color = new Color32(255, 255, 255, 255); }
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
