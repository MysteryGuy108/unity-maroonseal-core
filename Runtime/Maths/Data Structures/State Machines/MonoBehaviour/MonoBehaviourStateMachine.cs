using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.StateMachines
{
    public class MonoBehaviourStateMachine<TState> : StateMachine<TState> where TState : IMonoBehaviourState
    {
        #region MonoBehaviour State Machine
        public void FixedUpdate() => current?.FixedUpdate();
        public void LateUpdate() => current?.LateUpdate();

        public void TriggerEnter(Collider _collider) => current?.TriggerEnter(_collider);
        public void TriggerExit(Collider _collider) => current?.TriggerExit(_collider);

        public void CollisionEnter(Collision _collision) => current?.CollisionEnter(_collision);
        public void CollisionExit(Collision _collision) => current?.CollisionExit(_collision);
        #endregion
    }

    public class MonoBehaviourStateMachine : MonoBehaviourStateMachine<IMonoBehaviourState> {}
}
