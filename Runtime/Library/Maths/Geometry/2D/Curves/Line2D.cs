using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry {
    [System.Serializable]
    public struct Line2D : ICurve2D, IEquatable<Line2D>
    {
        public Vector2 p1;
        public Vector2 p2;

        readonly public float Length => Vector2.Distance(p1, p2);

        #region Constructors
        public Line2D(Vector2 _from, Vector2 _to) {
            p1 = _from;
            p2 = _to;
        }
        #endregion

        #region IEquatable
        readonly public bool Equals(Line2D _other) => this.p1 == _other.p1 && this.p2 == _other.p2;
        readonly public override bool Equals(object obj) => obj != null && obj is Line2D && ((Line2D)obj).Equals(this);

        public override readonly int GetHashCode() {
            unchecked { return HashCode.Combine(p1, p2); }
        }
        #endregion

        #region Operators
        public static bool operator ==(Line2D _a, Line2D _b) => _a.Equals(_b);
        public static bool operator !=(Line2D _a, Line2D _b) => !_a.Equals(_b);
        #endregion

        #region Casting
        public static explicit operator Line(Line2D _line) => new(_line.p1, _line.p2);
        #endregion

        #region Line2D
        public readonly Vector2 GetVector() => p2 - p1;
        public readonly Vector2 GetDirection() => GetVector().normalized;
        
        public void FlipDirection() => (p2, p1) = (p1, p2);
        #endregion

        #region IInterpolationShape
        public readonly Vector2 EvaluatePointAtTime(float _t) => Vector2.Lerp(p1, p2, _t);
        public readonly Vector2 EvaluateTangentAtTime(float _t) => GetDirection();
        #endregion
    }
}