using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;
using GameSystem;

public class JudgeSystem : MonoBehaviour
{
    [SerializeField] LineType lineType;

    int judgeIndex = 0;
    int maxIndex = 2147483647; //# 2^31-1
    List<JudgePackage> judges;
}