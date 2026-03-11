using System;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Box2D : IShape2D, ISDFShape
    {
        public PointTransform2D transform;
        readonly public PointTransform2D Transform => transform;

        public Vector2 dimensions;

        #region Constructors
        public Box2D(PointTransform2D _transform, Vector2 _dimensions) {
            transform = _transform;
            dimensions = _dimensions;
        }

        public Box2D(Vector2 _cornerA, Vector2 _cornerB) {
            transform = new((_cornerA + _cornerB) / 2.0f);
            dimensions = (_cornerB - _cornerA).Abs();
        }

        public Box2D(Vector2 _dimensions) {
            transform = PointTransform2D.Origin;
            dimensions = _dimensions;
        }
        #endregion

        #region Operators
        readonly public bool Equals(Box2D _other) {
            return this.transform == _other.transform && 
                this.dimensions == _other.dimensions;
        }
        public override readonly bool Equals(object obj) => this.Equals((Box2D)obj);

        public override readonly int GetHashCode() {
            unchecked { return HashCode.Combine(transform, dimensions); }
        }

        public static bool operator ==(Box2D _a, Box2D _b) => _a.Equals(_b);
        public static bool operator !=(Box2D _a, Box2D _b) => !_a.Equals(_b);
        #endregion

        #region Casting
        public static explicit operator Box(Box2D _box2D) => new(_box2D.transform.ToXY(), _box2D.dimensions);
        #endregion

        #region Shape2D
        public void Rotate(float _rotation) =>
            transform.angle += _rotation;

        public void Translate(Vector2 _translation) =>
            transform.position += _translation;
        #endregion

        #region Box2D
        readonly public bool IsPositionInBounds(Vector2 _position)
        {
            _position = transform.InverseTransformPosition(_position);
            Vector2 halfSize = dimensions * 0.5f;

            return _position.x >= -halfSize.x && _position.x <= halfSize.x &&
                _position.y >= -halfSize.y && _position.y <= halfSize.y;
        }
        #endregion

        #region ISDFShape
        public readonly float GetSignedDistance(Vector3 _position) {
            _position = transform.InverseTransformPosition(_position);

            Vector2 d = (Vector2)_position.Abs() - dimensions;
            return d.Max(0.0f).magnitude + Mathf.Min(Mathf.Max(d.x,d.y), 0.0f);
        }


        #endregion
    }
}