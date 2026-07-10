using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes 
{
    public interface IPolarShape<TVector> : ICurve<TVector>
    {
        public TVector EvaluatePointAtTheta(float _theta);
        public TVector EvaluateTangentAtTheta(float _theta);
    }

    public interface IPolarShape2D : IPolarShape<Vector2> {}
    public interface IPolarShape3D : IPolarShape<Vector3> {}
}