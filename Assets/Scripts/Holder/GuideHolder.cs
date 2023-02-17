using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideHolder : MonoBehaviour
{
    public int page = 0, indexer = 0;
    [SerializeField] BoxCollider2D[] guideColliders;

    public void ReSizeCollider(int guideCount)
    {
        foreach(BoxCollider2D collider2D in guideColliders)
        {
            collider2D.size = new Vector2(0.8f, 1600f / guideCount);
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
