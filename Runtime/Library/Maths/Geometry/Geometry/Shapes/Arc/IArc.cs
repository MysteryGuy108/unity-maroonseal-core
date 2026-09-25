using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry 
{
    public interface IArc<TVector, TTransform> : IPolarCurve<TVector>, IShape<TVector, TTransform>, IArcLengthCurve<TVector>
        where TTransform : ITransform<TVector>
    {
        public float DegreesDelta { get; }
    }
}
