using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Circle : IShape3D, IPolarShape3D
    {
        [field : SerializeField] public Transform3D Transform { get; set; }
        [Min(0.0f)] public float radius;

        public float Circumference {
            readonly get { return 2.0f * Mathf.PI * radius; }
            set { radius = value / (2.0f * Mathf.PI); }
        }

        #region Constructors
        public Circle(Transform3D _transform, float _radius) {
            Transform = _transform;
            radius = _radius;
        }

        public Circle(Vector3 _position, float _radius) : this(new Transform3D(_position), _radius) {}
    
        public Circle(Vector3 _position, Quaternion _rotation, float _radius) : this(new Transform3D(_position, _rotation), _radius) {}

        public Circle(Vector3 _position, Vector3 _normal, float _radius) : 
            this(new Transform3D(_position, Quaternion.FromToRotation(Vector3.up, _normal), Vector3.one), _radius) {}
        #endregion
        
        #region Operators
        public static bool operator == (Circle _a, Circle _b) {
            return _a.Transform == _b.Transform && _a.radius == _b.radius;
        }

        public static bool operator != (Circle _a, Circle _b) {
            return !(_a.Transform == _b.Transform && _a.radius == _b.radius);
        }
    
        readonly public override bool Equals(object obj) {
            if (obj == null) { return false; }
            if (obj is not Circle) { return false; }
            return (Circle)obj == this;
        }
        readonly public override int GetHashCode() { return System.HashCode.Combine(Transform, radius); }
        #endregion

        #region Casting
        public static implicit operator Circle2D(Circle _circle) => new(_circle.Transform, _circle.radius);
        #endregion

        #region Circle
        readonly public bool Contains(Vector3 _point)
        {
            Vector3 localPoint = Transform.InverseTransformPosition(_point);
            localPoint.z = 0.0f;
            return localPoint.magnitude <= radius;
        }
        #endregion

        #region IPolarSpaceShape
        readonly public Vector3 EvaluatePointAtTheta(float _radians) => PolarMath.GetCartesianPosition(Transform, radius, _radians);

        readonly public Vector3 EvaluateTangentAtTheta(float _radians) {
            return Transform.TransformDirection(PolarMath.GetCircleTangent(_radians));   
        }

        readonly public Vector3 EvaluatePointAtTime(float _t) => EvaluatePointAtTheta(_t * Mathf.PI * 2.0f);

        readonly public Vector3 EvaluateTangentAtTime(float _t) => EvaluateTangentAtTheta(_t * Mathf.PI * 2.0f);
        #endregion
    }
}