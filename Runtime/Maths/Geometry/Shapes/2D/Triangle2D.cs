using System;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Triangle2D : IShape2D, IPolygon, ISDFShape
    {
        public readonly PointTransform2D Transform => PointTransform2D.Origin;

        public Vector2 pointA;
        public Vector2 pointB;
        public Vector2 pointC;

        #region Constructors
        public Triangle2D(Vector2 _pointA, Vector2 _pointB, Vector2 _pointC) {
            pointA = _pointA;
            pointB = _pointB;
            pointC = _pointC;
        }
        #endregion

        #region Operators
        public static bool operator == (Triangle2D _a, Triangle2D _b) {
            return _a.pointA == _b.pointA && _a.pointB == _b.pointB && _a.pointC == _b.pointC;
        }

        public static bool operator != (Triangle2D _a, Triangle2D _b) {
            return !(_a.pointA == _b.pointA && _a.pointB == _b.pointB && _a.pointC == _b.pointC);
        }
    
        readonly public bool Equals(Triangle2D _other) {
            return this.pointA == _other.pointA &&
                this.pointB == _other.pointB &&
                this.pointC == _other.pointC;
        }
        public override readonly bool Equals(object obj) => this.Equals((Triangle2D)obj);
        readonly public override int GetHashCode() { return System.HashCode.Combine(pointA, pointB, pointC); }
        #endregion

        #region Casting
        public static explicit operator Triangle(Triangle2D _triangle) => new(_triangle.pointA, _triangle.pointB, _triangle.pointC);
        #endregion

        #region Shape2D
        public void Rotate(float _rotation)
        {
            Quaternion rotation = Quaternion.AngleAxis(_rotation, Vector3.forward);
            pointA = rotation * pointA;
            pointB = rotation * pointB;
            pointC = rotation * pointC;
        }

        public void Translate(Vector2 _translation)
        {
            pointA += _translation;
            pointB += _translation;
            pointC += _translation;
        }
        #endregion

        #region Triangle2D
        readonly public bool ContainsPoint(Vector2 _point) { return _point == pointA || _point == pointB || _point == pointC; }

        readonly public float GetLengthAB() { return Vector2.Distance(pointA, pointB); }
        readonly public float GetLengthBC() { return Vector2.Distance(pointB, pointC); }
        readonly public float GetLengthCA() { return Vector2.Distance(pointC, pointA); }

        readonly public Line2D GetEdgeAB() { return new Line2D(pointA, pointB); }
        readonly public Line2D GetEdgeBC() { return new Line2D(pointB, pointC); }
        readonly public Line2D GetEdgeCA() { return new Line2D(pointC, pointA); }

        readonly public Circle2D GetCircumcircle() {
            float d = 2 * (pointA.x * (pointB.y - pointC.y) + pointB.x * (pointC.y - pointA.y) + pointC.x * (pointA.y - pointB.y));

            float ux = ((pointA.x * pointA.x + pointA.y * pointA.y) * (pointB.y - pointC.y) + (pointB.x * pointB.x + pointB.y * pointB.y) * (pointC.y - pointA.y) + (pointC.x * pointC.x + pointC.y * pointC.y) * (pointA.y - pointB.y)) / d;
            float uy = ((pointA.x * pointA.x + pointA.y * pointA.y) * (pointC.x - pointB.x) + (pointB.x * pointB.x + pointB.y * pointB.y) * (pointA.x - pointC.x) + (pointC.x * pointC.x + pointC.y * pointC.y) * (pointB.x - pointA.x)) / d;

            Vector2 circumCentre = new(ux, uy);
            float circumRadius = Vector2.Distance(pointA, circumCentre);

            return new Circle2D(circumRadius);
        }
        #endregion

        #region IPolygon
        public readonly int VertexCount => 3;
        readonly public Vector2[] GetVertices() => new Vector2[3] { pointA, pointB, pointC };
        readonly public Line2D[] GetEdges() => new Line2D[3] { GetEdgeAB(), GetEdgeBC(), GetEdgeCA() };

        readonly Vector3[] IPolygon.GetVertices() => new Vector3[3] { pointA, pointB, pointC };
        readonly Line[] IPolygon.GetEdges() => new Line[3] { (Line)GetEdgeAB(), (Line)GetEdgeBC(), (Line)GetEdgeCA() };
        #endregion

        #region ISDFShape
        readonly public float GetSignedDistance(Vector3 _position) {
            Vector2 p = _position;
            Vector2 e0 = pointB-pointA, e1 = pointC-pointB, e2 = pointA-pointC;
            Vector2 v0 = p - pointA, v1 = p -pointB, v2 = p-pointC;

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