using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    public struct Line2D : IShape2D, IInterpolationShape
    {
        public Vector2 p1;
        public Vector2 p2;

        readonly public PointTransform2D Transform => PointTransform2D.Origin;

        #region Constructors
        public Line2D(Vector2 _from, Vector2 _to) {
            p1 = _from;
            p2 = _to;
        }
        #endregion

        #region Operators
        readonly public bool Equals(Line2D _other) {
            return this.p1 == _other.p1 && 
                this.p2 == _other.p2;
        }
        public override readonly bool Equals(object obj) => this.Equals((Line2D)obj);

        public override readonly int GetHashCode() {
            unchecked {
                return HashCode.Combine(p1, p2);
            }
        }
        public static bool operator ==(Line2D _a, Line2D _b) => _a.Equals(_b);
        public static bool operator !=(Line2D _a, Line2D _b) => !_a.Equals(_b);
        #endregion

        #region Casting
        public static explicit operator Line(Line2D _line) => new(_line.p1, _line.p2);
        #endregion

        #region Shape2D
        public void Rotate(float _rotation)
        {
            Quaternion rotation = Quaternion.AngleAxis(_rotation, Vector3.forward);
            p1 = rotation * p1;
            p2 = rotation * p2;
        }

        public void Translate(Vector2 _translation)
        {
            p1 += _translation;
            p2 += _translation;
        }
        #endregion

        #region Line2D
        public readonly float GetLength() { return Vector2.Distance(p1, p2); }

        public readonly Vector2 GetVector() => p2 - p1;
        public readonly Vector2 GetDirection() => GetVector().normalized;
        
        public void FlipDirection() { (p2, p1) = (p1, p2); }
        #endregion

        #region IInterpolationShape
        public readonly Vector2 EvaluatePositionAtTime(float _t) => Vector2.Lerp(p1, p2, _t);
        public readonly Vector2 EvaluateTangentAtTime(float _t) => GetDirection();

        readonly Vector3 IInterpolationShape.EvaluatePositionAtTime(float _t) => this.EvaluatePositionAtTime(_t);
        readonly Vector3 IInterpolationShape.EvaluateTangentAtTime(float _t) => this.EvaluateTangentAtTime(_t);
        #endregion
    }
}