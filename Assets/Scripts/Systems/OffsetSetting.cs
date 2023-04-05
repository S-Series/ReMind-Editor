using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OffsetSetting : MonoBehaviour
{
    private int getMs, testMs, index;
    private float drawMs, judgeMs;
    private IEnumerator DisplayCoroutine;
    [SerializeField] AudioSource[] audioSources;
    [SerializeField] InputAction input;
    [SerializeField] GameObject[] inputObjects;
    [SerializeField] TMPro.TextMeshPro outputTmp;

    private void Awake()
    {
        input.performed += item =>
        {
            DisplayInput();
        };
        input.Enable();
    }
    private void OnEnable()
    {
        testMs = 0;
        DisplayCoroutine = IDisplay(0);
        outputTmp.color = new Color32(150, 150, 150, 255);
        outputTmp.text = "";
    }
    private void OnDisable()
    {
        StopCoroutine(DisplayCoroutine);
    }
    private void FixedUpdate()
    {
        testMs++;
    }

    private void Update()
    {
        getMs = testMs;
        drawMs = getMs % 2000 + ValueManager.s_DrawOffset;
        judgeMs = getMs + ValueManager.s_JudgeOffset;

        if (drawMs > 2000) { drawMs -= 2000; }
        else if (drawMs < 0) { drawMs += 2000; }

        inputObjects[0].transform.localPosition
            = new Vector3(Mathf.Lerp(-1100f, 1100f, drawMs >= 1000
            ? (drawMs - 1000) / 2000f : (drawMs + 1000) / 2000f), 0, 0);

        if (judgeMs >= 500 * (index))
        {
            if (index % 4 == 0) { audioSources[0].Play(); }
            else { audioSources[1].Play(); }

            index++;
        }
    }
    private void DisplayInput()
    {
        inputObjects[1].transform.localPosition = inputObjects[0].transform.localPosition;
        StopCoroutine(DisplayCoroutine);
        DisplayCoroutine = IDisplay(testMs);
        StartCoroutine(DisplayCoroutine);
    }

    IEnumerator IDisplay(int judgeMs)
    {
        judgeMs = (judgeMs + ValueManager.s_JudgeOffset) % 2000;

        float timer = 0.0f;

        SpriteRenderer renderer;
        renderer = inputObjects[1].GetComponent<SpriteRenderer>();
        renderer.color = new Color32(255, 235, 0, 255);

        if (judgeMs == 0)
        {
            outputTmp.text = "<color=#FFFF64>��0ms</color>";
        }
        else
        {
            outputTmp.text = judgeMs > 1000 ?
                string.Format("<color=#6464FF>Fast</color>    -{0:D4}ms", 2000 - judgeMs) :
                string.Format("<color=#FF6464>Late</color>    +{0:D4}ms", judgeMs);
        }

        yield return new WaitForSeconds(0.25f);

        while (true)
        {
            timer += Time.deltaTime;
            renderer.color = new Color32(255, 235, 0, (byte)(Mathf.Lerp(0, 255, 1 - timer / 0.5f)));
            if (timer >= 0.5f) { break; }
            yield return null;
        }
    }
}
