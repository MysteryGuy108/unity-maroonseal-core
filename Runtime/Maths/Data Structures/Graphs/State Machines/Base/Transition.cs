
using UnityEngine;

using MaroonSeal.Maths.DataStructures.Graphs.Generic;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    public class Transition<TState> : Edge<TState> where TState : IState
    {
        public IPredicate Condition { get; }

        public Transition(TState _next, IPredicate _condition) : base(_next)
        {
            Condition = _condition;
        }
    }

    public class Transition : Transition<IState>
    {
        public Transition(IState _next, IPredicate _condition) : base(_next, _condition) {}
    }
}
