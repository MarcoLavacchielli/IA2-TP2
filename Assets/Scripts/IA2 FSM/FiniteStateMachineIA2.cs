using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachineIA2<T>
{
    private StateIA2 _currentState;
    private Dictionary<T, StateIA2> _allStates = new Dictionary<T, StateIA2>();

    public void AddState(T ID, StateIA2 state) => _allStates[ID] = state;

    public void ChangeState(T ID)
    {
        if (!_allStates.ContainsKey(ID)) return;

        _currentState?.Exit();
        _currentState = _allStates[ID];
        currentStateID = ID;
        _currentState.Enter();
    }

    public StateIA2 CurrentState => _currentState;
    public T currentStateID;
    //public EventState GetEventState(T Elem) => _allStates[Elem] as EventState;

    public void Update() => _currentState?.Update();
    public void LateUpdate() => _currentState?.LateUpdate();
    public void FixedUpdate() => _currentState?.FixedUpdate();
}
