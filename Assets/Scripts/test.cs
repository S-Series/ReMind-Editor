using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using System.Threading;
using TMPro;

public class test : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform transform;
    RectTransform rect;

    int count = 0; 

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void btn()
    {
        GameObject copy;
        copy = Instantiate(prefab, transform, false);
        copy.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100 * count - 60, 0);
        rect.sizeDelta = new Vector2(0, 100.25f * (count + 1) + 14);
        copy.GetComponentInChildren<TextMeshPro>().text = string.Format("No.{0} Object", count);
        count++;
    }
}