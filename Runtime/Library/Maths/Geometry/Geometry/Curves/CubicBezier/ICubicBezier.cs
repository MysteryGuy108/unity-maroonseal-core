using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface ICubicBezier<TVector> : ICurve<TVector>
    {
        public void SetPoints(TVector _anchorA, TVector _controlA, TVector _controlB, TVector _anchorB);
    }
}
