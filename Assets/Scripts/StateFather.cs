using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Facundo Sebastian Tisera
public abstract class StateFather//=state
{
    public abstract void OnEnter();

    public abstract void ThisStateUpdate();
    public abstract void OnExit();

    public MyHunterFiniteStateMachine HunterFSM;



}
