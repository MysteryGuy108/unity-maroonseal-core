using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    public class MonoBehaviourStateMachine<TState> : StateMachine<TState> where TState : IMonoBehaviourState
    {
        #region MonoBehaviour State Machine
        public void FixedUpdate() => current.Value?.FixedUpdate();
        public void LateUpdate() => current.Value?.LateUpdate();

        public void OnTriggerEnter(Collider _collider) => current.Value?.OnTriggerEnter(_collider);
        public void OnTriggerExit(Collider _collider) => current.Value?.OnTriggerExit(_collider);

        public void OnCollisionEnter(Collision _collision) => current.Value?.OnCollisionEnter(_collision);
        public void OnCollisionExit(Collision _collision) => current.Value?.OnCollisionExit(_collision);
        #endregion
    }

    public class MonoBehaviourStateMachine : MonoBehaviourStateMachine<IMonoBehaviourState> {}
}
