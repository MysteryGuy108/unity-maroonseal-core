using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry 
{
    public struct Capsule : IShape3D, ISDF3D
    {
        [SerializeField] private Transform3D transform;
        public Transform3D Transform
        {
            readonly get => transform;
            set => transform = value;
        }

        public enum Axis { XAxis, YAxis, ZAxis }

        public float height;
        public float radius;
        public Axis axis;

        #region Constructors
        public Capsule(Transform3D _transform, float _height, float _radius, Axis _axis) {
            transform = _transform;
            height = _height;
            radius = _radius;
            axis = _axis;
        }

        public Capsule(Vector3 _p1, Vector3 _p2, float _radius) {

            Vector3 delta = _p1 - _p2;

            transform = new((_p1 + _p2) / 2.0f) {
                Up = delta.normalized
            };
            radius = _radius;

            height = delta.magnitude + radius * 2.0f;
            axis = Axis.YAxis;
        }
        #endregion

        #region Shape3D
        public bool ContainsPoint(Vector3 _point)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Capsule
        readonly public (Vector3, Vector3) GetFoci()
        {
            float fociDistance = (0.5f * height) - radius;
            Vector3 p1 = transform.TransformPoint(GetAxis(axis) * fociDistance);
            Vector3 p2 = transform.TransformPoint(-GetAxis(axis) * fociDistance);
            return (p1, p2);
        }
        #endregion

        #region ISDFShape
        readonly public float GetSignedDistance(Vector3 _point) {
            _point = transform.InverseTransformPoint(_point);

            (Vector3, Vector3) foci = GetFoci();

            Vector3 pa = _point - foci.Item1;
            Vector3 ba = foci.Item2 - foci.Item1;

            float h = Mathf.Clamp(Vector3.Dot(pa,ba)/Vector3.Dot(ba,ba), 0.0f, 1.0f);
            return (pa - ba*h).magnitude - radius;
        }
        #endregion

        static public Vector3 GetAxis(Axis _axis) => _axis switch {
            Axis.XAxis => Vector3.right,
            Axis.YAxis => Vector3.up,
            Axis.ZAxis => Vector3.forward,
            _ => Vector3.zero,
        };
    }
}
