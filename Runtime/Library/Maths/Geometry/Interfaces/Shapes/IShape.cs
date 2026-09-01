using UnityEngine;

namespace MaroonSeal.Maths.Geometry {

    public interface IShape : IGeometry {}
    
    public interface IShape<TTransform, TVector> : IShape where TTransform : ITransform<TVector>
    {
        public TTransform Transform { get; set; }
        public bool ContainsPoint(TVector _point);
    }
}