using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface ITriangle<TVector, TTransform, TEdge> : IPolygon<TVector, TTransform, TEdge>, ISDF<TVector>
        where TTransform : ITransform<TVector>
        where TEdge : ICurve<TVector>
    {
    
    }
}
