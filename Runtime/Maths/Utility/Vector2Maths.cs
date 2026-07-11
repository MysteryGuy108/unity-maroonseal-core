using UnityEngine;

namespace MaroonSeal.Maths
{
    public struct Vector2Maths
    {
        static public Vector2 Clamp(Vector2 _value, Vector2 _min, Vector2 _max) => 
            new(Mathf.Clamp(_value.x, _min.x, _max.x), Mathf.Clamp(_value.y, _min.y, _max.y));

        static public Vector2 Repeat(Vector2 _a, Vector2 _b) => 
            new(Mathf.Repeat(_a.x, _b.x), Mathf.Repeat(_a.y, _b.y));

        static public Vector2Int CeilToInt(Vector2 _value) => 
            new(Mathf.CeilToInt(_value.x), Mathf.CeilToInt(_value.y));

        static public Vector2Int RoundToInt(Vector2 _value) => 
            new(Mathf.RoundToInt(_value.x), Mathf.RoundToInt(_value.y));

        static public Vector2Int FloorToInt(Vector2 _value) => 
            new (Mathf.FloorToInt(_value.x), Mathf.FloorToInt(_value.y));

        static public Vector2 FromRadians(float _radians, float _radius) => new Vector2(Mathf.Cos(_radians), Mathf.Sin(_radians)) * _radius;
        static public Vector2 FromDegrees(float _degrees, float _radius) => FromRadians(_degrees * Mathf.Deg2Rad, _radius);
    }
}
