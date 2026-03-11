using System;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.DataStructures.Graphs.Generic;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    public class StateMachine<TState> : Graph<TState, Transition<TState>> where TState : IState
    {
        protected Node current;
        public TState CurrentState
        {
            get
            {
                if (current == null) { return default; }
                return current.Value;
            }
        }

        #region Constructor
        public StateMachine() : base() {}
        #endregion

        #region State Machine
        virtual public void Update()
        {
            var transition = EvaluateTransitions();

            if (transition != null)
            {
                SetState(transition.Next);
            }

            current.Value?.Update();
        }
        #endregion

        #region Traversal
        virtual public void SetState(TState _state)
        {
            if (_state.Equals(CurrentState) && CurrentState != null) { return; }

            var previousState = current;
            var nextState = GetNode(_state);

            previousState?.Value.Exit();
            nextState?.Value.Enter();

            current = nextState;
        }

        public TCast GetCurrentState<TCast>() => CurrentState is TCast cast ? cast : default;
        #endregion

        #region Transitions
        Transition<TState> EvaluateTransitions()
        {
            foreach(var transition in current.Edges)
            {
                if (transition.Condition.Execute()) { return transition; }
            }

            return null;
        }

        public void AddTransition(TState _state, Transition<TState> _transition) => AddEdge(_state, _transition);
        public void AddTransition(TState _from, TState _to, IPredicate _condition) => AddTransition(_from, new(_to, _condition));
        public void AddTransition(TState _from, TState _to, Func<bool> _condition) => AddTransition(_from, _to, new Predicate(_condition));
        #endregion
    }

    public class StateMachine : StateMachine<IState> {}
}
