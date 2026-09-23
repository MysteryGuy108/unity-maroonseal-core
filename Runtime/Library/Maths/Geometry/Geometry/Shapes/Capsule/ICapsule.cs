using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface ICapsule<TVector, TTransform> : IShape<TVector, TTransform>, ISDF<TVector>
        where TTransform : ITransform<TVector>
    {
    
    }
}
