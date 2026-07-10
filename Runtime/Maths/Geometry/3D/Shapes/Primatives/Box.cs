using UnityEngine;

namespace MaroonSeal.Maths.Geometry {
    public struct Box : IShape3D, ISDF3D
    {
        [SerializeField] private Transform3D transform;
        public Transform3D Transform
        {
            readonly get => transform;
            set => transform = value;
        }

        public Vector3 dimensions;

        #region Constructors
        public Box(Transform3D _transform, Vector3 _dimensions) {
            transform = _transform;
            dimensions = _dimensions;
        }

        public Box(Vector3 _centre, Vector3 _size) {
            transform = new(_centre);
            dimensions = _size;
        }

        public Box(Vector3 _size) {
            transform = Transform3D.Origin;
            dimensions = _size;
        }
        #endregion

        #region Operators
        readonly public bool Equals(Box _other) {
            return this.transform == _other.transform && 
                this.dimensions == _other.dimensions;
        }
        public override readonly bool Equals(object obj) => obj is Box && this.Equals((Box)obj);

        readonly public override int GetHashCode() { return System.HashCode.Combine(transform, dimensions); }

        public static bool operator ==(Box _a, Box _b) => _a.Equals(_b);
        public static bool operator !=(Box _a, Box _b) => !_a.Equals(_b);
        #endregion

        #region Casting
        public static implicit operator Rectangle2D(Box _box) => new(_box.transform, _box.dimensions);
        #endregion

        #region Shape3D
        public void Rotate(Quaternion _rotation)
        {
            this.transform.position = _rotation * transform.position;
            this.transform.rotation = _rotation * transform.rotation;
        }

        public void Translate(Vector3 _translation) =>
            this.transform.position += _translation;
        #endregion

        #region Box
        readonly public bool ContainsPoint(Vector3 _point)
        {
            _point = transform.InverseTransformPoint(_point);
            Vector3 halfSize = dimensions * 0.5f;

            return _point.x >= -halfSize.x && _point.x <= halfSize.x &&
                _point.y >= -halfSize.y && _point.y <= halfSize.y &&
                _point.z >= -halfSize.z && _point.z <= halfSize.z;
        }
        #endregion

        #region ISDFShape
        readonly public float GetSignedDistance(Vector3 _point) {
            _point = transform.InverseTransformPoint(_point);
            Vector3 q = _point.Abs() - dimensions;
            return q.Max(0.0f).magnitude + Mathf.Min(Mathf.Max(q.x, Mathf.Max(q.y, q.z)), 0.0f);
        }
        #endregion
    }
}