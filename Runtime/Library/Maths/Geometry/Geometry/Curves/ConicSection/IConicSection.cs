using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface IConicSection<TVector, TTransform> : IShape<TVector, TTransform>, IPolarCurve<TVector>, ISDF<TVector>
        where TTransform : ITransform<TVector>
    {
        public float Eccentricity { get; }
    }
}
