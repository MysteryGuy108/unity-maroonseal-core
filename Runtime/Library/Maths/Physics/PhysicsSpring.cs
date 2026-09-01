using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.Physics {
    [System.Serializable]
    public class PhysicsSpring : PhysicsBody {
        [Header("Spring Settings")]
        [SerializeField] private float springConstant = 1.0f;
        [SerializeField] private float springDampening = 0.0f;
        [SerializeField] private Vector3 targetPosition = Vector3.zero;
        [SerializeField] private float maxExtension = Mathf.Infinity;

        public Vector3 Displacement { get { return Position - targetPosition; } }
        public Vector3 NormalisedDisplacement { get { return Displacement / maxExtension; } }

        override public void StepPosition(float _timeSinceLastStep) {
            Vector3 springForce = -springConstant * Displacement;
            AddForce(springForce);
            AddForce(-Velocity * springDampening);

            base.StepPosition(_timeSinceLastStep);

            if (Displacement.magnitude > maxExtension) {
                Vector3 offset = Vector3.ClampMagnitude(Displacement, maxExtension);
                position = targetPosition + offset;
                velocity = Vector3.zero;
            }
        }
    }
}