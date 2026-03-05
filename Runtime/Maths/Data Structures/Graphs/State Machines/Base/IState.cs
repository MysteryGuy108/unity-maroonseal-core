using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    public interface IState
    {
        void OnEnter();
        void Update();
        void OnExit();
    }

    public abstract class StateBase : IState
    {
        virtual public  void OnEnter() {}

        virtual public void Update() {}

        virtual public void OnExit() {}
    }
}
