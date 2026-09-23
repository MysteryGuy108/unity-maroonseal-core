using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry 
{
    public interface IArc<TVector, TTransform> : IPolarCurve<TVector>, IShape<TVector, TTransform>
        where TTransform : ITransform<TVector>
    {
        public float Length { get; }
        public float DegreesDelta { get; }
    }
}
