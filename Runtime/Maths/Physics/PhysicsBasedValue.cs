using UnityEngine;

namespace MaroonSeal
{
    abstract public class PhysicsBasedValue<TValue>
    {
        protected TValue value;

        protected float mass;
        protected TValue velocity;
        protected TValue drag;

        protected TValue resultant;

        abstract public void AddForce(float _force);
        abstract public void Step(float _timeStep);
    }
}
