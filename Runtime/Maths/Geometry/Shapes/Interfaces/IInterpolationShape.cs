using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes 
{
    public interface IInterpolationShape {
        public Vector3 EvaluatePositionAtTime(float _t);
        public Vector3 EvaluateTangentAtTime(float _t);

        static public Vector3 LerpBetweenPositions(IInterpolationShape _a, IInterpolationShape _b, float _t0, float _t1) {
            return Vector3.Lerp(_a.EvaluatePositionAtTime(_t0), _b.EvaluatePositionAtTime(_t0), _t1);
        }

        static public Vector3 LerpBetweenPositions(IInterpolationShape _a, IInterpolationShape _b, float _t) {
            return LerpBetweenPositions(_a, _b, _t, _t);
        }
    }
}