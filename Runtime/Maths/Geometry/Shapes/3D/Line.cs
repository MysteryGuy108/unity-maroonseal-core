using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Line : IShape3D, IInterpolationShape
    {
        public Vector3 start;
        public Vector3 end;

        readonly public PointTransform Transform => PointTransform.Origin;

        #region Constructors
        public Line(Vector3 _pointA, Vector3 _pointB) {
            start = _pointA;
            end = _pointB;
        }
        #endregion
        
        #region Operators
        public static bool operator == (Line _a, Line _b) { return _a.start == _b.start && _a.end == _b.end; }

        public static bool operator != (Line _a, Line _b) { return !(_a.start == _b.start && _a.end == _b.end); }
    
        readonly public override bool Equals(object obj) {
            if (obj == null || obj is not Line) { return false; }
            return (Line)obj == this;
        }

        readonly public override int GetHashCode() { return System.HashCode.Combine(start, end); }
        #endregion

        #region Casting
        public static implicit operator Line2D(Line _line) => new(_line.start, _line.end);
        #endregion

        #region Line
        public readonly float GetLength() { return Vector3.Distance(start, end); }

        public readonly Vector3 GetVector() => end - start;
        public readonly Vector3 GetDirection() => GetVector().normalized;

        public bool IsPositionInBoundingBox(Vector3 _position) {
            throw new NotImplementedException();
        }
        #endregion

        public void Rotate(Quaternion _rotation) {
            start = _rotation * start;
            end = _rotation * end;
        }

        public void Translate(Vector3 _translation) {
            start += _translation;
            end += _translation;
        }

        #region IInterpolationShape
        public readonly Vector3 EvaluatePositionAtTime(float _time) { return Vector3.Lerp(start, end, _time); }
        public readonly Vector3 EvaluateTangentAtTime(float _time) { return GetDirection(); }
        #endregion
    }
}