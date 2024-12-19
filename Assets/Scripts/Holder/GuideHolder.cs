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

    public void SetPosY(int y)
    {
        transform.localPosition = new Vector3(0, y, 0);

        int[] thirds = new int[2] {
            Mathf.RoundToInt(1600f / 3f),
            Mathf.RoundToInt(1600f / 3f * 2f) 
        };

        if (y == 0) { guideLineRenderer.color = color32s[3]; }
        else if (y % 400 == 0 && ValueManager.isAccent[0]) 
            { guideLineRenderer.color = color32s[2]; }
        else if ((y == thirds[0] || y == thirds[1]) && ValueManager.isAccent[1]) 
            { guideLineRenderer.color = color32s[1]; }
        else { guideLineRenderer.color = color32s[0]; }
    }
    public void SetColliderSize(int count)
    {
        foreach(BoxCollider2D collider2D in guideColliders)
        {
            collider2D.size = new Vector2(240, 1600f / count);
        }
    }
}
