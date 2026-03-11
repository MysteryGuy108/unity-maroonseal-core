using System;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.StateMachines
{
    abstract public class MonoBehaviourStateMachineRunnerBase<TState, TStateMachine> : StateMachineRunnerBase<TState, TStateMachine> 
        where TState : IMonoBehaviourState
        where TStateMachine : MonoBehaviourStateMachine<TState>, new()
    {
        #region MonoBehaviour
        virtual protected void FixedUpdate() => stateMachine.FixedUpdate();
        virtual protected  void LateUpdate() => stateMachine.LateUpdate();

        virtual protected  void OnTriggerEnter(Collider _collider) => stateMachine.TriggerEnter(_collider);
        virtual protected  void OnTriggerExit(Collider _collider) => stateMachine.TriggerExit(_collider);

        virtual protected  void OnCollisionEnter(Collision _collision) => stateMachine.CollisionEnter(_collision);
        virtual protected  void OnCollisionExit(Collision _collision) => stateMachine.CollisionExit(_collision);
        #endregion


    }

    abstract public class MonoBehaviourStateMachineRunnerBase : MonoBehaviourStateMachineRunnerBase<IMonoBehaviourState, MonoBehaviourStateMachine>
    {
        
    }
}
