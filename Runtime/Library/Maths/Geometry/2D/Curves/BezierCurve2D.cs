using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    [System.Serializable]
    public struct CubicBezier2D : ICurve2D, IEquatable<CubicBezier2D>
    {
        public Vector2 anchorA;
        public Vector2 controlA;
        public Vector2 controlB;
        public Vector2 anchorB;

        #region Constructors
        public CubicBezier2D(Vector2 _anchorA, Vector2 _controlA, Vector2 _controlB, Vector2 _anchorB)
        {
            anchorA = _anchorA; controlA = _controlA; controlB = _controlB; anchorB = _anchorB;
        }
        #endregion

        #region IEquatable
        readonly public bool Equals(CubicBezier2D _other) {
            return this.anchorA == _other.anchorA &&
                this.controlA == _other.controlA &&
                this.controlB == _other.controlB &&
                this.anchorB == _other.anchorB;
        }

        readonly public override bool Equals(object obj) => obj != null && obj is CubicBezier2D && ((CubicBezier2D)obj == this);
        readonly public override int GetHashCode() {
            unchecked { return System.HashCode.Combine(anchorA, controlA, controlB, anchorB); } 
        }
        #endregion

        #region Operators
        public static bool operator ==(CubicBezier2D _a, CubicBezier2D _b) => _a.Equals(_b);

        public static bool operator !=(CubicBezier2D _a, CubicBezier2D _b) => !_a.Equals(_b);
        #endregion

        #region Cubic Bezier
        public readonly Vector2 EvaluatePointAtTime(float _t)
        {
            _t = Mathf.Clamp01(_t);
            float tm = 1.0f - _t;
            float tm2 = tm * tm;
            float tm3 = tm * tm * tm;

            float t2 = _t * _t;
            float t3 = _t * _t * _t;
            return (tm3 * anchorA) + (3 * tm2 * _t * controlA) + (3 * tm * t2 * controlB) + (t3 * anchorB);
        }

        public readonly Vector2 EvaluateTangentAtTime(float _t)
        {
            _t = Mathf.Clamp01(_t);
            float tm = 1.0f - _t;
            float tm2 = tm * tm;
            float t2 = _t * _t;

            return (3.0f * tm2 * (controlA - anchorA)) + (6.0f * tm * _t * (controlB - controlA)) + (3.0f * t2 * (anchorB - controlB));
        }
        #endregion
    }
}
