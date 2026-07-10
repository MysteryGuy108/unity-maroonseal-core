using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Arc : IShape3D, IPolarShape3D
    {
        [field : SerializeField] public Transform3D Transform { get; set; }
        [Space]
        public float radius;
        public float startDegrees;
        public float endDegrees;

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
        public Arc(Transform3D _transform, float _radius, float _startDegrees, float _endDegrees) {
            this.Transform = _transform;
            this.radius = _radius;
            this.startDegrees = _startDegrees;
            this.endDegrees = _endDegrees;
        }

        public Arc(float _radius, float _startDegrees, float _endDegrees) {
            this.Transform = new(Vector3.zero);
            this.radius = _radius;
            this.startDegrees = _startDegrees;
            this.endDegrees = _endDegrees;
        }
        #endregion

        #region IShape3D
        public readonly bool Contains(Vector3 _position)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IPolarSpaceShape
        readonly public Vector3 EvaluatePointAtTheta(float _theta) =>
            Transform.TransformPosition(PolarMath.ToCartesian(radius, _theta));

        readonly public Vector3 EvaluateTangentAtTheta(float _theta) =>
            Transform.TransformVector(PolarMath.GetCircleTangent(_theta));

        readonly public Vector3 EvaluatePointAtTime(float _time) {
            float lerpTheta = Mathf.Lerp(startDegrees, endDegrees, _time) * Mathf.Deg2Rad;
            return EvaluatePointAtTheta(lerpTheta);
        }

        readonly public Vector3 EvaluateTangentAtTime(float _time) {
            float lerpTheta = Mathf.Lerp(startDegrees, endDegrees, _time) * Mathf.Deg2Rad;
            return EvaluateTangentAtTheta(lerpTheta);
        }
        #endregion
    }
}