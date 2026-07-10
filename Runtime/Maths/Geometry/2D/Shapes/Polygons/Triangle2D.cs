using System;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Triangle2D : IPolygon2D, ISDFShape
    {
        [field : SerializeField] public Transform2D Transform { get; set; }
        public readonly int VertexCount => 3;

        [SerializeField] private Vector2 p1;
        public Vector2 Point1 {
            readonly get => Transform.TransformPosition(p1);
            set => p1 = Transform.InverseTransformPosition(value);
        }

        [SerializeField] private Vector2 p2;
        public Vector2 Point2 {
            readonly get => Transform.TransformPosition(p2);
            set => p2 = Transform.InverseTransformPosition(value);
        }

        [SerializeField] private Vector2 p3;
        public Vector2 Point3 {
            readonly get => Transform.TransformPosition(p3);
            set => p3 = Transform.InverseTransformPosition(value);
        }

        #region Constructors
        public Triangle2D(Vector2 _p1, Vector2 _p2, Vector2 _p3, Transform2D? _transform = null) {
            p1 = _p1; p2 = _p2; p3 = _p3;
            Transform = _transform ?? Transform2D.Origin;
        }
        #endregion

        #region Operators
        public static bool operator == (Triangle2D _a, Triangle2D _b) => _a.p1 == _b.p1 && _a.p2 == _b.p2 && _a.p3 == _b.p3;
        public static bool operator != (Triangle2D _a, Triangle2D _b) => !(_a.p1 == _b.p1 && _a.p2 == _b.p2 && _a.p3 == _b.p3);
    
        readonly public bool Equals(Triangle2D _other) => this.p1 == _other.p1 && this.p2 == _other.p2 && this.p3 == _other.p3;
        public override readonly bool Equals(object obj) => this.Equals((Triangle2D)obj);
        readonly public override int GetHashCode() { return System.HashCode.Combine(Transform, p1, p2, p3); }
        #endregion

        #region Casting
        public static explicit operator Triangle(Triangle2D _triangle) => new(_triangle.p1, _triangle.p2, _triangle.p3);
        #endregion

        #region Triangle2D
        readonly public bool ContainsVertex(Vector2 _point) => _point == p1 || _point == p2 || _point == p3;

        readonly public float GetLengthAB() => Vector2.Distance(p1, p2);
        readonly public float GetLengthBC() => Vector2.Distance(p2, p3);
        readonly public float GetLengthCA() => Vector2.Distance(p3, p1);

        readonly public Line2D GetEdgeAB() => new(Point1, Point2);
        readonly public Line2D GetEdgeBC() => new(Point2, Point3);
        readonly public Line2D GetEdgeCA() => new(Point3, Point1);

        readonly public Circle2D GetCircumcircle() {
            float d = 2 * (p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y));

            float ux = ((p1.x * p1.x + p1.y * p1.y) * (p2.y - p3.y) + (p2.x * p2.x + p2.y * p2.y) * (p3.y - p1.y) + (p3.x * p3.x + p3.y * p3.y) * (p1.y - p2.y)) / d;
            float uy = ((p1.x * p1.x + p1.y * p1.y) * (p3.x - p2.x) + (p2.x * p2.x + p2.y * p2.y) * (p1.x - p3.x) + (p3.x * p3.x + p3.y * p3.y) * (p2.x - p1.x)) / d;

            Vector2 circumCentre = Transform.TransformPosition(new Vector2(ux, uy));
            float circumRadius = Vector2.Distance(p1, circumCentre);

            return new Circle2D(circumCentre, circumRadius);
        }
        #endregion

        #region IShape
        readonly public bool Contains(Vector2 _point)
        {
            Vector2 p = Transform.InverseTransformPosition(_point);
            float denominator = (p2.y - p3.y) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.y - p3.y);
            if (denominator == 0.0f) { return false; }

            float w_A = ((p2.y - p3.y) * (p.x - p3.x) + (p3.x - p2.x) * (p.y - p3.y)) / denominator;
            float w_B = ((p3.y - p1.y) * (p.x - p3.x) + (p1.x - p3.x) * (p.y - p3.y)) / denominator;
            float w_C = 1 - w_A - w_B;
            return w_A >= 0 && w_B >= 0 && w_C >= 0;
        }
        #endregion

        #region IPolygon
        readonly public IEnumerable<Vector2> GetVertices()
        {
            yield return Point1;
            yield return Point2;
            yield return Point3;
        }

        readonly public IEnumerable<Line2D> GetEdges()
        {
            yield return GetEdgeAB();
            yield return GetEdgeBC();
            yield return GetEdgeCA();
        }
        #endregion

        #region ISDFShape
        readonly public float GetSignedDistance(Vector3 _position) {
            Vector2 p = _position;
            Vector2 e0 = p2-p1, e1 = p3-p2, e2 = p1-p3;
            Vector2 v0 = p - p1, v1 = p -p2, v2 = p-p3;

            Vector2 pq0 = v0 - e0 * Mathf.Clamp(Vector2.Dot(v0,e0) / Vector2.Dot(e0, e0), 0.0f, 1.0f);
            Vector2 pq1 = v1 - e1 * Mathf.Clamp(Vector2.Dot(v1,e1) / Vector2.Dot(e1, e1), 0.0f, 1.0f);
            Vector2 pq2 = v2 - e2 * Mathf.Clamp(Vector2.Dot(v2,e2) / Vector2.Dot(e2,e2), 0.0f, 1.0f);
            float s = Mathf.Sign( e0.x * e2.y - e0.y * e2.x );

            Vector2 a = new(Vector2.Dot(pq0,pq0), s*(v0.x*e0.y-v0.y*e0.x));
            Vector2 b = new (Vector2.Dot(pq1,pq1), s*(v1.x*e1.y-v1.y*e1.x));
            Vector2 c = new (Vector2.Dot(pq2,pq2), s*(v2.x*e2.y-v2.y*e2.x));

            Vector2 d = Vector2.Min(Vector2.Min(a, b), c);

            return -Mathf.Sqrt(d.x) * Mathf.Sign(d.y);
        }
        #endregion
    }
}