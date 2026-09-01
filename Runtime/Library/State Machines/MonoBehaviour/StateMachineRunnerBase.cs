using System;
using UnityEngine;

namespace MaroonSeal.StateMachines
{
    /// <summary>
    /// Component used to run state machines.
    /// </summary>
    /// <typeparam name="TState">The type of state machines to run. </typeparam>
    /// <typeparam name="TStateMachine">The type of state machine to run. </typeparam>
    abstract public class StateMachineRunnerBase<TState, TStateMachine> : MonoBehaviour
        where TState : IState
        where TStateMachine : StateMachine<TState>, new()
    {
        [SerializeField] protected TStateMachine stateMachine;
        public IState CurrentState => stateMachine.Current;

        #region MonoBehaviour
        virtual protected void Awake() => stateMachine = new();
        virtual protected void Update() => stateMachine.Update();
        #endregion

        #region State Machine
        protected void SetState(TState _state) => stateMachine.SetState(_state);

        protected TCast GetCurrentState<TCast>() => CurrentState is TCast cast ? cast : default;


        protected void AddTransition(TState _from, TState _to, IPredicate _condition) => stateMachine.AddTransition(_from, _to, _condition);

        protected void AddTransition(TState _from, TState _to, Func<bool> _condition) => AddTransition(_from, _to, new Predicate(_condition));
        #endregion
    }
}
