using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public struct LineObject
    {
        public float PosValue { get; set; }
        public float LineValue { get; }
        public Transform LineTransform { get; }
        public LineObject (float x, GameObject y)
        {
            PosValue = x;
            LineValue = x;
            LineTransform = y.transform;
        }
    }
    private static LineGenerator s_this;
    public static List<LineObject> s_LineObjects;
    [SerializeField] GameObject LinePrefab;
    [SerializeField] Transform GenerateField;
    private void Awake()
    {
        if (s_this == null) { s_this = this; }
        s_LineObjects = new List<LineObject>();
        s_LineObjects.Add(new LineObject(0, Instantiate(LinePrefab, GenerateField)));
    }
    public static void AddLineValue(float value)
    {
        if (s_LineObjects.Exists(item => item.LineValue == value)) { return; }
        //else { s_LineObjects.Add(); }

        s_LineObjects.OrderBy(item => item);
    }
    public static void RemoveLineValue(float value)
    {
        //if (!s_LineObjects.Contains(item: value)) { return; }
        //else { s_LineObjects.Remove(item: value); }

        UpdateLineField();
    }
    private static void UpdateLineField()
    {

    }
}
