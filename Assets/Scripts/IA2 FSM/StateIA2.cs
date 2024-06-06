using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateIA2
{
    public string name;
    public StateIA2(string name = "")
    {
        this.name = name;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void LateUpdate();
    public abstract void FixedUpdate();
    public abstract void Exit();
}
