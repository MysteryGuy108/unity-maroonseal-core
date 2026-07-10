using UnityEngine;

namespace MaroonSeal {
    public static class Vector3Extensions
    {
        #region Conversions
        public static Vector3 With(this Vector3 _current, float? x = null, float? y = null, float? z = null) {
            return new Vector3(x ?? _current.x, y ?? _current.y, z ?? _current.z);
        }

        public static Vector2 ToXY(this Vector3 _current) { return new(_current.x, _current.y); }

        public static Vector2 ToXZ(this Vector3 _current) { return new(_current.x, _current.z); }

        public static Vector2 ToZY(this Vector3 _current) { return new(_current.z, _current.y); }
        #endregion

        #region Maths Operators
        public static Vector3 Abs(this Vector3 _current) {
            return new(Mathf.Abs(_current.x), Mathf.Abs(_current.y), Mathf.Abs(_current.z));
        }

        public static Vector3 Max(this Vector3 _current, Vector3 _max) {
            return new(Mathf.Max(_current.x, _max.x), Mathf.Max(_current.y, _max.y), Mathf.Max(_current.z, _max.z));
        }

        public static Vector3 Max(this Vector3 _current, float _max) {
            return _current.Max(Vector3.one * _max);
        }

        public static Vector3 Min(this Vector3 _current, Vector3 _min) {
            return new(Mathf.Min(_current.x, _min.x), Mathf.Min(_current.y, _min.y), Mathf.Min(_current.z, _min.z));
        }

        public static Vector3 Min(this Vector3 _current, float _min) {
            return _current.Min(Vector3.one * _min);
        }

        public static bool IsInBounds(this Vector3 _current, Vector3 _pointA, Vector3 _pointB)
        {
            Vector3 min = _pointA.Min(_pointB);
            Vector3 max = _pointA.Max(_pointB);

            return min.x <= _current.x && _current.x <= max.x &&
                    min.y <= _current.y && _current.y <= max.y &&
                    min.z <= _current.z && _current.z <= max.z;
        }

        public static Vector3 Clamp(this Vector3 _current, Vector3 _min, Vector3 _max)
        {
            return new(Mathf.Clamp(_current.x, _min.x, _max.x), Mathf.Clamp(_current.y, _min.y, _max.y), Mathf.Clamp(_current.z, _min.z, _max.z));
        }

        public static Vector2 Clamp(this Vector3 _current, float _min, float _max) {
            return _current.Clamp(Vector3.one * _min, Vector3.one * _max);
        }
        #endregion
    }
}

