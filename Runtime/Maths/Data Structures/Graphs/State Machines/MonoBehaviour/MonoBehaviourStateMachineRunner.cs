using System;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    abstract public class MonoBehaviourStateMachineRunner<TState> : MonoBehaviour where TState : IMonoBehaviourState
    {
        [SerializeField] protected MonoBehaviourStateMachine<TState> stateMachine;

        [EditorReadonly] public string currentState;

        public IState CurrentState => stateMachine.CurrentState;

        #region MonoBehaviour
        virtual protected void Awake() => stateMachine = new MonoBehaviourStateMachine<TState>();
        virtual protected void Update() => stateMachine.Update();
        virtual protected void FixedUpdate() => stateMachine.FixedUpdate();
        #endregion

        #region State Machine
        public void SetState(TState _state) => stateMachine.SetState(_state);

        public TCast GetCurrentState<TCast>() => CurrentState is TCast cast ? cast : default;

        public void AddTransition(TState _state, StateTransition<TState> _transition) => stateMachine.AddEdge(_state, _transition);

        public void AddTransition(TState _from, TState _to, IPredicate _condition) => AddTransition(_from, new(_to, _condition));

        public void AddTransition(TState _from, TState _to, Func<bool> _condition) => AddTransition(_from, _to, new Predicate(_condition));
        #endregion
    }

    abstract public class MonoBehaviourStateMachineRunner : MonoBehaviourStateMachineRunner<IMonoBehaviourState>
    {
        
    }
}
