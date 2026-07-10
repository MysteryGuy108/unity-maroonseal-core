using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry {
    [System.Serializable]
    public struct Rectangle2D : IPolygon2D, ISDF2D, IEquatable<Rectangle2D>
    {
        [field : SerializeField] public Transform2D Transform {get; set; }
        public Vector2 dimensions;
        public readonly int VertexCount => 4;

        private readonly Vector2 HalfExtents => dimensions / 2.0f;

        public readonly Vector2 Point1 => Transform.TransformPoint(new(HalfExtents.x, HalfExtents.y));
        public readonly Vector2 Point2 => Transform.TransformPoint(new(-HalfExtents.x, HalfExtents.y));
        public readonly Vector2 Point3 => Transform.TransformPoint(new(-HalfExtents.x, -HalfExtents.y));
        public readonly Vector2 Point4 => Transform.TransformPoint(new(HalfExtents.x, -HalfExtents.y));

        #region Constructors
        public Rectangle2D(Transform2D _transform, Vector2 _dimensions) {
            Transform = _transform;
            dimensions = _dimensions;
        }

        public Rectangle2D(Vector2 _cornerA, Vector2 _cornerB) {
            Transform = new((_cornerA + _cornerB) / 2.0f);
            dimensions = (_cornerB - _cornerA).Abs();
        }

        public Rectangle2D(Vector2 _dimensions) {
            Transform = Transform2D.Origin;
            dimensions = _dimensions;
        }
        #endregion

        #region IEquatable
        readonly public bool Equals(Rectangle2D _other) => this.Transform == _other.Transform && this.dimensions == _other.dimensions;
        public override readonly bool Equals(object obj) => obj != null && obj is Rectangle2D && ((Rectangle2D)obj).Equals(this);

        public override readonly int GetHashCode() {
            unchecked { return HashCode.Combine(Transform, dimensions); }
        }
        #endregion

        #region Operators
        public static bool operator ==(Rectangle2D _a, Rectangle2D _b) => _a.Equals(_b);
        public static bool operator !=(Rectangle2D _a, Rectangle2D _b) => !_a.Equals(_b);
        #endregion

        #region Casting
        public static explicit operator Box(Rectangle2D _box2D) => new(_box2D.Transform.ToXY(), _box2D.dimensions);
        #endregion

        #region IShape2D
        readonly public bool ContainsPoint(Vector2 _position)
        {
            _position = Transform.InverseTransformPoint(_position);
            Vector2 halfSize = dimensions * 0.5f;

            return _position.x >= -halfSize.x && _position.x <= halfSize.x &&
                _position.y >= -halfSize.y && _position.y <= halfSize.y;
        }
        #endregion

        #region IPolygon2D
        public readonly IEnumerable<Vector2> GetVertices()
        {
            yield return Point1;
            yield return Point2;
            yield return Point3;
            yield return Point4;
        }

        public readonly IEnumerable<Line2D> GetEdges()
        {
            yield return new Line2D(Point1, Point2);
            yield return new Line2D(Point2, Point3);
            yield return new Line2D(Point3, Point4);
            yield return new Line2D(Point4, Point1);
        }
        #endregion

        #region ISDF2D
        public readonly float GetSignedDistance(Vector2 _point) {
            _point = Transform.InverseTransformPoint(_point);

            Vector2 d = _point.Abs() - dimensions;
            return d.Max(0.0f).magnitude + Mathf.Min(Mathf.Max(d.x,d.y), 0.0f);
        }
        #endregion
    }
}