using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface ICircle<TVector, TTransform> : IPolarCurve<TVector>, IShape<TVector, TTransform>
        where TTransform : ITransform<TVector>
    {
    
    }
}
