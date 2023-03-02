using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineHolder : MonoBehaviour
{
    [SerializeField] Transform ColliderField;

    public int page;
    public TextMeshPro[] texts;
    public List<GuideHolder> holders = new List<GuideHolder>();

    private void InitColliderField()
    {
        for (int i = 0; i < ColliderField.childCount; i++)
        {
            Destroy(ColliderField.GetChild(i).gameObject);
        }
        holders = new List<GuideHolder>();
    }

    public void GenerateCollider(GameObject game, int count)
    {
        InitColliderField();

        GameObject copyObject;
        GuideHolder copyHolder;

        for (int i = 0; i < holders.Count; i++)
        {
            Destroy(holders[i].gameObject);
        }

        holders = new List<GuideHolder>();

        for (int i = 0; i < count; i++)
        {
            copyObject = Instantiate(game, ColliderField, false);
            copyHolder = copyObject.GetComponent<GuideHolder>();
            holders[i].index = ;
            holders[i].ReSizeCollider(count, i);
            holders.Add(copyHolder);
        }
    }
}
