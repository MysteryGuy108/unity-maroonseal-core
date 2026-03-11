using System;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    abstract public class StateMachineRunnerBase<TState, TStateMachine> : MonoBehaviour
        where TState : IState
        where TStateMachine : StateMachine<TState>, new()
    {
        [SerializeField] protected TStateMachine stateMachine;
        public IState CurrentState => stateMachine.CurrentState;

        #region MonoBehaviour
        virtual protected void Awake() => stateMachine = new();
        virtual protected void Update() => stateMachine.Update();
        #endregion

        #region State Machine
        protected void SetState(TState _state) => stateMachine.SetState(_state);

        protected TCast GetCurrentState<TCast>() => CurrentState is TCast cast ? cast : default;

        protected void AddTransition(TState _state, Transition<TState> _transition) => stateMachine.AddEdge(_state, _transition);

        protected void AddTransition(TState _from, TState _to, IPredicate _condition) => AddTransition(_from, new(_to, _condition));

        protected void AddTransition(TState _from, TState _to, Func<bool> _condition) => AddTransition(_from, _to, new Predicate(_condition));
        #endregion
    }

    abstract public class StateMachineRunnerBase : StateMachineRunnerBase<IState, StateMachine> {}
}
