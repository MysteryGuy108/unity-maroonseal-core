using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    public interface IState
    {
        bool IsActive { get; }
        void Enter();
        void Update();
        void Exit();
    }
}
