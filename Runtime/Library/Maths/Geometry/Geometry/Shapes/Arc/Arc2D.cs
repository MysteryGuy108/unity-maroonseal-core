using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    [System.Serializable]
    public struct Arc2D : IArc<Vector2, Transform2D>
    {
        [field : SerializeField] public Transform2D Transform { get; set; }
        [Space]
        public float radius;
        [Range(-180.0f, 180.0f)] public float startDegrees;
        [Range(-180.0f, 180.0f)] public float endDegrees;

        readonly public float Length => Mathf.Abs(RadiansDelta * radius);

        readonly public float DegreesDelta => Mathf.DeltaAngle(startDegrees, endDegrees);
        readonly public float RadiansDelta => DegreesDelta * Mathf.Deg2Rad;

        public float StartRadians {
            readonly get => startDegrees * Mathf.Deg2Rad;
            set => startDegrees = value * Mathf.Rad2Deg;
        }

        public float EndRadians {
            readonly get => endDegrees * Mathf.Deg2Rad;
            set => endDegrees = value * Mathf.Rad2Deg;
        }

        #region Constructors
        public Arc2D(Transform2D _transform, float _radius, float _startDegrees, float _endDegrees) {
            this.Transform = _transform;
            this.radius = _radius;
            this.startDegrees = _startDegrees;
            this.endDegrees = _endDegrees;
        }

        public Arc2D(float _radius, float _startDegrees, float _endDegrees) {
            this.Transform = new(Vector2.zero);
            this.radius = _radius;
            this.startDegrees = _startDegrees;
            this.endDegrees = _endDegrees;
        }
        #endregion

        #region IShape3D
        public readonly bool ContainsPoint(Vector2 _point)
        {
            Vector2 localPoint = Transform.InverseTransformPoint(_point);
            float theta = Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;
            return localPoint.magnitude < radius && theta >= Mathf.Min(startDegrees, endDegrees) && theta <= Mathf.Max(startDegrees, endDegrees);
        }
        #endregion

        #region IPolarSpaceShape
        readonly public bool IsLoop => Mathf.Repeat(startDegrees, 360.0f) == Mathf.Repeat(endDegrees, 360.0f);
        
        readonly public Vector2 EvaluatePointAtTheta(float _theta) =>
            Transform.TransformPoint(Vector2Maths.FromRadians(_theta, radius));

        readonly public Vector2 EvaluateTangentAtTheta(float _theta) =>
            Transform.TransformVector(Circle2D.GetTangentAtTheta(_theta));

        readonly public Vector2 EvaluatePositionAtTime(float _time) {
            float lerpTheta = Mathf.Lerp(startDegrees, endDegrees, _time) * Mathf.Deg2Rad;
            return EvaluatePointAtTheta(lerpTheta);
        }

        readonly public Vector2 EvaluateTangentAtTime(float _time) {
            float lerpTheta = Mathf.Lerp(startDegrees, endDegrees, _time) * Mathf.Deg2Rad;
            return EvaluateTangentAtTheta(lerpTheta);
        }

        readonly public float ClosestTimeToPosition(Vector2 _position)
        {
            Vector2 projectedPosition = this.Transform.InverseTransformPoint(_position);
            PolarVector2 polar = new(projectedPosition);

            float rangeMin = Mathf.Min(this.startDegrees, this.endDegrees);
            float rangeMax = Mathf.Max(this.startDegrees, this.endDegrees);
            float rangeSpan = rangeMax - rangeMin;

            // Bring the point's angle into the [rangeMin, rangeMin + 360) window so it's
            // directly comparable to the arc's (possibly reversed) angular span.
            float shifted = rangeMin + Mathf.Repeat(polar.Degrees - rangeMin, 360.0f);

            float clampedDegrees;
            if (shifted <= rangeMax) {
                // Point's angle falls within the arc's span - it's already the closest angle.
                clampedDegrees = shifted;
            } else {
                // Point falls in the gap outside the arc. Snap to whichever end of the
                // arc is angularly nearer by bisecting the gap.
                float gapMidpoint = rangeMax + (360.0f - rangeSpan) * 0.5f;
                clampedDegrees = (shifted < gapMidpoint) ? rangeMax : rangeMin;
            }

            // InverseLerp works correctly whether startDegrees < endDegrees or not.
            return Mathf.InverseLerp(this.startDegrees, this.endDegrees, clampedDegrees);
        }
        #endregion
    }
}
