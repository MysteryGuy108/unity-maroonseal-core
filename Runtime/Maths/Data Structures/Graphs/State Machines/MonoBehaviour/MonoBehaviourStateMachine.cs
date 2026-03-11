using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    public class MonoBehaviourStateMachine<TState> : StateMachine<TState> where TState : IMonoBehaviourState
    {
        #region MonoBehaviour State Machine
        public void FixedUpdate() => current.Value?.FixedUpdate();
        public void LateUpdate() => current.Value?.LateUpdate();

        public void TriggerEnter(Collider _collider) => current.Value?.TriggerEnter(_collider);
        public void TriggerExit(Collider _collider) => current.Value?.TriggerExit(_collider);

        public void CollisionEnter(Collision _collision) => current.Value?.CollisionEnter(_collision);
        public void CollisionExit(Collision _collision) => current.Value?.CollisionExit(_collision);
        #endregion
    }

    public class MonoBehaviourStateMachine : MonoBehaviourStateMachine<IMonoBehaviourState> {}
}
