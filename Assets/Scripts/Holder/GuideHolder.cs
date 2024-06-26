using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideHolder : MonoBehaviour
{
    public int posY = 0;
    private static Color32[] color32s =
    {
        new Color32(200, 200, 200, 025),    // Normal
        new Color32(255, 000, 255, 025),    // third
        new Color32(000, 255, 185, 025),    // fourth
        new Color32(255, 255, 255, 255)     // BPM Guide
    };

    [SerializeField] SpriteRenderer guideLineRenderer;
    [SerializeField] BoxCollider2D[] guideColliders;

    public void ReSizeCollider(int guideCount, int index)
    {
        int pos;
        pos = Mathf.RoundToInt(16.0f / guideCount * index);

        transform.localPosition = new Vector3(0, pos * 2, 0);
        for (int i = 0; i < guideColliders.Length; i++)
        {
            guideColliders[i].size = new Vector2
                (i == 0 ? 630 : 240, Mathf.RoundToInt(3200.0f / GuideGenerate.s_guideCount));
        }
    }
    public void ReSizeLineRenderer(float invertScale)
    {
        guideLineRenderer.transform.localScale = new Vector3(300, 1000 / invertScale, 1);
    }
    public void EnableCollider(bool isEnable)
    {
        foreach(BoxCollider2D collider2D in guideColliders)
        {
            collider2D.enabled = isEnable;
            collider2D.GetComponent<MouseOver>().enabled = isEnable;
        }
    }
    public void UpdateLineColor(bool third, bool fourth)
    {
        Color32 color32;
        if (third)
        {
            if (fourth) { color32 = color32s[3]; }
            else { color32 = color32s[1]; }
        }
        else if (fourth) { color32 = color32s[2]; }
        else { color32 = color32s[0]; }

        guideLineRenderer.color = color32;
        guideLineRenderer.transform.GetChild(0)
            .GetComponent<SpriteRenderer>().color = color32;
    }
    public void UpdatePosY(int value)
    {
        if (value == posY) {;}
    }
}
