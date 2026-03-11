
using UnityEngine;

using MaroonSeal.Maths.DataStructures.Graphs.StateMachines;
using System.Collections;

namespace MaroonSeal
{
    public interface IMonoBehaviourState : IState
    {
        public void TransitionEnter();

        public void LateUpdate();
        public void FixedUpdate();

        public void TriggerEnter(Collider _collider);
        public void TriggerExit(Collider _collider);

        public void CollisionEnter(Collision _collision);
        public void CollisionExit(Collision _collision);
    }

    abstract public class MonoBehaviourState : StateBase, IMonoBehaviourState
    {
        #region IMonoBehaviourState
        public void TransitionEnter()
        {
            if (!IsActive) { return; }
            OnTransitionEnter();
        }

        public void LateUpdate()
        {
            if (!IsActive) { return; }
            OnLateUpdate();
        }

        public void FixedUpdate()
        {
            if (!IsActive) { return; }
            OnFixedUpdate();
        }

        public void TriggerEnter(Collider _collider)
        {
            if (!IsActive) { return; }
            OnTriggerEnter(_collider);
        }

        public void TriggerExit(Collider _collider)
        {
            if (!IsActive) { return; }
            OnTriggerExit(_collider);
        }

        public void CollisionEnter(Collision _collision)
        {
            if (!IsActive) { return; }
            OnCollisionEnter(_collision);
        }

        public void CollisionExit(Collision _collision)
        {
            if (!IsActive) { return; }
            OnCollisionExit(_collision);
        }
        #endregion

        virtual protected void OnTransitionEnter() {}

        virtual protected void OnLateUpdate() {}
        virtual protected void OnFixedUpdate() {}

        virtual protected void OnTriggerEnter(Collider _collider) {}
        virtual protected void OnTriggerExit(Collider _collider) {}

        virtual protected void OnCollisionEnter(Collision _collision) {}
        virtual protected void OnCollisionExit(Collision _collision) {}
    }
}
