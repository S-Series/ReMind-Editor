using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;
using GameSystem;

public class JudgeSystem : MonoBehaviour
{
    enum LineType { Line01 = 1, Line02 = 2, Line03 = 3, Line04 = 4, Side_L = 5, Side_R = 6 };
    [SerializeField] LineType lineType;

    int judgeIndex = 0;
    int maxIndex = 2147483647; //# 2^31-1
    List<JudgePackage> judges;
}