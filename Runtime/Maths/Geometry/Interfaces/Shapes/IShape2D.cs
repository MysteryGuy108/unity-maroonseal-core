using UnityEngine;

namespace MaroonSeal.Maths.Geometry 
{
    public interface IShape2D : IShape<Transform2D, Vector2> {}

    public static class IShape2DExtensions
    {
        public static void Rotate(this IShape2D _shape, float _rotation)
        {
            Transform2D transform = _shape.Transform;
            transform.angle += _rotation;
            _shape.Transform = transform;
        }

        public static void Translate(this IShape2D _shape, Vector2 _translation)
        {
            Transform2D transform = _shape.Transform;
            transform.position += _translation;
            _shape.Transform = transform;
        }
    }
}
