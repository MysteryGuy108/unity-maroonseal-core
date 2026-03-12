using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;

namespace MaroonSeal.Maths.Geometry.Shapes {
    public struct Box : IShape3D, ISDFShape 
    {
        public PointTransform transform;
        readonly public PointTransform Transform => transform;

        public Vector3 dimensions;

        #region Constructors
        public Box(PointTransform _transform, Vector3 _dimensions) {
            transform = _transform;
            dimensions = _dimensions;
        }

        public Box(Vector3 _centre, Vector3 _size) {
            transform = new(_centre);
            dimensions = _size;
        }

        public Box(Vector3 _size) {
            transform = PointTransform.Origin;
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
        public static implicit operator Box2D(Box _box) => new(_box.transform, _box.dimensions);
        #endregion

        #region Shape3D
        public void Rotate(Quaternion _rotation)
        {
            this.transform.position = _rotation * transform.position;
            this.transform.Rotation = _rotation * transform.Rotation;
        }

        public void Translate(Vector3 _translation) =>
            this.transform.position += _translation;
        #endregion

        #region Box
        readonly public bool IsPositionInBounds(Vector2 _position)
        {
            _position = transform.InverseTransformPosition(_position);
            Vector2 halfSize = dimensions * 0.5f;

            return _position.x >= -halfSize.x && _position.x <= halfSize.x &&
                _position.y >= -halfSize.y && _position.y <= halfSize.y;
        }
        #endregion

        #region ISDFShape
        readonly public float GetSignedDistance(Vector3 _point) {
            _point = transform.InverseTransformPosition(_point);
            Vector3 q = _point.Abs() - dimensions;
            return q.Max(0.0f).magnitude + Mathf.Min(Mathf.Max(q.x, Mathf.Max(q.y, q.z)), 0.0f);
        }
        #endregion
    }
}