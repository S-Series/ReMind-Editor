using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideParent : MonoBehaviour
{
    public static GameObject copyPrefab; //$ Get Data From "GuideGenerate.cs"
    private static int lastCount = 0;
    public List<GuideHolder> holders = new List<GuideHolder>();

    public void UpdateGuides(int count)
    {
        if (count == lastCount) { return; }

        if (count > lastCount)
        {
            int[] value = new int[2] { count, holders.Count };
            for (int i = value[0]; i < value[1]; i++)
            {
                holders[i].gameObject.SetActive(false);
            }
        }
        else //(count < lastCount)
        {
            int[] value = new int[2] { lastCount, count };
            for (int i = value[0]; i < value[1]; i++)
            {
                try { holders[i].gameObject.SetActive(true); }
                catch
                {
                    holders.Add(
                        Instantiate(copyPrefab, this.transform, false)
                            .GetComponent<GuideHolder>()
                    );
                }
            }
        }
        
        //$ Set GuideHolder Position
        lastCount = count;
        for (int i = 0; i < count; i++)
        {
            holders[i].SetColliderSize(count);
            holders[i].SetPosY(Mathf.RoundToInt(1600f / count * i));
        }
    }
}
