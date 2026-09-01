using System;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.DataStructures.Graphs;

namespace MaroonSeal.StateMachines
{
    /// <summary>
    /// A class used to define a state machine data structure where each state can be updated
    /// at runtime.
    /// </summary>
    /// <typeparam name="TState">State to represent each node in the graph</typeparam>
    public class StateMachine<TState> where TState : IState
    {
        readonly private Graph<TState, IPredicate> graph;

        protected TState current;
        public TState Current => current;

        #region Constructor
        public StateMachine() => graph = new();
        ~StateMachine() => Clear();
        #endregion

        #region State Machine
        virtual public void Update()
        {
            GraphEdge<TState, IPredicate> transition = EvaluateTransitions();

            if (transition.To != null) { SetState(transition.To); }

            current?.Update();
        }
        #endregion

        #region Traversal
        virtual public void SetState(TState _state)
        {
            if (_state.Equals(Current) && Current != null) { return; }

            TState previousState = current;
            TState nextState = _state;

            previousState?.Exit();
            nextState?.Enter();

            current = nextState;
        }

        public TCast GetCurrentState<TCast>() => Current is TCast cast ? cast : default;
        #endregion

        #region Transitions
        GraphEdge<TState, IPredicate> EvaluateTransitions()
        {
            foreach(GraphEdge<TState, IPredicate> transition in graph.GetEdges(current))
            {
                if (transition.To == null) { continue; }
                if (transition.Value.Execute()) { return transition; }
            }

            return new();
        }

        public void AddTransition(TState _from, TState _to, IPredicate _condition) => graph.AddEdge(_from, _to, _condition);
        public void AddTransition(TState _from, TState _to, Func<bool> _condition) => AddTransition(_from, _to, new Predicate(_condition));
        #endregion
    
        public void Clear() => graph.Clear();
    }
}
