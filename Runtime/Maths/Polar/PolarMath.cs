using UnityEngine;

namespace MaroonSeal.Maths
{
    public static class PolarMath
    {
        public static Vector2 ToCartesian(float _radius, float _radians) {
            return new Vector2(Mathf.Cos(_radians), Mathf.Sin(_radians)) * _radius;
        }

        public static Vector3 GetCartesianPosition(ITransform _transform, float _radius, float _radians) {
            return _transform.TransformPosition(ToCartesian(_radius, _radians));
        }

        public static Vector2 GetCircleTangent(float _radians) {
            _radians += Mathf.PI*0.5f;
            return new Vector2(Mathf.Cos(_radians), Mathf.Sin(_radians));   
        }
    }
}
