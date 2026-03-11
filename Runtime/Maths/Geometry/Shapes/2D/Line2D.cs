using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    public struct Line2D : IShape2D, IInterpolationShape
    {
        public Vector2 from;
        public Vector2 to;

        readonly public PointTransform2D Transform => PointTransform2D.Origin;

        #region Constructors
        public Line2D(Vector2 _from, Vector2 _to) {
            from = _from;
            to = _to;
        }
        #endregion

        #region Operators
        readonly public bool Equals(Line2D _other) {
            return this.from == _other.from && 
                this.to == _other.to;
        }
        public override readonly bool Equals(object obj) => this.Equals((Line2D)obj);

        public override readonly int GetHashCode() {
            unchecked {
                return HashCode.Combine(from, to);
            }
        }
        public static bool operator ==(Line2D _a, Line2D _b) => _a.Equals(_b);
        public static bool operator !=(Line2D _a, Line2D _b) => !_a.Equals(_b);
        #endregion

        #region Casting
        public static explicit operator Line(Line2D _line) => new(_line.from, _line.to);
        #endregion

        #region Shape2D
        public void Rotate(float _rotation)
        {
            Quaternion rotation = Quaternion.AngleAxis(_rotation, Vector3.forward);
            from = rotation * from;
            to = rotation * to;
        }

        public void Translate(Vector2 _translation)
        {
            from += _translation;
            to += _translation;
        }
        #endregion

        #region Line2D
        public readonly float GetLength() { return Vector2.Distance(from, to); }

        public readonly Vector2 GetVector() => to - from;
        public readonly Vector2 GetDirection() => GetVector().normalized;
        
        public void FlipDirection() { (to, from) = (from, to); }
        #endregion

        #region IInterpolationShape
        public readonly Vector2 EvaluatePositionAtTime(float _t) => Vector2.Lerp(from, to, _t);
        public readonly Vector2 EvaluateTangentAtTime(float _t) => GetDirection();

        readonly Vector3 IInterpolationShape.EvaluatePositionAtTime(float _t) => this.EvaluatePositionAtTime(_t);
        readonly Vector3 IInterpolationShape.EvaluateTangentAtTime(float _t) => this.EvaluateTangentAtTime(_t);
        #endregion
    }
}