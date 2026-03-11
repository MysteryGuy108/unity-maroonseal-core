using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes 
{
    public interface IPolarShape : IInterpolationShape
    {
        public Vector3 EvaluatePositionAtTheta(float _theta);
        public Vector3 EvaluateTangentAtTheta(float _theta);

        public static Vector2 ToCartesian(float _radius, float _radians) {
            return new Vector2(Mathf.Cos(_radians), Mathf.Sin(_radians)) * _radius;
        }

        public static Vector2 GetCartesianPosition(IPointTransform _transform, float _radius, float _radians) {
            return _transform.TransformPosition(ToCartesian(_radius, _radians));
        }

        public static Vector2 GetCircleTangent(float _radians) {
            _radians += Mathf.PI*0.5f;
            return new Vector2(Mathf.Cos(_radians), Mathf.Sin(_radians));   
        }
    }
}