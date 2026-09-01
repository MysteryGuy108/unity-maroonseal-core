using UnityEngine;

namespace MaroonSeal {
    public static class Vector2Extensions
    {
        #region Conversions
        public static Vector3 ToXY(this Vector2 _current) { return new(_current.x, _current.y, 0.0f); }

        public static Vector3 ToXZ(this Vector2 _current) { return new(_current.x, 0.0f, _current.y); }

        public static Vector3 ToZY(this Vector2 _current) { return new(0.0f, _current.y, _current.x); }

        public static Vector2 With(this Vector2 _current, float? x = null, float? y = null) { return new Vector2(x ?? _current.x, y ?? _current.y); }
        #endregion

        #region Maths Operators
        public static Vector2 Abs(this Vector2 _current) { return new(Mathf.Abs(_current.x), Mathf.Abs(_current.y)); }

        public static Vector2 Max(this Vector2 _current, Vector2 _max) { return new(Mathf.Max(_current.x, _max.x), Mathf.Max(_current.y, _max.y)); }
        public static Vector2 Max(this Vector2 _current, float _max) { return _current.Max(Vector2.one * _max); }

        public static Vector2 Min(this Vector2 _current, Vector2 _min) { return new(Mathf.Min(_current.x, _min.x), Mathf.Max(_current.y, _min.y)); }
        public static Vector2 Min(this Vector2 _current, float _min) { return _current.Min(Vector2.one * _min); }

        public static Vector2 Clamp(this Vector2 _current, Vector2 _min, Vector2 _max) {
            return new(Mathf.Clamp(_current.x, _min.x, _max.x), Mathf.Clamp(_current.y, _min.y, _max.y));
        }

        public static Vector2 Clamp(this Vector2 _current, float _min, float _max) {
            return _current.Clamp(Vector2.one * _min, Vector2.one * _max);
        }
        #endregion
    }
}
