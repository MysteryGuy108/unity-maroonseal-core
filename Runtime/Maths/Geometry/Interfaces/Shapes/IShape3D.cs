using UnityEngine;

namespace MaroonSeal.Maths.Geometry 
{
    public interface IShape3D : IShape<Transform3D, Vector3> {}

    public static class IShape3DExtensions
    {
        public static void Rotate(this IShape3D _shape, Quaternion _rotation)
        {
            Transform3D transform = _shape.Transform;
            transform.rotation *= _rotation;
            _shape.Transform = transform;
        }

        public static void Translate(this IShape3D _shape, Vector3 _translation)
        {
            Transform3D transform = _shape.Transform;
            transform.position += _translation;
            _shape.Transform = transform;
        }
    }
}
