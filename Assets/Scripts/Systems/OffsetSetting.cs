using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OffsetSetting : MonoBehaviour
{
    private InputAction[] actions;
    private float[] OffsetMs = { 0, 0, 0 };
    private bool isActing = false, isActThisFrame = false;
    private IEnumerator[] offsetCoroutine = { null, null };

    [SerializeField] AudioSource[] GuideSounds;
    [SerializeField] GameObject[] GuideSticks;
    [SerializeField] GameObject[] SubGuideSticks;
    [SerializeField] UnityEngine.UI.Button[] buttons;
    private Animator[] GuideStickBlink;

    private void Awake()    
    {
        actions[0].performed += item => CheckMs();
        actions[1].performed += item =>
        {
            if (isActing) { ResetSystem(); }
            else { SettingBox.DisableSetting(); }
        };
        
        offsetCoroutine[0] = IDrawOffset();
        offsetCoroutine[1] = IJudgeOffset();

        GuideStickBlink[0] = GuideSticks[0].GetComponent<Animator>();
        GuideStickBlink[1] = GuideSticks[1].GetComponent<Animator>();
    }
    private void OnEnable()
    {
        actions[0].Enable();
        actions[1].Enable();
    }
    private void OnDisable()
    {
        actions[0].Disable();
        actions[1].Disable();
    }

    private void FixedUpdate()
    {
        if (!isActing) { return; }

        OffsetMs[0] += Time.fixedDeltaTime * 1000f;
    }

    private void CheckMs()
    {
        isActThisFrame = true;
    }

    public void DrawOffset_btn()
    {
        if (isActing) { ResetSystem(); return; }

        OffsetMs[0] = 0;
        isActing = true;
        buttons[1].interactable = false;

        StopCoroutine(offsetCoroutine[0]);
        offsetCoroutine[0] = IDrawOffset();
        StartCoroutine(offsetCoroutine[0]);
    }
    public void SoundOffset_btn()
    {
        if (isActing) { ResetSystem(); return; }

        OffsetMs[0] = 0;
        isActing = true;
        buttons[0].interactable = false;

        StopCoroutine(offsetCoroutine[1]);
        offsetCoroutine[1] = IDrawOffset();
        StartCoroutine(offsetCoroutine[1]);
    }
    private void ResetSystem()
    {
        OffsetMs[0] = 0;
        StopCoroutine(offsetCoroutine[0]);
        StopCoroutine(offsetCoroutine[1]);
    }

    private IEnumerator ISound()
    {
        for (int i = 0; true; i++)
        {
            yield return new WaitWhile(() => OffsetMs[0] >= 1000 * i);
            GuideSounds[0].Play();
            yield return new WaitWhile(() => OffsetMs[0] >= 250 + (1000 * i));
            GuideSounds[1].Play();
            yield return new WaitWhile(() => OffsetMs[0] >= 500 + (1000 * i));
            GuideSounds[1].Play();
            yield return new WaitWhile(() => OffsetMs[0] >= 750 + (1000 * i));
            GuideSounds[1].Play();
        }
    }
    //# Display Delay
    private IEnumerator IDrawOffset()
    {
        for (int i = 0; true; i++)
        {
            yield return new WaitWhile(() => OffsetMs[0] + OffsetMs[1] >= 1000 * i);
            //DoSomething();
        }
    }
    //# Input Delay
    private IEnumerator IJudgeOffset()
    {
        while (true)
        {
            yield return new WaitWhile(() => isActThisFrame);
            isActThisFrame = false;
            float dif;
            dif = (OffsetMs[0] + OffsetMs[2]) % 1000;
            dif = dif > 500 ? dif - 1000 : dif;
        }
    }
}
