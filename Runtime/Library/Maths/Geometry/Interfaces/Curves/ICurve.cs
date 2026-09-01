using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface ICurve<TVector> : IGeometry
    {
        public TVector EvaluatePointAtTime(float _t);
        public TVector EvaluateTangentAtTime(float _t);
    }

    public interface ICurve2D : ICurve<Vector2> { }

    public interface ICurve3D : ICurve<Vector3> { }
}
