using System;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.DataStructures.Graphs;

namespace MaroonSeal.Maths.DataStructures.StateMachines
{
/// <summary>
    /// A class used to define a state machine data structure where each state can be updated
    /// at runtime.
    /// </summary>
    /// <typeparam name="TState">State to represent each node in the graph</typeparam>
    public class StateMachine<TState> : Graph<TState, IPredicate> where TState : IState
    {
        protected TState current;
        public TState CurrentState
        {
            get
            {
                if (current == null) { return default; }
                return current;
            }
        }

        #region Constructor
        public StateMachine() : base() {}
        #endregion

        #region State Machine
        virtual public void Update()
        {
            var transition = EvaluateTransitions();

            if (transition.Key != null)
            {
                SetState(transition.Key);
            }

            current?.Update();
        }
        #endregion

        #region Traversal
        virtual public void SetState(TState _state)
        {
            if (_state.Equals(CurrentState) && CurrentState != null) { return; }

            TState previousState = current;
            TState nextState = _state;

            previousState?.Exit();
            nextState?.Enter();

            current = nextState;
        }

        public TCast GetCurrentState<TCast>() => CurrentState is TCast cast ? cast : default;
        #endregion

        #region Transitions
        KeyValuePair<TState, IPredicate> EvaluateTransitions()
        {
            foreach(KeyValuePair<TState, IPredicate> transition in GetNodeNeighbors(current))
            {
                if (transition.Value.Execute()) { return transition; }
            }

            return new();
        }

        public void AddTransition(TState _from, TState _to, IPredicate _condition) => AddEdge(_from, _to, _condition);
        public void AddTransition(TState _from, TState _to, Func<bool> _condition) => AddTransition(_from, _to, new Predicate(_condition));
        #endregion
    }

    public class StateMachine : StateMachine<IState> {}
}
