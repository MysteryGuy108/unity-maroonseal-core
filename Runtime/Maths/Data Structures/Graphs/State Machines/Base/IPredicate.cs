using System;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    public interface IPredicate
    {
        bool Execute();
    }

    public class Predicate : IPredicate
    {
        readonly Func<bool> func;

        public Predicate(Func<bool> _func) => func = _func;
        public bool Execute() => func.Invoke();
    }
}
