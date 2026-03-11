using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Arc : IShape3D, IPolarShape
    {
        public PointTransform transform;
        readonly public PointTransform Transform => transform;
        [Space]
        public float radius;
        public float startDegrees;
        public float endDegrees;

        public float StartRadians {
            readonly get => startDegrees * Mathf.Deg2Rad;
            set => startDegrees = value * Mathf.Rad2Deg;
        }

        public float EndRadians {
            readonly get => endDegrees * Mathf.Deg2Rad;
            set => endDegrees = value * Mathf.Rad2Deg;
        }

        #region Constructors
        public Arc(PointTransform _transform, float _radius, float _startDegrees, float _endDegrees) {
            this.transform = _transform;
            this.radius = _radius;
            this.startDegrees = _startDegrees;
            this.endDegrees = _endDegrees;
        }
        #endregion

        readonly public float GetLength() => Mathf.Abs(GetRadiansDelta() * radius);

        readonly public float GetDegreesDelta() => Mathf.DeltaAngle(startDegrees, endDegrees);
        readonly public float GetRadiansDelta() => GetDegreesDelta() * Mathf.Deg2Rad;

        #region Shape3D
        public void Rotate(Quaternion _rotation)
        {
            this.transform.position = _rotation * transform.position;
            this.transform.Rotation = _rotation * transform.Rotation;
        }

        public void Translate(Vector3 _translation) =>
            this.transform.position += _translation;
        #endregion

        #region IPolarSpaceShape
        readonly public Vector3 EvaluatePositionAtTheta(float _theta) =>
            transform.TransformPosition(IPolarShape.ToCartesian(radius, _theta));

        readonly public Vector3 EvaluateTangentAtTheta(float _theta) =>
            transform.TransformVector(IPolarShape.GetCircleTangent(_theta));

        readonly public Vector3 EvaluatePositionAtTime(float _time) {
            float lerpTheta = Mathf.Lerp(startDegrees, endDegrees, _time) * Mathf.Deg2Rad;
            return EvaluatePositionAtTheta(lerpTheta);
        }

        readonly public Vector3 EvaluateTangentAtTime(float _time) {
            float lerpTheta = Mathf.Lerp(startDegrees, endDegrees, _time) * Mathf.Deg2Rad;
            return EvaluateTangentAtTheta(lerpTheta);
        }
        #endregion
    }
}