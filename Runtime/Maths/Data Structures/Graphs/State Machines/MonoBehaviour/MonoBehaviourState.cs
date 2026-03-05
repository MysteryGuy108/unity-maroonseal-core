
using UnityEngine;

using MaroonSeal.Maths.DataStructures.Graphs.StateMachines;
using System.Collections;

namespace MaroonSeal
{
    public interface IMonoBehaviourState : IState
    {
        public void OnTransitionEnter();

        public IEnumerator EnterRoutine();
        public void LateUpdate();
        public void FixedUpdate();


        public void OnTriggerEnter(Collider _collider);
        public void OnTriggerExit(Collider _collider);

        public void OnCollisionEnter(Collision _collision);
        public void OnCollisionExit(Collision _collision);
    }

    public class MonoBehaviourState : IMonoBehaviourState
    {
        virtual public void OnEnter() {}
        virtual public void OnTransitionEnter() {}
        virtual public IEnumerator EnterRoutine() => null;

        virtual public void Update() {}
        virtual public void LateUpdate() {}
        virtual public void FixedUpdate() {}

        virtual public void OnExit() {}

        virtual public void OnTriggerEnter(Collider _collider) {}
        virtual public void OnTriggerExit(Collider _collider) {}

        virtual public void OnCollisionEnter(Collision _collision) {}
        virtual public void OnCollisionExit(Collision _collision) {}
    }
}
