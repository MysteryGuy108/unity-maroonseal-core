using UnityEngine;

namespace MaroonSeal
{
    public class PhysicsBasedFloat : PhysicsBasedValue<float>
    {
        override public void AddForce(float _force) => resultant += _force;

        override public void Step(float _timeStep)
        {
            velocity += resultant / mass * _timeStep;
            velocity -= velocity * drag;
            value += velocity * _timeStep;
        }
    }
}
