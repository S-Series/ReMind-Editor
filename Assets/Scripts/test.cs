using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    [SerializeField] Animator anim;
    private const string animTag = "Crouch";
    bool isPressed = false;
    KeyCode KC = KeyCode.F;

    IEnumerator Delay;

    private void Start()
    {
        Delay = IDoSomething();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KC))
        {
            StartCoroutine(Delay);
        }
        if (Input.GetKeyUp(KC))
        {
            StopCoroutine(Delay);
            Delay = IDoSomething();
        }
    }

    private IEnumerator IDoSomething()
    {
        yield return new WaitForSeconds(0.3f);
        SkillActivate();
    }

    private void SkillActivate()
    {

    }
}
