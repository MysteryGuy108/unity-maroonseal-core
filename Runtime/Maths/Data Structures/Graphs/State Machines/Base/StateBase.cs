using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
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

        virtual protected  void OnEnter() {}

        virtual protected void OnUpdate() {}

        virtual protected void OnExit()  { }
    }
}
