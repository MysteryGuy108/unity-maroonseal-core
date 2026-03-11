using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {

    public interface IShape {}
    
    public interface IShape<TTransform> : IShape where TTransform : IPointTransform
    {
        public TTransform Transform { get; }
    }

    public interface IShape3D : IShape<PointTransform> {
        public void Rotate(Quaternion _rotation);
        public void Translate(Vector3 _translation);   
    }

    public interface IShape2D : IShape<PointTransform2D> {
        public void Rotate(float _rotation);
        public void Translate(Vector2 _translation);   
    }
}