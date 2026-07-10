using UnityEngine;

namespace MaroonSeal.Maths.Geometry 
{
    public interface IPolarShape<TVector> : ICurve<TVector>
    {
        public TVector EvaluatePointAtTheta(float _theta);
        public TVector EvaluateTangentAtTheta(float _theta);
    }

    public interface IPolarShape2D : IPolarShape<Vector2>, ICurve2D {}
    public interface IPolarShape3D : IPolarShape<Vector3>, ICurve3D {}
}