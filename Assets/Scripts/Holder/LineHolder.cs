using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineHolder : MonoBehaviour
{
    [SerializeField] Transform ColliderField;
    
    public TextMeshPro[] texts;
    public List<GuideHolder> holders = new List<GuideHolder>();
}
