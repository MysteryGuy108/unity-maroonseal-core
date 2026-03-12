using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;

namespace MaroonSeal.Maths.Geometry.Shapes 
{
    public struct Capsule : IShape3D, ISDFShape 
    {
        public enum Axis { XAxis, YAxis, ZAxis }
        public PointTransform transform;
        readonly public PointTransform Transform => transform;

        public float height;
        public float radius;
        public Axis axis;

        #region Constructors
        public Capsule(PointTransform _transform, float _height, float _radius, Axis _axis) {
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
        public void Rotate(Quaternion _rotation)
        {
            this.transform.position = _rotation * transform.position;
            this.transform.Rotation = _rotation * transform.Rotation;
        }

        public void Translate(Vector3 _translation) => this.transform.position += _translation;
        #endregion

        #region Capsule
        readonly public (Vector3, Vector3) GetFoci()
        {
            float fociDistance = (0.5f * height) - radius;
            Vector3 p1 = transform.TransformPosition(GetAxis(axis) * fociDistance);
            Vector3 p2 = transform.TransformPosition(-GetAxis(axis) * fociDistance);
            return (p1, p2);
        }
        #endregion

        #region ISDFShape
        readonly public float GetSignedDistance(Vector3 _point) {
            _point = transform.InverseTransformPosition(_point);

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
