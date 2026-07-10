using UnityEngine;

namespace MaroonSeal.Maths.Geometry {

    public interface IShape : IGeometry {}
    
    public interface IShape<TTransform, TVector> : IShape where TTransform : ITransform
    {
        public TTransform Transform { get; set; }
        public bool Contains(TVector _point);
    }
}