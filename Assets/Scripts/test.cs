using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        print("Triggered");
    }
}