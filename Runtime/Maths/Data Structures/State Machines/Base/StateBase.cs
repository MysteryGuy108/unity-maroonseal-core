using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.StateMachines
{
    /// <summary>
    /// Base class for state machine states.
    /// </summary>
    public abstract class StateBase : IState
    {
        [SerializeField][HideInInspector] private bool isActive;
        public bool IsActive => isActive;

        #region IState
        public void Enter()
        {
            isActive = true;
            OnEnter();
        }

        public void Update()
        {
            if (!isActive) { return; }
            OnUpdate();
        }

        public void Exit()
        {
            OnExit();
            isActive = false;
        }
        #endregion

        /// <summary>
        /// Called upon state entered.
        /// </summary>
        virtual protected  void OnEnter() {}

        /// <summary>
        /// Called upon state updated.
        /// </summary>
        virtual protected void OnUpdate() {}

        /// <summary>
        /// Called upon state exit.
        /// </summary>
        virtual protected void OnExit()  { }
    }
}
