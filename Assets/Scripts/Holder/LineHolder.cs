using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineHolder : MonoBehaviour
{
    [SerializeField] TextMeshPro[] texts;
    [SerializeField] Transform ColliderField;
    public List<GuideHolder> holders = new List<GuideHolder>();
}
