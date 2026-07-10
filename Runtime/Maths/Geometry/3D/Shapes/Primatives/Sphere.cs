using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Sphere : IShape3D, ISDFShape
    {
        [field : SerializeField] public Transform3D Transform { get; set; }
        [Min(0.0f)]public float radius;

        #region Constructors
        public Sphere(Transform3D _transform, float _radius) {
            Transform = _transform;
            radius = _radius;
        }
        public Sphere(Vector3 _centre, float _radius) {
            Transform = new(_centre);
            radius = _radius;
        }
        #endregion

        #region Operators
        public static bool operator == (Sphere _a, Sphere _b) { return _a.Transform == _b.Transform && _a.radius == _b.radius; }

        public static bool operator != (Sphere _a, Sphere _b) { return !(_a.Transform == _b.Transform && _a.radius == _b.radius); }
    
        readonly public override bool Equals(object obj) {
            return ((Sphere)obj == this) && obj != null && obj is Sphere;
        }
        
        readonly public override int GetHashCode() { return System.HashCode.Combine(Transform, radius); }
        #endregion

        #region Sphere
        readonly public float GetArea() { return 4.0f * Mathf.PI * radius * radius; }
        readonly public bool Contains(Vector3 _point) { return Vector3.Distance(_point, Transform.position) < radius; }
        #endregion

        #region ISDFShape
        readonly public float GetSignedDistance(Vector3 _point) {
            return Vector3.Distance(_point, Transform.position) - radius;
        }
        #endregion

        #region IInterpolation
        static public Sphere Lerp(Sphere _a, Sphere _b, float _time) {
            return new(Transform3D.Lerp(_a.Transform, _b.Transform, _time), Mathf.Lerp(_a.radius, _b.radius, _time));
        }
        #endregion
    }
}