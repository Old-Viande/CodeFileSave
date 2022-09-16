using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class TurnBaseFSM : Singleton<TurnBaseFSM>
{
    public Dictionary<StateType, IState> state = new Dictionary<StateType, IState>();
    public StateType currentStateType;
    public IState currentState;

    protected virtual void Start()
    {
        state.Add(StateType.Start, new StartState(this));
        state.Add(StateType.Running, new RuningState(this));
        state.Add(StateType.End, new EndState(this));
        state.Add(StateType.Check, new CheckState(this));
        state.Add(StateType.Move, new MoveState(this));
        state.Add(StateType.Attack, new AttackState(this));
        state.Add(StateType.Deffence, new DeffenceState(this));
        TranState(StateType.Start);
    }

    private void Update()
    {
        currentState.OnUpdata();
    }

    public void TranState(StateType type)
    {
        if (currentStateType == type || type == StateType.Unknown)
        {
            return;
        }
        if (currentStateType != StateType.Unknown)
            currentState.OnExit();
        currentStateType = type;
        currentState = state[type];
        currentState.OnEnter();
    }

}
*/