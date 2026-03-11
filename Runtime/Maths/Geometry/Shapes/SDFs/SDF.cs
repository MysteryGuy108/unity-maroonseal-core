using UnityEngine;

namespace MaroonSeal.Maths.Geometry.SDFs {

    public static class SDF {
        #region Operators
        public static float Union(float _d1, float _d2) { return Mathf.Min(_d1, _d2); }

        public static float Subtraction(float _d1, float _d2) { return Mathf.Max(-_d1, _d2); }

        public static float Intersection(float _d1, float _d2) { return Mathf.Max(_d1, _d2); }

        public static float Xor(float _d1, float _d2) { return Mathf.Max(Mathf.Min(_d1,_d2),-Mathf.Max(_d1,_d2)); }
        #endregion

        #region Smooth Operators
        public static float SmoothUnion(float _d1, float _d2, float _k) {
            float h = Mathf.Clamp(0.5f + 0.5f*(_d2-_d1)/_k, 0.0f, 1.0f );
            return Mathf.Lerp( _d2, _d1, h ) - _k*h*(1.0f-h);
        }

        public static float SmoothSubtraction(float _d1, float _d2, float _k) {
            float h = Mathf.Clamp( 0.5f - 0.5f*(_d2+_d1)/_k, 0.0f, 1.0f);
            return Mathf.Lerp( _d2, -_d1, h) + _k*h*(1.0f-h);
        }

        public static float SmoothIntersection(float _d1, float _d2, float _k) {
            float h = Mathf.Clamp(0.5f - 0.5f*(_d2-_d1)/_k, 0.0f, 1.0f );
            return Mathf.Lerp( _d2, _d1, h ) + _k*h*(1.0f-h);
        }
        #endregion
    }
}