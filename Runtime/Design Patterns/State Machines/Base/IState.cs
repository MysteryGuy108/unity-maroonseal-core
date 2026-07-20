using UnityEngine;

namespace MaroonSeal.DataStructures.StateMachines
{
    /// <summary>
    /// Interface for classes that are to be used as states in a state machine.
    /// </summary>
    public interface IState
    {
        bool IsActive { get; }

        /// <summary>
        /// To be called upon state entry.
        /// </summary>
        void Enter();

        /// <summary>
        /// To be called upon state machine update.
        /// </summary>
        void Update();

        /// <summary>
        /// To be called upon state exit.
        /// </summary>
        void Exit();
    }
}
