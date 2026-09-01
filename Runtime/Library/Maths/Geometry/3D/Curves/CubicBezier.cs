using System;
using UnityEngine;


namespace MaroonSeal.Maths.Geometry {
    [System.Serializable]
    public struct CubicBezier : ICurve3D
    {
        public Vector3 anchorA;
        public Vector3 controlA;
        public Vector3 controlB;
        public Vector3 anchorB;

        #region Constructors
        public CubicBezier(Vector3 _anchorA, Vector3 _controlA, Vector3 _controlB, Vector3 _anchorB)
        {
            anchorA = _anchorA;
            controlA = _controlA;
            controlB = _controlB;
            anchorB = _anchorB;
        }
        #endregion

        #region Operators
        public static bool operator ==(CubicBezier _a, CubicBezier _b)
        {
            return _a.anchorA == _b.anchorA &&
                _a.controlA == _b.controlA &&
                _a.controlB == _b.controlB &&
                _a.anchorB == _b.anchorB;
        }

        public static bool operator !=(CubicBezier _a, CubicBezier _b)
        {
            return !(_a.anchorA == _b.anchorA &&
                _a.controlA == _b.controlA &&
                _a.controlB == _b.controlB &&
                _a.anchorB == _b.anchorB);
        }

        readonly public override bool Equals(object obj) { return ((CubicBezier)obj == this) && obj != null && obj is CubicBezier; }
        readonly public override int GetHashCode() { return System.HashCode.Combine(anchorA, controlA, controlB, anchorB); }
        #endregion

        #region Cubic Bezier
        public readonly Vector3 EvaluatePointAtTime(float _t)
        {
            float tm = 1.0f - _t;
            float tm2 = tm * tm;
            float tm3 = tm * tm * tm;

            float t2 = _t * _t;
            float t3 = _t * _t * _t;
            return (tm3 * anchorA) + (3 * tm2 * _t * controlA) + (3 * tm * t2 * controlB) + (t3 * anchorB);
        }

        public readonly Vector3 EvaluateTangentAtTime(float _t)
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