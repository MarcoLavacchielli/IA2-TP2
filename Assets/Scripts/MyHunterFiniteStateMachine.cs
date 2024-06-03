using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Facundo Sebastian Tisera
public class MyHunterFiniteStateMachine//=FiniteStateMachine
{
    //estado actual
    StateFather _currentState;
    //Diccionario de estados
    Dictionary<PlayerStates, StateFather> _allStates = new Dictionary<PlayerStates, StateFather>();
    

    public void Update()
    {
        
        _currentState?.ThisStateUpdate(); //Evita llamados nulos
    }

    public void AddState(PlayerStates name, StateFather state)
    {
        if (!_allStates.ContainsKey(name))
        {
            _allStates.Add(name, state);
            state.HunterFSM = this;
        }
        else
        {
            _allStates[name] = state;
        }
    }

    public void ChangeState(PlayerStates name) //Para ir cambiando de estados, los vamos pidiendo
    {
        _currentState?.OnExit(); //Si o si necesario preguntar por null
        if (_allStates.ContainsKey(name)) _currentState = _allStates[name];
        _currentState?.OnEnter();
    }
   
}
