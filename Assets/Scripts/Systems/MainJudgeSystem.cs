using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNote;
using GameSystem;

public class MainJudgeSystem : MonoBehaviour
{
    public static MainJudgeSystem s_this;
    void Awake()
    {
        if (s_this == null) { s_this = this; }
    }
}
