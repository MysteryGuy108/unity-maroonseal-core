using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Circle : IShape3D, IPolarShape
    {
        public PointTransform transform;
        readonly public PointTransform Transform => transform;

        [Min(0.0f)] public float radius;

        public float Circumference {
            readonly get { return 2.0f * Mathf.PI * radius; }
            set { radius = value / (2.0f * Mathf.PI); }
        }

        #region Constructors
        public Circle(PointTransform _transform, float _radius) {
            transform = _transform;
            radius = _radius;
        }

        public Circle(Vector3 _position, float _radius) {
            transform = new(_position, Quaternion.identity, Vector3.one);
            radius = _radius;
        }

        public Circle(Vector3 _position, Quaternion _rotation, float _radius) {
            transform = new(_position, _rotation, Vector3.one);
            radius = _radius;
        }

        public Circle(Vector3 _position, Vector3 _normal, float _radius) {
            transform = new(_position, Quaternion.FromToRotation(Vector3.up, _normal), Vector3.one);
            radius = _radius;
        }
        #endregion
        
        #region Operators
        public static bool operator == (Circle _a, Circle _b) {
            return _a.transform == _b.transform && _a.radius == _b.radius;
        }

        public static bool operator != (Circle _a, Circle _b) {
            return !(_a.transform == _b.transform && _a.radius == _b.radius);
        }
    
        readonly public override bool Equals(object obj) {
            if (obj == null) { return false; }
            if (obj is not Circle) { return false; }
            return (Circle)obj == this;
        }
        readonly public override int GetHashCode() { return System.HashCode.Combine(transform, radius); }
        #endregion

        #region Casting
        public static implicit operator Circle2D(Circle _circle) => new(_circle.transform, _circle.radius);
        #endregion

        #region Shape3D
        public void Rotate(Quaternion _rotation) {
            this.transform.position = _rotation * transform.position;
            this.transform.Rotation = _rotation * transform.Rotation;
        }

        public void Translate(Vector3 _translation) =>
            this.transform.position += _translation;
        #endregion

        #region Circle
        readonly public bool IsPositionInCircle(Vector3 _point)
        {
            Vector3 localPoint = transform.InverseTransformPosition(_point);
            localPoint.z = 0.0f;
            return localPoint.magnitude <= radius;
        }
        #endregion

        #region IPolarSpaceShape
        readonly public Vector3 EvaluatePositionAtTheta(float _radians) => IPolarShape.GetCartesianPosition(transform, radius, _radians);

        readonly public Vector3 EvaluateTangentAtTheta(float _radians) {
            return transform.TransformDirection(IPolarShape.GetCircleTangent(_radians));   
        }

        readonly public Vector3 EvaluatePositionAtTime(float _t) => EvaluatePositionAtTheta(_t * Mathf.PI * 2.0f);

        readonly public Vector3 EvaluateTangentAtTime(float _t) => EvaluateTangentAtTheta(_t * Mathf.PI * 2.0f);
        #endregion
    }
}