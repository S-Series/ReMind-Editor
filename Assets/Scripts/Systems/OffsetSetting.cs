using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OffsetSetting : MonoBehaviour
{
    private int testMs, index;
    private IEnumerator TestCoroutine, DisplayCoroutine;
    [SerializeField] AudioSource[] audioSources;
    [SerializeField] InputAction input;
    [SerializeField] GameObject[] inputObjects;

    private void Awake()
    {
        TestCoroutine = ITesting();
        input.performed += item =>
        {
            print("run");
            DisplayInput();
        };
        input.Enable();
    }
    private void OnEnable()
    {
        testMs = 0;
        TestCoroutine = ITesting();
        DisplayCoroutine = IDisplay();
        StartCoroutine(TestCoroutine);
    }
    private void OnDisable()
    {
        StopCoroutine(TestCoroutine);
        StopCoroutine(DisplayCoroutine);
    }
    private void FixedUpdate()
    {
        testMs++;
    }

    IEnumerator ITesting()
    {
        float drawMs, judgeMs;
        while (true)
        {
            if (testMs > 2000) { testMs -= 2000; }

            drawMs = testMs + ValueManager.s_DrawOffset;
            judgeMs = testMs + ValueManager.s_JudgeOffset;

            inputObjects[0].transform.localPosition
                = new Vector3(Mathf.Lerp(-660f, 660f, testMs >= 1000 
                ? (testMs - 1000) / 2000f : (testMs + 1000) / 2000f), 0, 0);

            if (judgeMs >= 500 * (index))
            {
                // if (index == 0) { audioSources[0].Play(); }
                // else { audioSources[1].Play(); }
            }

            yield return null;
        }
    }

    private void DisplayInput()
    {
        inputObjects[1].transform.localPosition = inputObjects[0].transform.localPosition;
        StopCoroutine(DisplayCoroutine);
        DisplayCoroutine = IDisplay();
        StartCoroutine(DisplayCoroutine);
    }

    IEnumerator IDisplay()
    {
        print("display");
        float timer = 0.0f;

        SpriteRenderer renderer;
        renderer = inputObjects[1].GetComponent<SpriteRenderer>();
        renderer.color = new Color32(255, 235, 0, 255);

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
