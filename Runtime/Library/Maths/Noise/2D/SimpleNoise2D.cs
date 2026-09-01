using UnityEngine;

namespace MaroonSeal.Maths
{
    public class SimpleNoise2D : Noise2D
    {
        protected override float GetRawSample(Vector2 _sample)
        {
            NoiseHasher hasher = new(Seed);
            hasher.Add(_sample.x); hasher.Add(_sample.y);
            uint hash = hasher.Finish();

            return (float)hash / uint.MaxValue;
        }
    }
}
