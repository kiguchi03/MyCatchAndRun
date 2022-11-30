using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TOwner> where TOwner : class
{
    public TOwner Owner { get; }

    //現在のステート
    public State<TOwner> CurrentState { get; private set; }

    //ステートリスト
    public List<State<TOwner>> states = new List<State<TOwner>>();

    public StateMachine(TOwner owner)
    {
        Owner = owner;
    }

    public void Start(State<TOwner> firstState)
    {
        CurrentState = firstState;
        CurrentState.Enter(null);
    }

    public void Start<Tfirst>() where Tfirst : State<TOwner>, new()
    {
        Start(GetOrAddState<Tfirst>());
    }

    public void Update()
    {
        CurrentState.Update();
    }

    //ステートの遷移先を追加するメソッド
    public void AddTransition<Tfrom, Tto>(int eventID) where Tfrom : State<TOwner>, new()
        where Tto : State<TOwner>, new()
    {
        Tfrom from = GetOrAddState<Tfrom>();

        if (!from.transition.ContainsKey(eventID))
        {
            var to = GetOrAddState<Tto>();
            from.transition.Add(eventID, to);
        }
    }

    //強制遷移を追加するメソッド
    public void AddForceTransition<Tto>(int eventID) where Tto : State<TOwner>, new()
    {
        AddTransition<ForceState<TOwner>, Tto>(eventID);
    }

    //ステートリストからステートを取得、またはステートリストにステートを追加するメソッド
    public T GetOrAddState<T>() where T : State<TOwner>, new()
    {
        foreach (var stat in states)
        {
            if (typeof(T) == stat.GetType())
            {
                return (T)stat;
            }
        }
        T state = new T();
        state.stateMachine = this;
        states.Add(state);

        return state;

    }

    //ステートを遷移させるメソッド
    public void OfferTransition(int eventID)
    {
        State<TOwner> to;
        if (!CurrentState.transition.TryGetValue(eventID, out to))
        {
            if (!GetOrAddState<ForceState<TOwner>>().transition.TryGetValue(eventID,out to))
            {
                return;
            }
        }
        ChangeState(to);
    }

    public void ChangeState(State<TOwner> nextState)
    {
        CurrentState.Ezit(nextState);
        nextState.Enter(CurrentState);
        CurrentState = nextState;
    }
}
