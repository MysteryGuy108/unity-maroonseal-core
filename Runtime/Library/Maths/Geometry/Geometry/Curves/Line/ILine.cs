using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface ILine<TVector> : IArcLengthCurve<TVector>
    {
        public void SetPoints(TVector _p1, TVector _p2);
        public TVector GetDelta();
    }
}
