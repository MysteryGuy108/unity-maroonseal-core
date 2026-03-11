using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Sphere : ISDFShape
    {
        public PointTransform transform;
        [Min(0.0f)]public float radius;

        #region Constructors
        public Sphere(PointTransform _transform, float _radius) {
            transform = _transform;
            radius = _radius;
        }
        public Sphere(Vector3 _centre, float _radius) {
            transform = new(_centre);
            radius = _radius;
        }
        #endregion

        #region Operators
        public static bool operator == (Sphere _a, Sphere _b) { return _a.transform == _b.transform && _a.radius == _b.radius; }

        public static bool operator != (Sphere _a, Sphere _b) { return !(_a.transform == _b.transform && _a.radius == _b.radius); }
    
        readonly public override bool Equals(object obj) {
            return ((Sphere)obj == this) && obj != null && obj is Sphere;
        }
        
        readonly public override int GetHashCode() { return System.HashCode.Combine(transform, radius); }
        #endregion

        #region Shape3D
        public void Rotate(Quaternion _rotation) {
            this.transform.position = _rotation * transform.position;
            this.transform.Rotation = _rotation * transform.Rotation;
        }

        public void Translate(Vector3 _translation) =>
            this.transform.position += _translation;
        #endregion

        #region Sphere
        readonly public float GetArea() { return 4.0f * Mathf.PI * radius * radius; }
        readonly public bool IsPointInSphere(Vector3 _point) { return Vector3.Distance(_point, transform.position) < radius; }
        #endregion

        #region ISDFShape
        readonly public float GetSignedDistance(Vector3 _point) {
            return Vector3.Distance(_point, transform.position) - radius;
        }
        #endregion

        #region IInterpolation
        static public Sphere Lerp(Sphere _a, Sphere _b, float _time) {
            return new(PointTransform.Lerp(_a.transform, _b.transform, _time), Mathf.Lerp(_a.radius, _b.radius, _time));
        }
        #endregion
    }
}