using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OffsetSetting : MonoBehaviour
{
    private InputAction[] actions;
    private float[] OffsetMs = { 0, 0 };
    private bool isActing = false, isActThisFrame = false;
    private IEnumerator[] timerCoroutine = { null, null };
    private IEnumerator[] offsetCoroutine = { null, null };

    [SerializeField] AudioSource[] GuideSounds;
    [SerializeField] GameObject[] GuideSticks;
    [SerializeField] GameObject[] SubGuideSticks;
    private Animator[] GuideStickBlink;

    private void Awake()
    {
        actions[0].performed += item => CheckMs();
        actions[1].performed += item =>
        {
            if (isActing) { EndAllCoroutine(); }
            else { SettingBox.DisableSetting(); }
        };
        
        offsetCoroutine[0] = IDrawOffset();
        offsetCoroutine[1] = ISoundOffset();

        timerCoroutine[0] = ITimer(0);
        timerCoroutine[1] = ISound();

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

    private void CheckMs()
    {
        isActThisFrame = true;
    }

    public void DrawOffset_btn()
    {
        if (isActing) { return; }
    }
    public void SoundOffset_btn()
    {

    }
    private void EndAllCoroutine()
    {

    }

    private IEnumerator ITimer(int index)
    {
        float timer = 0.0f;
        isActThisFrame = false;

        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= 1.0f) { timer -= 1.0f; }
            OffsetMs[0] = timer * 1000f;



            return null;
        }
    }
    private IEnumerator ISound()
    {
        while (true)
        {
            GuideSounds[0].Play();

            yield return new WaitWhile(() => OffsetMs[0] >= 250);
            
            GuideSounds[1].Play();

            yield return new WaitWhile(() => OffsetMs[0] >= 500);
            
            GuideSounds[1].Play();

            yield return new WaitWhile(() => OffsetMs[0] >= 750);
            
            GuideSounds[1].Play();

            yield return new WaitWhile(() => OffsetMs[0] <= 250);
        }
    }
    private IEnumerator IDrawOffset()
    {
        while (true)
        {
            yield return new WaitWhile(() => isActThisFrame);

            GuideSticks[0].transform.localPosition 
                = SubGuideSticks[1].transform.localPosition;
            isActThisFrame = false;
        }
    }
    private IEnumerator ISoundOffset()
    {
        while (true)
        {
            yield return new WaitWhile(() => isActThisFrame);

            isActThisFrame = false;
        }
    }
}
