using System;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;


namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Triangle : IShape3D, IPolygon3D, ISDFShape
    {
        [field : SerializeField] public Transform3D Transform { get; set; }

        [SerializeField] private Vector3 p1;
        public Vector3 Point1 {
            readonly get => Transform.TransformPosition(p1);
            set => p1 = Transform.InverseTransformPosition(value);
        }

        [SerializeField] private Vector3 p2;
        public Vector3 Point2 {
            readonly get => Transform.TransformPosition(p2);
            set => p2 = Transform.InverseTransformPosition(value);
        }

        [SerializeField] private Vector3 p3;
        public Vector2 Point3 {
            readonly get => Transform.TransformPosition(p3);
            set => p3 = Transform.InverseTransformPosition(value);
        }


        #region Constructors
        public Triangle(Vector3 _p1, Vector3 _p2, Vector3 _p3, Transform3D? _transform = null) {
            p1 = _p1;
            p2 = _p2;
            p3 = _p3;

            Transform = _transform ?? Transform3D.Origin;
        }
        #endregion

        #region Operators
        public static bool operator == (Triangle _a, Triangle _b) {
            return _a.p1 == _b.p1 && _a.p2 == _b.p2 && _a.p3 == _b.p3;
        }

        public static bool operator != (Triangle _a, Triangle _b) {
            return !(_a.p1 == _b.p1 && _a.p2 == _b.p2 && _a.p3 == _b.p3);
        }
    
        readonly public override bool Equals(object obj) {
            return ((Triangle)obj == this) && obj != null && obj is Triangle;
        }
        readonly public override int GetHashCode() { return System.HashCode.Combine(p1, p2, p3); }
        #endregion

        #region Casting
        public static implicit operator Triangle2D(Triangle _triangle) => new(_triangle.p1, _triangle.p2, _triangle.p3);
        #endregion

        #region Triangle
        readonly public bool ContainsVertex(Vector3 _point) { return _point == p1 || _point == p2 || _point == p3; }

        readonly public float GetLengthAB() { return Vector3.Distance(p1, p2); }
        readonly public float GetLengthBC() { return Vector3.Distance(p2, p3); }
        readonly public float GetLengthCA() { return Vector3.Distance(p3, p1); }

        readonly public Line GetEdgeAB() { return new Line(Point1, Point2); }
        readonly public Line GetEdgeBC() { return new Line(Point2, Point3); }
        readonly public Line GetEdgeCA() { return new Line(Point3, Point1); }

        readonly public Vector3 GetNormal() {
            Vector3 deltaA = p2 - p1;
            Vector3 deltaB = p3 - p1;
            return Vector3.Cross(deltaA, deltaB).normalized;
        }

        readonly public Vector3 GetCentroid() { return (p1 + p2 + p3) / 3.0f; }

        readonly public Sphere GetCircumsphere() {
            Vector3 ac = p3 - p1;
            Vector3 ab = p2 - p1;
            Vector3 abXac = Vector3.Cross(ab, ac);

            // this is the vector from a TO the circumsphere center
            Vector3 toCircumsphereCenter = (Vector3.Cross(abXac, ab)*ac.sqrMagnitude + Vector3.Cross(ac, abXac )*ab.sqrMagnitude) / (2.0f*abXac.sqrMagnitude) ;
            float circumsphereRadius = toCircumsphereCenter.magnitude;

            // The 3 space coords of the circumsphere center then:
            Vector3 ccs = p1  +  toCircumsphereCenter; // now this is the actual 3space location
            return new(ccs, circumsphereRadius);
        }
        #endregion

        #region IShape3D
        readonly public bool Contains(Vector3 _point)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IPolygon
        public readonly int VertexCount => 3;
        readonly public IEnumerable<Vector3> GetVertices()
        {
            yield return Point1;
            yield return Point2;
            yield return Point3;
        }

        readonly public IEnumerable<Line> GetEdges()
        {
            yield return GetEdgeAB();
            yield return GetEdgeBC();
            yield return GetEdgeCA();
        }
        #endregion

        #region ISDFShape
        public float GetSignedDistance(Vector3 _point) {
            throw new System.NotImplementedException();
        }
        #endregion

        static public Vector3 GetNormal(Vector3 _p0, Vector3 _p1, Vector3 _p2) {
            Vector3 deltaA = _p1 - _p0;
            Vector3 deltaB = _p2 - _p0;
            return Vector3.Cross(deltaA, deltaB).normalized;
        }
    }
}
