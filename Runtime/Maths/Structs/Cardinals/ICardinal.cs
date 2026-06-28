using UnityEngine;

namespace MaroonSeal.Maths
{
    public interface ICardinal
    {
        public int Index { get; }
        public float Theta { get; }
        
        static protected int FromThetaToIndex(float _theta, int _maxIndex)
        {
            float segmentSize = 360.0f / _maxIndex;
            return (int)((_theta - segmentSize/2.0f) / segmentSize);
        }

        static protected int FromVectorToIndex(Vector2 _vector, int _maxIndex)
        {
            float theta = Mathf.Atan2(_vector.y, _vector.x) * Mathf.Rad2Deg;
            return FromThetaToIndex(theta, _maxIndex);
        }
    }

    static public class ICardinalExtensions {
        static public Vector2 ToVector2(this ICardinal _cardinal)
        {
            float rads = _cardinal.Theta * Mathf.Deg2Rad;
            return new(Mathf.Cos(rads), Mathf.Sin(rads));
        }

        static public Vector3 ToVector3(this ICardinal _cardinal)
        {
            float rads = _cardinal.Theta * Mathf.Deg2Rad;
            return new(Mathf.Cos(rads), 0.0f, Mathf.Sin(rads));
        }

        static public Quaternion ToRotation(this ICardinal _cardinal)
        {
            return Quaternion.Euler(0.0f, _cardinal.Theta, 0.0f);
        }
    }
}
