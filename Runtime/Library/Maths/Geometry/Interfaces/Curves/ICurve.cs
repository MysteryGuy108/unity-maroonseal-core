using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface ICurve<TVector> : IGeometry
    {
        public TVector EvaluatePositionAtTime(float _t);
        public TVector EvaluateTangentAtTime(float _t);
    }
}
