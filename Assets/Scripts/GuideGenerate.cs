using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideGenerate : MonoBehaviour
{
    private static GuideGenerate s_this;
    public static int s_guideCount;
    [SerializeField] GameObject guidePrefab;

    private void Awake()
    {
        s_this = this;
    }
}
