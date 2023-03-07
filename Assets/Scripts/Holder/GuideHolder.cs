using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideHolder : MonoBehaviour
{
    public int index = 0;
    public int posY = 0;

    [SerializeField] SpriteRenderer guideLineRenderer;
    [SerializeField] BoxCollider2D[] guideColliders;

    public void ReSizeCollider(int guideCount, int index)
    {
        int pos;
        pos = Mathf.RoundToInt(16.0f / guideCount * index);

        transform.localPosition = new Vector3(0, pos * 2, 0);
        foreach (BoxCollider2D collider in guideColliders)
        {
            collider.size = new Vector2(240, Mathf.RoundToInt(3200.0f / GuideGenerate.s_guideCount));
        }
    }

    public void ReSizeLineRenderer(float invertScale)
    {
        guideLineRenderer.transform.localScale = new Vector3(300, 100 / invertScale, 1);
    }

    public void EnableCollider(bool isEnable)
    {
        foreach(BoxCollider2D collider2D in guideColliders)
        {
            collider2D.enabled = isEnable;
            collider2D.GetComponent<MouseOver>().enabled = isEnable;
        }
    }

    public void UpdateLineColor(int count, int index)
    {
        int _pos;
        _pos = Mathf.RoundToInt(1600f / count * (index + this.index));
        if (_pos % 1600 == 0) { guideLineRenderer.color = new Color32(255, 255, 255, 255); }
        else if (_pos % 400 == 0) { guideLineRenderer.color = new Color32(60, 242, 187, 50); }
        else { guideLineRenderer.color = new Color32(255, 255, 255, 25); }
    }
}
