using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry {
    [System.Serializable]
    public struct Line : ILine<Vector3>
    {
        readonly public bool IsLoop => false;
        
        public Vector3 p1;
        public Vector3 p2;

        readonly public float Length => Vector3.Distance(p1, p2);

        #region Constructors
        public Line(Vector3 _pointA, Vector3 _pointB) {
            p1 = _pointA;
            p2 = _pointB;
        }
        #endregion
        
        #region Operators
        public static bool operator == (Line _a, Line _b) { return _a.p1 == _b.p1 && _a.p2 == _b.p2; }

        public static bool operator != (Line _a, Line _b) { return !(_a.p1 == _b.p1 && _a.p2 == _b.p2); }
    
        readonly public override bool Equals(object obj) {
            if (obj == null || obj is not Line) { return false; }
            return (Line)obj == this;
        }

        readonly public override int GetHashCode() { return System.HashCode.Combine(p1, p2); }
        #endregion

        #region Casting
        public static implicit operator Line2D(Line _line) => new(_line.p1, _line.p2);
        #endregion

        #region ILine
        public void SetPoints(Vector3 _p1, Vector3 _p2) { p1 = _p1; p2 = _p2; }
        public readonly Vector3 GetDelta() => p2 - p1;
        public readonly Vector3 GetDirection() => GetDelta().normalized;

        public bool IsPositionInBoundingBox(Vector3 _position) {
            throw new NotImplementedException();
        }
        #endregion

        public void Rotate(Quaternion _rotation) {
            p1 = _rotation * p1;
            p2 = _rotation * p2;
        }

        public void Translate(Vector3 _translation) {
            p1 += _translation;
            p2 += _translation;
        }

        #region ICurve
        public readonly Vector3 EvaluatePositionAtTime(float _time) { return Vector3.Lerp(p1, p2, _time); }
        public readonly Vector3 EvaluateTangentAtTime(float _time) { return GetDirection(); }

        public readonly float ClosestTimeToPosition(Vector3 _position)
        {
            Vector3 AB = p2 - p1;
            Vector3 AV = _position - p1;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }
        #endregion
    }
}