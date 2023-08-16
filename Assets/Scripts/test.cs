using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    float ms, calMs = 0;
    AudioSource source;
    void Start()
    {
        Application.targetFrameRate = 60;
        source = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        calMs += Time.fixedDeltaTime;
    }
    private void Update()
    {
        ms = source.time * 1000f;
        print(calMs + ":" + ms);
    }
}