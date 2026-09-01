using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.Physics {
    [System.Serializable]
    public class PhysicsBody {
        [Header("Physics Body Settings")]
        [SerializeField] protected float mass;
        public float Mass { get { return mass; } }
        [Space]
        [SerializeField] protected Vector3 position;
        public Vector3 Position { get { return position; } }
        [SerializeField] protected Vector3 velocity;
        public Vector3 Velocity { get { return velocity; } }
        [Space]
        [SerializeField] private bool lockXAxis;
        [SerializeField] private bool lockYAxis;
        [SerializeField] private bool lockZAxis;

        virtual public void StepPosition(float _timeSinceLastStep) { 
            Vector3 positionDelta = velocity * _timeSinceLastStep;

            position.x = lockXAxis ? position.x : position.x + (positionDelta.x * _timeSinceLastStep);
            position.y = lockYAxis ? position.y : position.y + (positionDelta.y * _timeSinceLastStep); 
            position.z = lockZAxis ? position.z : position.z + (positionDelta.z * _timeSinceLastStep); 
        }

        public void AddAcceleration(Vector3 _acceleration) { velocity += _acceleration; }
        public void AddForce(Vector3 _force) { AddAcceleration(_force / mass); }
    }
}

