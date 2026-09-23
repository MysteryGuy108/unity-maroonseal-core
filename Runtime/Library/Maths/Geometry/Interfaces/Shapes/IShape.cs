using UnityEngine;

namespace MaroonSeal.Maths.Geometry {
    
    public interface IShape<TVector, TTransform> : IGeometry where TTransform : ITransform<TVector>
    {
        public TTransform Transform { get; set; }
        public bool ContainsPoint(TVector _point);
    }
}