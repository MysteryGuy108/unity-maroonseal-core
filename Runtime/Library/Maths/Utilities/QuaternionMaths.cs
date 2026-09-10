using UnityEngine;

namespace MaroonSeal.Maths
{
    public struct QuaternionMaths
    {
        /// <summary>
        /// Calculates the rotation angle (in degrees) of a rotation around a specific direction.
        /// </summary>
        /// <param name="rotation">The rotation.</param>
        /// <param name="direction">The axis or direction you want to check the angle around.</param>
        /// <returns>Signed angle in degrees between -180 and 180.</returns>
        public static float GetAngleAroundDirection(Quaternion rotation, Vector3 direction)
        {
            // Ensure the direction vector is normalized
            Vector3 axis = direction.normalized;

            // Grab the imaginary vector part of the quaternion (X, Y, Z)
            Vector3 quaternionVector = new Vector3(rotation.x, rotation.y, rotation.z);

            // Project the quaternion's vector component onto our target axis
            Vector3 twistedAxis = Vector3.Project(quaternionVector, axis);

            // Reconstruct a new "Twist" quaternion representing only rotation around this axis
            // We use the original W component for the scalar part
            Quaternion twistRotation = new Quaternion(twistedAxis.x, twistedAxis.y, twistedAxis.z, rotation.w).normalized;

            // Extract the angle from this isolated twist rotation
            twistRotation.ToAngleAxis(out float angle, out Vector3 extractedAxis);

            // Make the angle signed relative to our target direction
            // If the extracted axis points opposite to our target direction, flip the angle sign
            if (Vector3.Dot(extractedAxis, axis) < 0f)
            {
                angle = -angle;
            }

            // Clamp the angle to a standard -180 to 180 degree range
            return Mathf.DeltaAngle(0, angle);
        }
    }
}
