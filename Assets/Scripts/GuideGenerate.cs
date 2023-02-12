using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideGenerate : MonoBehaviour
{
    private static GuideGenerate s_this;
    public static int s_guideCount;
    [SerializeField] GameObject guidePrefab;
    [SerializeField] Transform guideField;

    private void Awake()
    {
        Generate(1);
        s_this = this;
    }
    public static void Generate(int count)
    {
        int _pos;
        GameObject _copyObject;

        if (count < 01) { count = 01; }
        if (count > 32) { count = 32; }
        s_guideCount = count;

        //$ Reset Guide Field
        for (int i = 0; i < s_this.guideField.childCount - 1; i++)
        {
            Destroy(s_this.guideField.GetChild(i).gameObject);
        }

        for (int i = 0; i < s_guideCount - 1; i++)
        {
            _pos = Mathf.RoundToInt(1600.0f / s_guideCount * (i + 1));
            for (int j = 0; j < 999; j++)
            {
                _copyObject = Instantiate(s_this.guidePrefab, s_this.guideField, false);
                _copyObject.name = string.Format("{0} - {1}", j, i);
                _copyObject.transform.localPosition
                    = new Vector3(0, 1600 * j + _pos, 0);
                if (_pos % 400 == 0)
                    { _copyObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 0, 000, 150); }
                else if (_pos % 200 == 0) 
                    { _copyObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 0, 255, 150); }
            }
        }
    }
}
