using UnityEngine;

namespace MaroonSeal.Maths.Geometry 
{
    public interface IPolarCurve<TVector> : ICurve<TVector>
    {
        public TVector EvaluatePointAtTheta(float _theta);
        public TVector EvaluateTangentAtTheta(float _theta);
    }
}