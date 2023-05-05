using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    Action action;
    A a = new A();
    B b = new B();

    private void Start()
    {
        action += a.Skill;
        action += b.Skill;
        Reset();
        UseSkill();
    }
    public void UseSkill() { action?.Invoke(); }
    private void Reset()
    {
        action = null;
    }
}

public class A : SkillAction
{
    public override void Skill()
    {
        Debug.Log("01");
    }
}

public class B : SkillAction
{
    public override void Skill()
    {
        Debug.Log("02");
    }
}

public abstract class SkillAction
{
    public abstract void Skill();
}