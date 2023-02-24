using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineHolder : MonoBehaviour
{
    [SerializeField] Transform ColliderField;
    
    public TextMeshPro[] texts;
    public List<GuideHolder> holders = new List<GuideHolder>();

    public void InitColliderField()
    {
        for (int i = 0; i < ColliderField.childCount; i++)
        {
            Destroy(ColliderField.GetChild(i).gameObject);
        }
        holders = new List<GuideHolder>();
    }

    public void GenerateCollider(GameObject game, int count)
    {

    }
}
