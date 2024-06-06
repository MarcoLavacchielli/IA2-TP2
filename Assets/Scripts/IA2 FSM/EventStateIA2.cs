using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventStateIA2 : StateIA2
{
    public EventStateIA2(string name = "")
    {
        this.name = name;
    }

    public override void Enter() => OnEnter();

    public override void Exit() => OnExit();

    public override void FixedUpdate() => OnFixedUpdate();

    public override void LateUpdate() => OnLateUpdate();

    public override void Update() => OnUpdate();


    public Action OnEnter = delegate { };
    public Action OnExit = delegate { };
    public Action OnUpdate = delegate { };
    public Action OnFixedUpdate = delegate { };
    public Action OnLateUpdate = delegate { };
}
