using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<TOwner> where TOwner : class
{
    protected StateMachine<TOwner> StateMachine => stateMachine;
    internal StateMachine<TOwner> stateMachine;

    internal Dictionary<int, State<TOwner>> transition = new Dictionary<int, State<TOwner>>();

    protected TOwner owner => stateMachine.Owner;

    internal void Enter(State<TOwner> preState)
    {
        OnEnter(preState);
    }

    protected virtual void OnEnter(State<TOwner> preState)
    {

    }

    internal void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    internal void Ezit(State<TOwner> nextState)
    {
        onExit(nextState);
    }

    protected virtual void onExit(State<TOwner> nextState)
    {

    }
}

public sealed class ForceState<TOwner> : State<TOwner> where TOwner : class
{
    
}