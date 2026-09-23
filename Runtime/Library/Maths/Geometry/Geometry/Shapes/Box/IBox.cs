using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface IBox<TVector, TTransform> : IShape<TVector, TTransform>, ISDF<TVector>
        where TTransform : ITransform<TVector>
    {
    
    }
}
