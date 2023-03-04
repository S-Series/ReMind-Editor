using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideGenerate : MonoBehaviour
{
    private static GuideGenerate s_this;
    public static int s_guideCount = 1;
    [SerializeField] GameObject ColliderPrefab;
    [SerializeField] Transform ColliderField;
    private static List<GuideHolder> holders = new List<GuideHolder>();

    private void Awake() { s_this = this; }

    public static void Generate(int count)
    {
        GameObject copyObject;
        
        foreach (GuideHolder holder in holders)
        {
            Destroy(holder.gameObject);
        }

        holders = new List<GuideHolder>();

        if (count < 01) { count = 01; }
        if (count > 33) { count = 32; }

        s_guideCount = count;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < count; j++)
            {
                
            }
        }
    }
}
