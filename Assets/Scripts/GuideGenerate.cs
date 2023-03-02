using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideGenerate : MonoBehaviour
{
    private static GuideGenerate s_this;
    public static int s_guideCount = 1;
    [SerializeField] GameObject guidePrefab;

    private void Awake() { s_this = this; }

    public static void Generate(int count)
    {
        if (count < 01) { count = 01; }
        if (count > 33) { count = 32; }

        s_guideCount = count;
        foreach (LineHolder holder in NoteField.s_holders)
        {
            holder.GenerateCollider(s_this.guidePrefab, count);
            foreach (GuideHolder guide in holder.holders)
            {
                guide.EnableCollider(NoteGenerate.s_isGenerating);
            }
        }
    }
}
