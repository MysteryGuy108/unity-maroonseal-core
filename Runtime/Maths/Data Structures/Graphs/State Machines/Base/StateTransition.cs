
using UnityEngine;

using MaroonSeal.Maths.DataStructures.Graphs.Generic;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    public class StateTransition<TState> : Edge<TState> where TState : IState
    {
        public IPredicate Condition { get; }

        public StateTransition(TState _next, IPredicate _condition) : base(_next)
        {
            Condition = _condition;
        }
    }

    public class StateTransition : StateTransition<IState>
    {
        public StateTransition(IState _next, IPredicate _condition) : base(_next, _condition) {}
    }
}
