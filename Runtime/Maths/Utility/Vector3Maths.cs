using UnityEngine;

namespace MaroonSeal.Maths
{
    public struct Vector3Maths
    {
        static public Vector3 Clamp(Vector3 _value, Vector3 _min, Vector3 _max)
        {
            return new(
                Mathf.Clamp(_value.x, _min.x, _max.x),
                Mathf.Clamp(_value.y, _min.y, _max.y),
                Mathf.Clamp(_value.z, _min.z, _max.z)
            );
        }

        static public Vector3 Repeat(Vector3 _a, Vector3 _b)
        {
            return new(
                Mathf.Repeat(_a.x, _b.x),
                Mathf.Repeat(_a.y, _b.y),
                Mathf.Repeat(_a.z, _b.z)
            );
        }

        static public Vector3 Abs(Vector3 _v)
        {
            return new(
                Mathf.Abs(_v.x),
                Mathf.Abs(_v.y),
                Mathf.Abs(_v.z)
            );
        }

        static public Vector3Int RoundToInt(Vector3 _v)
        {
            return new(
                Mathf.RoundToInt(_v.x),
                Mathf.RoundToInt(_v.y),
                Mathf.RoundToInt(_v.z)
            );
        }
    }
}
