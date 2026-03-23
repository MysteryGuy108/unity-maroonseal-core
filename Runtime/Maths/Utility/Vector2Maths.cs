using UnityEngine;

namespace MaroonSeal.Maths
{
    public struct Vector2Maths
    {
        static public Vector2 Clamp(Vector2 _value, Vector2 _min, Vector2 _max)
        {
            return new(
                Mathf.Clamp(_value.x, _min.x, _max.x),
                Mathf.Clamp(_value.y, _min.y, _max.y)
            );
        }

        static public Vector2 Repeat(Vector2 _a, Vector2 _b)
        {
            return new(
                Mathf.Repeat(_a.x, _b.x),
                Mathf.Repeat(_a.y, _b.y)
            );
        }
    }
}
